using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Combat : MonoBehaviour
{
    [SerializeField] Button[] _options;
    [SerializeField] PlayableDirector _playableDirector;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(_options[0].gameObject);
    }

    public void OnClick()
    {
        _playableDirector.Play();
        gameObject.SetActive(false);
    }
}