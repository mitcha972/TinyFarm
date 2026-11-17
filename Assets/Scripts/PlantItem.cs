using UnityEngine;
using UnityEngine.UI;

public class PlantItem : MonoBehaviour
{
    public Plant plant;
    [SerializeField] private Text nameTxt;
    [SerializeField] private Text priceTxt;
    [SerializeField] private Text btnTxt;
    [SerializeField] private Image icon;
    [SerializeField] private Image btnImage;

    public void Setup(Plant plant)
    {
        this.plant = plant;
        nameTxt.text = plant.PlantName;
        priceTxt.text = "$" + plant.BuyPrice;
        icon.sprite = plant.Icon;
        SetSelected(false);
    }

    public void SetSelected(bool selected)
    {
        FarmManager farm = FarmManager.Instance;
        btnImage.color = selected ? farm.cancelColor : farm.buyColor;
        btnTxt.text = selected ? "Cancel" : "Buy";
    }

    public void OnClickSelect()
    {
        FarmManager.Instance.SelectPlant(this);
    }
}
