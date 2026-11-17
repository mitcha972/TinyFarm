using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class OrderItem : MonoBehaviour
{
    private FarmManager fm;
    [FormerlySerializedAs("precentTxt")] [SerializeField] private Text percentTxt;
    [SerializeField] private Text priceTxt;
    [SerializeField] private Text btnTxt;
    [SerializeField] private Transform orderSlot;
    [SerializeField] private Button btn;
    [SerializeField] private Image bkg;
    [FormerlySerializedAs("precent")] [SerializeField] private int percentBonus;
    [SerializeField] private List<Order> orders = new List<Order>();
    private IngredientSlot[] slots;
    private int price;
    private bool completed;

    private void Awake()
    {
        CacheSlots();
    }

    private void CacheSlots()
    {
        if (slots == null)
            slots = orderSlot.GetComponentsInChildren<IngredientSlot>(true);
    }

    private void Start()
    {
        fm = FarmManager.Instance;
        fm.OnInventoryChanged += OnInventoryChanged;
        CheckOrderItem();
    }

    private void OnDestroy()
    {
        if (fm != null) fm.OnInventoryChanged -= OnInventoryChanged;
    }

    public void Setup(List<Order> lines, int percentBonus)
    {
        // Orders can be created under a hidden tab before Unity calls Awake.
        CacheSlots();
        orders = new List<Order>();
        foreach (Order line in lines)
        {
            if (line.plant == null || line.quantity <= 0) continue;
            Order existing = orders.Find(order => order.plant == line.plant);
            if (existing == null) orders.Add(new Order(line.plant, line.quantity));
            else existing.quantity += line.quantity;
        }
        this.percentBonus = percentBonus;
        completed = false;
        price = 0;
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].gameObject.SetActive(i < orders.Count);
            if (i < orders.Count) slots[i].InitializeUI(orders[i].quantity, orders[i].plant.Icon);
        }
        foreach (Order order in orders) price += order.plant.SellPrice * order.quantity;
        price += Mathf.RoundToInt(price * (percentBonus / 100f));
        priceTxt.text = "$" + price;
        percentTxt.text = percentBonus + "%";
        btnTxt.text = "Sell";
        Color color = bkg.color;
        color.a = 1f;
        bkg.color = color;
        CheckOrderItem();
    }

    private void OnInventoryChanged(Plant plant, int amount)
    {
        CheckOrderItem();
    }

    public void CheckOrderItem()
    {
        if (fm == null) return;
        bool canComplete = !completed && orders.Count > 0 && orders.Count <= slots.Length;
        for (int i = 0; i < orders.Count; i++)
        {
            bool enough = fm.GetAmount(orders[i].plant) >= orders[i].quantity;
            if (i < slots.Length) slots[i].quantityTxt.color = enough ? Color.green : Color.red;
            canComplete &= enough;
        }
        btn.interactable = canComplete;
    }

    public void CompleteOrder()
    {
        if (fm == null || completed || orders.Count == 0 || orders.Count > slots.Length) return;
        foreach (Order order in orders)
            if (fm.GetAmount(order.plant) < order.quantity) return;
        completed = true;
        foreach (Order order in orders) fm.TryRemovePlant(order.plant, order.quantity);
        fm.AddMoney(price);
        btnTxt.text = "Complete";
        Color color = bkg.color;
        color.a = 0.3f;
        bkg.color = color;
        btn.interactable = false;
    }

    // Keep old Inspector callbacks subject to the same completion checks.
    public void Sell()
    {
        CompleteOrder();
    }
}
