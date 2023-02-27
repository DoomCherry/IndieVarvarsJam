using SimpleMan.AsyncOperations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public CanvasGroup disclamer;
    public CanvasGroup mainMenu;
    public AudioSource mainMusic;

    private Coroutine _viewProcess;
    private float minStep = 0.01f;


    private void Awake()
    {
        Screen.SetResolution(1920,1080, true);
        _viewProcess = this.RepeatUntil(() => disclamer.alpha >= 1, OnDisclamer, OffDisclamerProcess, minStep);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }


    private void OffDisclamerProcess()
    {
        mainMenu.alpha = 1;
        mainMenu.interactable = true;
        mainMenu.blocksRaycasts = true;

        if(_viewProcess != null)
        StopCoroutine(_viewProcess);

        _viewProcess = this.RepeatUntil(() => disclamer.alpha <= 0, OffDisclamer, () => { mainMusic.Play(); }, minStep);
    }

    private void OnDisclamer()
    {
        OnCanvasGroup(disclamer);
    }

    private void OffDisclamer()
    {
        OffCanvasGroup(disclamer);
    }

    private void OffCanvasGroup(CanvasGroup group)
    {
        group.blocksRaycasts = false;
        group.interactable = false;

        group.alpha -= minStep;
    }

    private void OnCanvasGroup(CanvasGroup group)
    {
        group.blocksRaycasts = true;
        group.interactable = true;

        group.alpha += minStep;
    }
}
