using UnityEngine;
using UnityEngine.UI;

public class BagItem : MonoBehaviour
{
    [SerializeField] private Text nameTxt;
    [SerializeField] private Text quantityTxt;
    [SerializeField] private Image icon;

    public void Setup(Plant plant, int amount)
    {
        nameTxt.text = plant.PlantName;
        icon.sprite = plant.Icon;
        SetAmount(amount);
    }

    public void SetAmount(int amount)
    {
        quantityTxt.text = amount.ToString();
    }
}
