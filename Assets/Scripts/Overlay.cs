using UnityEngine;
using UnityEngine.UI;

public class Overlay : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    public void SetFill(float fillAmount)
    {
        fillImage.fillAmount = fillAmount;
    }
}
