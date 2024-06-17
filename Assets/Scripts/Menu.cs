using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private Volume postProcessingVolume;

    [SerializeField]
    private TMP_Text text;

    private Canvas _menuCanvas;

    public bool IsMenuActive => _menuCanvas.enabled;

    void Awake()
    {
        _menuCanvas = GetComponentInChildren<Canvas>();
        _menuCanvas.enabled = false;
    }

    public void ToggleMenu(string message = "") 
    {
        if(!string.IsNullOrEmpty(message))
            text.SetText(message);

        _menuCanvas.enabled = !_menuCanvas.enabled; 

        postProcessingVolume.enabled = _menuCanvas.enabled;
    }
}
