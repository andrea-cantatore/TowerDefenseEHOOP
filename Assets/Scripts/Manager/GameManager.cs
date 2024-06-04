using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int CoinCount;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EventManager.OnGamePause?.Invoke(true);
        }
    }

    private void OnEnable()
    {
        EventManager.ChangeCoins += AddCoin;
    }
    
    private void OnDisable()
    {
        EventManager.ChangeCoins -= AddCoin;
    }

    private void Awake()
    {
        instance = this;
    }
    
    public void MainMenu()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }
    
    public void AddCoin(int toChange)
    {
        CoinCount += toChange;
        UIManager.instance.coinText.text = CoinCount.ToString();
    }
}
