using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class StallItem : MonoBehaviour
{
    [SerializeField] private Plant plant;
    private FarmManager fm;
    [SerializeField] private StallManager stallManager;
    [SerializeField] private InputField inputField;
    [SerializeField] private GameObject marketManager;
    public GameObject contentArea;
    [SerializeField] private Text nameTxt;
    [SerializeField] private Text priceTxt;
    [SerializeField] private Text quantityTxt;
    [SerializeField] private Text btnTxt;
    [SerializeField] private Text inputText;
    [SerializeField] private Image icon;
    [SerializeField] private Sprite blankIcon;
    [SerializeField] private int quantity;
    [FormerlySerializedAs("percent")] [SerializeField, Range(-20, 20)] private int percentBonus;
    [SerializeField] private bool isActive;

    private void Start()
    {
        fm = FarmManager.Instance;
        RefreshPrice();
    }

    public void Setup(StallManager manager, GameObject market, GameObject overlay)
    {
        stallManager = manager;
        marketManager = market;
        contentArea = overlay;
    }

    public void SellOne()
    {
        if (!isActive || plant == null || quantity <= 0) return;
        // Goods were already removed from the bag when the stall started.
        fm.AddMoney(SalePrice);
        quantity--;
        quantityTxt.text = quantity.ToString();
        inputText.text = quantity.ToString();
        if (quantity == 0) RemoveStall();
    }

    public void SellPlant()
    {
        SellOne();
    }

    public void ChoosePlant()
    {
        if (isActive || stallManager == null) return;
        marketManager.SetActive(true);
        contentArea.SetActive(true);
        contentArea.GetComponent<StallSelectionOverlay>().tab = gameObject;
        stallManager.selectedStall = this;
        gameObject.SetActive(false);
    }

    public void GetPlant(Plant plant)
    {
        if (isActive || plant == null) return;
        this.plant = plant;
        icon.sprite = plant.Icon;
        nameTxt.text = plant.PlantName;
        RefreshPrice();
    }

    public void GetQuantity()
    {
        if (isActive) return;
        quantity = int.TryParse(inputField.text, out int parsed) && parsed > 0 ? parsed : 0;
        quantityTxt.text = quantity.ToString();
    }

    public void GetPercent(int percentBonus)
    {
        if (isActive) return;
        this.percentBonus = Mathf.Clamp(percentBonus, -20, 20);
        RefreshPrice();
    }

    private int SalePrice => Mathf.RoundToInt(plant.SellPrice * (1 + Mathf.Clamp(percentBonus, -20, 20) / 100f));

    private void RefreshPrice()
    {
        percentBonus = Mathf.Clamp(percentBonus, -20, 20);
        string bonus = percentBonus.ToString("+0;-0;0") + "%";
        priceTxt.text = plant == null ? bonus : "$" + SalePrice + " (" + bonus + ")";
    }

    public void SetUpStall()
    {
        if (isActive) return;
        GetQuantity();
        if (plant == null || quantity <= 0 || !fm.TryRemovePlant(plant, quantity)) return;
        isActive = true;
        btnTxt.text = "Close";
        inputField.gameObject.SetActive(false);
        inputText.text = quantity.ToString();
    }

    public void RemoveStall()
    {
        if (isActive && quantity > 0) fm.AddPlant(plant, quantity);
        isActive = false;
        plant = null;
        quantity = 0;
        percentBonus = 0;
        quantityTxt.text = "-";
        priceTxt.text = "-";
        icon.sprite = blankIcon;
        nameTxt.text = "Crop..";
        btnTxt.text = "Start";
        inputText.text = "";
        inputField.text = "";
        inputField.gameObject.SetActive(true);
    }

    public void ButtonClick()
    {
        if (isActive) RemoveStall();
        else SetUpStall();
    }
}
