using SimpleMan.AsyncOperations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextController : MonoBehaviour
{
    [SerializeField] private string[] _phrases;
    [SerializeField] private float _phraseTimeToWrite = 2;
    [SerializeField] private float _phraseLiveTime = 3;
    [SerializeField] private float _phraseDelay = 5;
    [SerializeField] TextMeshProUGUI _phraseContainer;

    private Coroutine _mainProcess, _colorChangeProcess;

    private float _textWriteDelay = 0.1f;
    private int _trying = 0;


    public System.Action OnTextWrite;


    private float FrameStep => _textWriteDelay / _phraseTimeToWrite;
    private int MaxFrameSteps => (int)(1f / FrameStep);
    private int GetTextPerFrame(int phraseLength) => (int)(phraseLength * FrameStep);


    private void OnEnable()
    {
        DrawRandomText();
    }

    private void DrawRandomText()
    {
        string phrase = GetRandomPhrase();
        DrawText(phrase);
    }

    private void DrawText(string text)
    {
        Color color = _phraseContainer.color;
        color.a = 1;
        _phraseContainer.color = color;

        _trying = 0;

        if (_mainProcess != null)
            StopCoroutine(_mainProcess);
        OnTextWrite?.Invoke();

        _mainProcess = this.RepeatUntil(TextIsDrawing, TextDrawing, WaitText, _textWriteDelay);




        void TextDrawing()
        {
            _trying++;
            _phraseContainer.text = text.Substring(0, GetTextPerFrame(text.Length) * _trying);
        }

        bool TextIsDrawing()
        {
            return _trying >= MaxFrameSteps;
        }

        void WaitText()
        {
            _phraseContainer.text = text;

            if (_mainProcess != null)
                StopCoroutine(_mainProcess);

            _mainProcess = this.Delay(_phraseLiveTime, HideText);
        }
    }

    private void HideText()
    {
        if (_mainProcess != null)
            StopCoroutine(_mainProcess);

        if (_colorChangeProcess != null)
            StopCoroutine(_colorChangeProcess);

        _mainProcess = this.RepeatUntil(IsTextHiden, HideTextAlpha, OnTextHide, 0.1f);
        _colorChangeProcess = this.Delay(_phraseDelay, DrawRandomText);



        void HideTextAlpha()
        {
            Color color = _phraseContainer.color;
            color.a -= Time.deltaTime * 10;
            _phraseContainer.color = color;
        }

        bool IsTextHiden()
        {
            Color color = _phraseContainer.color;
            return color.a == 0;
        }

        void OnTextHide()
        {
            if (_colorChangeProcess != null)
                StopCoroutine(_colorChangeProcess);
        }
    }

    private string GetRandomPhrase()
    {
        int value = Random.Range(0, _phrases.Length);
        return _phrases[value];
    }
}
