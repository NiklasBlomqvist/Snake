using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private Volume postProcessingVolume;

    [SerializeField]
    private TMP_Text menuText;

    public bool IsMenuActive => _menuCanvas.enabled;


    private Canvas _menuCanvas;

    private string _pauseMessage = "Game Paused, Press ESC to resume.";

    private string _startMessage = "Press SPACE to start a new game.";

    private string _gameOverMessage = "Game Over - Press SPACE to start a new game.";

    void Awake()
    {
        _menuCanvas = GetComponentInChildren<Canvas>();
        _menuCanvas.enabled = false;
    }

    public void PauseMenu() 
    {
        menuText.text = _pauseMessage;
        _menuCanvas.enabled = true;
        postProcessingVolume.enabled = true;
        Time.timeScale = 0;
    }

    public void HideMenu() 
    {
        _menuCanvas.enabled = false;
        postProcessingVolume.enabled = false;
        Time.timeScale = 1;
    }

    public void StartMenu() 
    {
        menuText.text = _startMessage;
        _menuCanvas.enabled = true;
        postProcessingVolume.enabled = true;
        Time.timeScale = 0;
    }

    public void GameOverMenu() 
    {
        menuText.text = _gameOverMessage;
        _menuCanvas.enabled = true;
        postProcessingVolume.enabled = true;
        Time.timeScale = 0;
    }
}
