using System.Collections.Generic;
using UnityEngine;

public class StallManager : MonoBehaviour
{
    [SerializeField] private GameObject stallItem;
    [SerializeField] private GameObject clickImage;
    [SerializeField] private GameObject mm;
    public StallItem selectedStall;
    [SerializeField] private int speed;
    private float timer = 2f;
    private readonly List<StallItem> stallItems = new List<StallItem>();

    private void Start()
    {
        foreach (StallItem stall in GetComponentsInChildren<StallItem>(true))
        {
            stall.Setup(this, mm, clickImage);
            if (!stallItems.Contains(stall)) stallItems.Add(stall);
        }
    }

    private void Update()
    {
        timer -= speed * Time.deltaTime;
        if (timer <= 0) CheckStall();
    }

    public void CheckStall()
    {
        foreach (StallItem stall in stallItems)
            if (stall != null) stall.SellOne();
        timer = 2f;
    }

    public void AddStall()
    {
        StallItem stall = Instantiate(stallItem, transform).GetComponent<StallItem>();
        stall.Setup(this, mm, clickImage);
        stallItems.Add(stall);
    }

    public void CancelStall()
    {
        if (selectedStall != null) selectedStall.gameObject.SetActive(true);
        clickImage.SetActive(false);
        mm.SetActive(false);
        selectedStall = null;
    }
}
