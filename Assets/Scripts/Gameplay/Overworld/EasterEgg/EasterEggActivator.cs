using System.Collections;
using System.Linq;
using UnityEngine;

public class EasterEggActivator : MonoBehaviour
{
    [SerializeField] SpriteRenderer _cover;
    [SerializeField] Collider2D _oldCollider;
    [SerializeField] Collider2D _newCollider;

    bool _isRevealed;

    void Awake()
    {
        _cover.gameObject.SetActive(true);
        _newCollider.enabled = false;
    }

    public void Verify()
    {
        if (_isRevealed) return;

        var allFires = FindObjectsOfType<FireInteractor>();
        if (allFires.Any(fire => fire.isActive))
            return;

        _isRevealed = true;

        StartCoroutine(CO_Reveal());
    }

    IEnumerator CO_Reveal()
    {
        const float transitionTime = 2f;
        var t = 0f;
        var color = _cover.color;
        while (t < transitionTime)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, t / transitionTime);
            _cover.color = color;
            yield return null;
        }

        _cover.gameObject.SetActive(false);
        _newCollider.enabled = true;
        _oldCollider.enabled = false;
    }
}