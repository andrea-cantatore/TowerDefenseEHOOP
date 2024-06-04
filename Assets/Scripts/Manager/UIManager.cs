using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject gameOverPanel;
    public GameObject gameWinPanel;
    public GameObject gamePausePanel;
    public TMP_Text coinText;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        EventManager.OnGameEnd += GameEnd;
        EventManager.OnGamePause += GamePause;
    }

    public void GameEnd(bool isWin)
    {
        if(isWin)
        {
            gameWinPanel.SetActive(true);
        }
        else
        {
            gameOverPanel.SetActive(true);
        }
    }
    
    public void GamePause(bool isActive)
    {
        gamePausePanel.SetActive(!gamePausePanel.activeSelf);
        if (gamePausePanel.activeSelf)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }
    
    public void MainMenu()
    {
        GameManager.instance.MainMenu();
    }

    public void Resume()
    {
        gamePausePanel.SetActive(false);
        GameManager.instance.Resume();
    }

    public void Pause()
    {
        gamePausePanel.SetActive(true);
        GameManager.instance.Pause();
    }
}
