using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;
    [SerializeField]
    private GameObject _pausePanel;
    
    [SerializeField]
    private Animator _pauseAnimator;

    public void GameOver()
    {
        _isGameOver = true;
    }

    private void Start()
    {
        _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
            SceneManager.LoadScene("Game");

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (Input.GetKeyDown(KeyCode.P))
            PauseGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        _pausePanel.SetActive(true);
        _pauseAnimator.SetBool("startAnimation", true);        
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        _pausePanel.SetActive(false);
        _pauseAnimator.SetBool("startAnimation", false);  
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        _pausePanel.SetActive(false);
        SceneManager.LoadScene("Main_Menu");
        _pauseAnimator.SetBool("startAnimation", false);
    }
}
