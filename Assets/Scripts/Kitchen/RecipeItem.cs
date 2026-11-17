using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class RecipeItem : MonoBehaviour
{
    private FarmManager fm;
    [FormerlySerializedAs("precent")] [SerializeField] private int percentBonus;
    [SerializeField] private Transform HGL;
    public List<Order> orders;
    [SerializeField] private Text nameTxt;
    [SerializeField] private Text priceTxt;
    [SerializeField] private Text btnTxt;
    [SerializeField] private Button btn;
    [SerializeField] private Image bkg;
    public float bakeTime;
    public string recipeName;
    [SerializeField] private bool isBaking;
    private float timer;
    [SerializeField] private Plant outputPlant;

    public void Setup(RecipeData recipe)
    {
        orders = recipe.orders;
        recipeName = recipe.recipeName;
        bakeTime = recipe.bakeTime;
        outputPlant = recipe.OutputPlant;
    }

    private void Start()
    {
        timer = bakeTime;
        fm = FarmManager.Instance;
        // Combine repeated ingredients so the availability check uses the full amount.
        var lines = new List<Order>();
        foreach (Order order in orders)
        {
            Order existing = lines.Find(line => line.plant == order.plant);
            if (existing == null) lines.Add(new Order(order.plant, order.quantity));
            else existing.quantity += order.quantity;
        }
        orders = lines;
        IngredientSlot[] slots = HGL.GetComponentsInChildren<IngredientSlot>(true);

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].gameObject.SetActive(i < orders.Count);
            if (i < orders.Count) slots[i].InitializeUI(orders[i].quantity, orders[i].plant.Icon);
        }

        nameTxt.text = recipeName;
        priceTxt.text = outputPlant != null ? "$" + outputPlant.SellPrice : "-";
        if (outputPlant == null) btn.interactable = false;
    }

    private void Update()
    {
        if (!isBaking) return;
        timer -= Time.deltaTime;
        if (timer <= 0) Bake();
    }

    public void CheckRecipe()
    {
        if (isBaking || outputPlant == null || orders.Count == 0) return;
        foreach (Order order in orders)
            if (order.plant == null || order.quantity <= 0 || fm.GetAmount(order.plant) < order.quantity) return;
        isBaking = true;
        foreach (Order order in orders) fm.TryRemovePlant(order.plant, order.quantity);
        btnTxt.text = "Baking";
        Color color = bkg.color;
        color.a = 0.15f;
        bkg.color = color;
        btn.interactable = false;
    }

    public void RemovePlants()
    {
        CheckRecipe();
    }

    public void Bake()
    {
        if (!isBaking || outputPlant == null) return;
        isBaking = false;
        timer = bakeTime;
        btnTxt.text = "Bake";
        Color color = bkg.color;
        color.a = 0f;
        bkg.color = color;
        btn.interactable = true;
        fm.AddPlant(outputPlant, 1);
    }
}
