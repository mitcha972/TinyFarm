using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StallManager : MonoBehaviour
{
    public GameObject stallItem;
    public GameObject clickImage;
    public GameObject mm;
    public StallItem selectedStall;

    [SerializeField] private FarmManager fm;
    private float timer;
    public int speed;

    private List<StallItem> stallItems = new List<StallItem>();
    
    private void Start()
    {
        //AddStall();
        timer = 2f;
    }

    private void Update()
    {
        timer -= speed*Time.deltaTime;

        if (timer < 0){
            CheckStall();
        }       
    }
    
    public void CheckStall()
    {
        foreach(StallItem si in stallItems)
        {
            //If the stall is active
            if(si.isActive == true)
            {
                //buy goods
                si.SellPlant();
                
                if(si.quantity<= 0)
                    si.RemoveStall();
            }
        }

        timer = 2;
    }

    public void AddStall()
    {
        //Check Money
        //

        //Add Stall
        StallItem newStall = Instantiate(stallItem, transform).GetComponent<StallItem>();
        newStall.ci = clickImage;
        newStall.mm = mm;
        stallItems.Add(newStall);

        //Update Money
        //
    }

    public void CancleStall()
    {
        selectedStall.gameObject.SetActive(true);
        clickImage.SetActive(false);
        selectedStall = null;
    }
}