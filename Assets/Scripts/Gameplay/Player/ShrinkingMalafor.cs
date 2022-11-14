using System.Collections;
using UnityEngine;

public class ShrinkingMalafor : MonoBehaviour
{
    [SerializeField] int _repeat = 2;
    [SerializeField] float[] _steps = { 0.75f, 0.5f, 0.25f, };
    [SerializeField] float _delay1 = 0.1f;
    [SerializeField] float _delay2 = 0.4f;

    public IEnumerator CO_Shrink()
    {
        for (var i = 0; i < _repeat; ++i)
        {
            foreach (var size in _steps)
            {
                transform.localScale = size * Vector3.one;
                yield return new WaitForSeconds(_delay1);
            }

            yield return new WaitForSeconds(_delay2);
        }
    }
}