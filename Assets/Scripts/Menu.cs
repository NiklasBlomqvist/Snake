using System;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    [SerializeField]
    private Game game;
    private Canvas _menuCanvas;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        GetComponentInChildren<Button>().onClick.AddListener(StartGame);
        _menuCanvas = GetComponentInChildren<Canvas>();

        Time.timeScale = 0;
        game.StartGame();
    }

    private void StartGame()
    {
        Time.timeScale = 1.0f;
        _menuCanvas.enabled = false;
    }
}
