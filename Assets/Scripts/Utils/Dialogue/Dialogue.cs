using System;
using TMPro;
using UnityEngine;

[Serializable]
public class Dialogue
{
    public DialogueCharacter character;
    public DialogueFontSize fontSize = DialogueFontSize.Normal;
    public Wobble wobble = Wobble.None;
    [TextArea(3, 10)] public string line;
    public bool topOfScreen;
}

public enum DialogueFontSize
{
    Normal,
    Small,
    Large,
}

public enum Wobble
{
    None,
    Slight,
    Serious,
}