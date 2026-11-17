using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    [SerializeField] private GameObject orderItem;
    [SerializeField] private PlantStoreManager pm;
    [SerializeField] private Transform contentArea;
    private readonly List<OrderItem> orderItems = new List<OrderItem>();

    private void Start()
    {
        CreateOrders();
    }

    public void CreateOrders()
    {
        var available = pm.plantObjects.FindAll(plant => plant.UnlockLevel <= FarmManager.Instance.Level);
        if (available.Count == 0) return;
        for (int i = 0; i < 5; i++)
        {
            var candidates = new List<Plant>(available);
            int lineCount = Mathf.Min(Random.Range(1, 3), candidates.Count);
            var lines = new List<Order>();
            for (int j = 0; j < lineCount; j++)
            {
                int index = Random.Range(0, candidates.Count);
                lines.Add(new Order(candidates[index], Random.Range(1, 5)));
                candidates.RemoveAt(index);
            }
            OrderItem item = Instantiate(orderItem, contentArea).GetComponent<OrderItem>();
            item.Setup(lines, Random.Range(-10, 16));
            orderItems.Add(item);
        }
    }

    public void CheckOrders()
    {
        foreach (OrderItem item in orderItems) item.CheckOrderItem();
    }

    public void RefreshOrders()
    {
        foreach (OrderItem item in orderItems) Destroy(item.gameObject);
        orderItems.Clear();
        CreateOrders();
    }
}
