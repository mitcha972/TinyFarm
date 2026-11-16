using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotManager : MonoBehaviour
{
    private Plant selectedPlant;
    private PlantFoodObject selectedPlantFood;
    public GameObject droppedPlant;

    private FarmManager fm;

    private SpriteRenderer plantSpriteRenderer;
    private SpriteRenderer plotSpriteRenderer;

    [SerializeField] private Transform canvasTransform;

    [Header("Sprites")]
    public Sprite dryPlotSprite; //DryPlot 
    public Sprite notRakedSprite; //Not Raked Plot
    public Sprite healthyPlotSprite; 

    //public Sprite unkeptPlotSprite;

    public Color availableColor = Color.green;
    public Color unavailableColor = Color.red; 

    private BoxCollider2D plantCollider;

    [Header("Game Play Variables")]
    private bool isPlanted = false;
    private bool isFertalized = false;
    private bool isDry = true;
    public bool isBought = true;

    private int plantStage = 0; 

    private float growTimer;
    private float growSpeed = 1f;   

    void Start()
    {
        SetUpPlot();
    }

    void SetUpPlot()
    {
        plantSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        plantCollider = transform.GetChild(0).GetComponent<BoxCollider2D>();
        fm = transform.parent.GetComponent<FarmManager>();
        plotSpriteRenderer = GetComponent<SpriteRenderer>();

        if (isBought)
        {
            plotSpriteRenderer.sprite = dryPlotSprite;
        }
        else
        {
            plotSpriteRenderer.sprite = null;
        }        
    }

    private void Update()
    {
        if (isPlanted && !isDry)
        {
            growTimer -= growSpeed*Time.deltaTime;

            if (growTimer < 0 && plantStage<selectedPlant.PlantStages.Length-1)
            {
                growTimer = selectedPlant.TimeBtwStages;
                plantStage++;
                UpdatePlant();
            }
        }
    }

    private void MouseHover()
    {
        if (!isPlanted)
        {
            if(fm.isPlanting && fm.selectPlant.plant.BuyPrice <= fm.money && isBought)
            {
                Plant(fm.selectPlant.plant);

            }
        }

/*
        if (!isFertalized)
        {
            if(fm.isBoosting && fm.selectPlantFood.plantFood.BuyPrice <= fm.money && isBought)
            {
                //isDry = false;
                //plot.sprite = healthyPlotSprite;
                if (isPlanted) UpdatePlant();
                isFertalized = true;
                //particle.SetActive(true);
                fm.Transaction(-fm.selectPlantFood.plantFood.BuyPrice);
            }
        }
*/

        if (fm.isSelecting)
        {
            switch (fm.selectedTool)
            {
                case 1:
                    if (isBought) {
                        isDry = false;
                        plotSpriteRenderer.sprite = healthyPlotSprite;
                        if (isPlanted) UpdatePlant();
                    }
                    break;
                case 2:                    
                    if (isPlanted)
                    {
                        if (plantStage == selectedPlant.PlantStages.Length - 1 && !fm.isPlanting)
                        {
                            Harvest(selectedPlant);
                            //fm.AddPlant(new Plant(selectedPlant.PlantName,selectedPlant.SellPrice,selectedPlant.Icon),1);   
                        }
                    }
                    break;
                case 3:
                    if (fm.money >= 100 && !isBought)
                    {
                        fm.Transaction(-100);
                        isBought = true;
                        plotSpriteRenderer.sprite = dryPlotSprite;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void OnMouseOver()
    {
        
        if (fm.isPlanting)
        {
            if(isPlanted || fm.selectPlant.plant.BuyPrice > fm.money || !isBought)
            {
                //can't buy
                plotSpriteRenderer.color = unavailableColor;
            }
            else
            {
                //can buy
                plotSpriteRenderer.color = availableColor;
            }
        }
        if (fm.isSelecting)
        {
            switch (fm.selectedTool)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    if (!isBought && fm.money>=100)
                    {
                        plotSpriteRenderer.sprite = dryPlotSprite;
                        plotSpriteRenderer.color = availableColor;
                    }
                    else
                    {
                        plotSpriteRenderer.sprite = dryPlotSprite;
                        plotSpriteRenderer.color = unavailableColor;
                    }
                    break;
                default:
                    plotSpriteRenderer.color = unavailableColor;
                    break;
            }
        }

        if((Input.GetKey(KeyCode.Mouse0)))
        {
            OnMouseOver();
        }
    }

    private void OnMouseExit()
    {
        plotSpriteRenderer.color = Color.white;
        
        if (!isBought)
        {
            plotSpriteRenderer.sprite = null;
        }
    }

    void Harvest(Plant newplant)
    {
        isPlanted = false;
        plantSpriteRenderer.gameObject.SetActive(false);
        //fm.Transaction(selectedPlant.sellPrice);
        isDry = true;
        plotSpriteRenderer.sprite = dryPlotSprite;
        growSpeed = 1f; 

        DroppedPlant droppedplant = Instantiate(droppedPlant, canvasTransform).GetComponent<DroppedPlant>();

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        droppedplant.transform.position = screenPos;

        droppedplant.Setup(newplant);
    }

    void Plant(Plant newPlant)
    {
        selectedPlant = newPlant;
        isPlanted = true;

        fm.Transaction(-selectedPlant.BuyPrice);

        plantStage = 0;
        UpdatePlant();
        growTimer = selectedPlant.TimeBtwStages;
        plantSpriteRenderer.gameObject.SetActive(true);
    }

    void UpdatePlant()
    {
        if (isDry)
        {
            plantSpriteRenderer.sprite = selectedPlant.DryPlanted;
        }
        else
        {
            plantSpriteRenderer.sprite = selectedPlant.PlantStages[plantStage];
        }
    }
}