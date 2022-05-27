using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KLD_TutoDialogParent : MonoBehaviour
{
    Color white = Color.white;
    Color noAlpha = new Color(1f, 1f, 1f, 0f);

    [SerializeField] float fadeTime = 0.2f;

    [SerializeField] Image[] dialogs;

    public void ShowDialog(int _index)
    {
        StartCoroutine(DialogBlend(fadeTime, dialogs[_index], noAlpha, white));
    }

    public void ShowDialog(int _index, float _delay)
    {
        StartCoroutine(WaitAndShowDialog(_index, _delay));
    }

    IEnumerator WaitAndShowDialog(int _index, float _delay)
    {
        yield return new WaitForSeconds(_delay);
        StartCoroutine(DialogBlend(fadeTime, dialogs[_index], noAlpha, white));
    }

    public void NextDialog(int _endedIndex)
    {
        StartCoroutine(NextDialogCoroutine(_endedIndex));
    }

    public void HideDialog(int _index)
    {
        StartCoroutine(DialogBlend(fadeTime, dialogs[_index], white, noAlpha));
    }

    IEnumerator NextDialogCoroutine(int _endedIndex)
    {
        StartCoroutine(DialogBlend(fadeTime, dialogs[_endedIndex], white, noAlpha));
        yield return new WaitForSeconds(fadeTime);
        StartCoroutine(DialogBlend(fadeTime, dialogs[_endedIndex + 1], noAlpha, white));
    }

    IEnumerator DialogBlend(float _time, Image _image, Color _startColor, Color _endColor)
    {
        float t = 0f;

        while (t < _time)
        {
            _image.color = Color.Lerp(_startColor, _endColor, t / _time);
            yield return null;
            t += Time.deltaTime;
        }
        _image.color = _endColor;
    }
}
