using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueImage : MonoBehaviour
{
    [SerializeField] Image talkingHead;
    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text contents;
    [SerializeField] Image _downArrow;
    [SerializeField] float textSpeed = 50;
    [SerializeField] float spriteSpeed = 10;
    [SerializeField] float timeBetweenSentences = 0.5f;

    Dialogue _currentDialogue;
    Coroutine _coroutine;

    public bool isDone =>
        contents.maxVisibleCharacters >= _currentDialogue.line.Length - 1;

    public void SkipToEndOfLine()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        contents.maxVisibleCharacters = _currentDialogue.line.Length;
        _downArrow.gameObject.SetActive(true);
    }

    public void TypeNextLine(Dialogue dialogue)
    {
        _coroutine = StartCoroutine(CO_TypeNextLine(dialogue));
    }

    public void StopCoroutine()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    IEnumerator CO_TypeNextLine(Dialogue dialogue)
    {
        _currentDialogue = dialogue;

        _downArrow.gameObject.SetActive(false);
        talkingHead.sprite = _currentDialogue.character.mouthClosedSprite;
        title.text = _currentDialogue.character.characterName;
        InitializeContents(_currentDialogue);

        if (_currentDialogue.wobble != Wobble.None)
        {
            contents.maxVisibleCharacters = _currentDialogue.line.Length;
            while (true)
            {
                WobbleContents(_currentDialogue.wobble);
                yield return null;
            }
        }

        var sentences = SplitIntoSentences(_currentDialogue.line);
        contents.maxVisibleCharacters = 0;
        for (var i = 0; i < sentences.Count; ++i)
        {
            var sentence = sentences[i];
            var t = 0f;
            var charIndex = 0;
            while (charIndex < sentence.Length &&
                   contents.maxVisibleCharacters < _currentDialogue.line.Length)
            {
                t += Time.deltaTime;

                var newCharIndex = Mathf.Clamp(Mathf.CeilToInt(t * textSpeed), 0, sentence.Length);
                contents.maxVisibleCharacters += newCharIndex - charIndex;
                charIndex = newCharIndex;

                talkingHead.sprite = Mathf.Floor(t * spriteSpeed) % 2 == 0
                    ? _currentDialogue.character.mouthClosedSprite
                    : _currentDialogue.character.mouthOpenSprite;

                yield return null;
            }

            talkingHead.sprite = _currentDialogue.character.mouthClosedSprite;

            if (i < sentences.Count - 1)
                yield return new WaitForSeconds(timeBetweenSentences);
        }

        _downArrow.gameObject.SetActive(true);
    }

    void WobbleContents(Wobble wobble)
    {
        if (wobble == Wobble.None) return;

        contents.ForceMeshUpdate();
        var mesh = contents.mesh;
        var vertices = mesh.vertices;
        var multiplier = wobble == Wobble.Slight ? 0.5f : 1f;
        for (var i = 0; i < vertices.Length; i++)
        {
            var offset = Time.time + i;
            vertices[i] += new Vector3(
                Mathf.Sin(offset * 50) * multiplier,
                Mathf.Cos(offset * 25) * multiplier
            );
        }

        mesh.vertices = vertices;
        contents.canvasRenderer.SetMesh(mesh);
    }

    void InitializeContents(Dialogue dialogue)
    {
        contents.fontSize = dialogue.fontSize switch
        {
            DialogueFontSize.Small => 8,
            DialogueFontSize.Normal => 12,
            DialogueFontSize.Large => 16,
            _ => throw new ArgumentOutOfRangeException(),
        };

        contents.fontStyle = dialogue.fontStyle;
        contents.text = _currentDialogue.line;
    }

    static List<string> SplitIntoSentences(string line)
    {
        List<string> sentences = new();
        var sentence = "";
        foreach (var character in line)
        {
            if (character is not ('.' or '!' or '?'))
            {
                sentence += character;
                continue;
            }

            // account for consecutive puncutation marks
            if (sentence.Length == 0 && sentences.Count > 0)
            {
                sentences[^1] += character;
            }
            else
            {
                sentence += character;
                sentences.Add(sentence);
                sentence = "";
            }
        }

        if (sentence.Length > 0)
            sentences.Add(sentence);

        return sentences;
    }
}