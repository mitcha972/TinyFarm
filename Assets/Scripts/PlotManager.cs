using UnityEngine;
using UnityEngine.EventSystems;

public class PlotManager : MonoBehaviour
{
    private const int PlotPrice = 100;
    private Plant selectedPlant;
    [SerializeField] private GameObject droppedPlant;
    [SerializeField] private SpriteRenderer plantSpriteRenderer;
    private SpriteRenderer plotSpriteRenderer;
    private FarmManager fm;
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private Sprite dryPlotSprite;
    [SerializeField] private Sprite notRakedSprite;
    [SerializeField] private Sprite healthyPlotSprite;
    [SerializeField] private Color availableColor = Color.green;
    [SerializeField] private Color unavailableColor = Color.red;
    private bool isPlanted;
    private bool isDry = true;
    [SerializeField] private bool isBought = true;
    private int plantStage;
    private float growTimer;

    private void Awake()
    {
        plotSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        fm = FarmManager.Instance;
        if (plantSpriteRenderer == null || plotSpriteRenderer == null)
        {
            Debug.LogError("Assign the plot and plant SpriteRenderers on PlotManager.", this);
            enabled = false;
            return;
        }
        plotSpriteRenderer.sprite = isBought ? dryPlotSprite : null;
    }

    private void Update()
    {
        if (!isPlanted || isDry || plantStage >= selectedPlant.PlantStages.Length - 1) return;
        growTimer -= Time.deltaTime;
        if (growTimer <= 0)
        {
            growTimer = selectedPlant.TimeBtwStages;
            plantStage++;
            UpdatePlant();
        }
    }

    private void MouseHover()
    {
        if (!isPlanted && isBought && fm.isPlanting && fm.SelectedSeed != null)
            PlantSeed(fm.SelectedSeed.plant);

        if (!fm.isSelecting) return;
        switch (fm.selectedTool)
        {
            case ToolType.Water:
                if (!isBought) return;
                isDry = false;
                plotSpriteRenderer.sprite = healthyPlotSprite;
                if (isPlanted) UpdatePlant();
                break;
            case ToolType.Harvest:
                if (isPlanted && plantStage == selectedPlant.PlantStages.Length - 1)
                    Harvest(selectedPlant);
                break;
            case ToolType.BuyPlot:
                if (!isBought && fm.TrySpend(PlotPrice))
                {
                    isBought = true;
                    plotSpriteRenderer.sprite = dryPlotSprite;
                }
                break;
        }
    }

    private void OnMouseOver()
    {
        if (!enabled || fm == null) return;
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            OnMouseExit();
            return;
        }
        if (fm.isPlanting && fm.SelectedSeed != null)
            plotSpriteRenderer.color = !isPlanted && isBought && fm.SelectedSeed.plant.BuyPrice <= fm.Money
                ? availableColor : unavailableColor;
        else if (fm.selectedTool == ToolType.BuyPlot)
        {
            plotSpriteRenderer.sprite = dryPlotSprite;
            plotSpriteRenderer.color = !isBought && fm.Money >= PlotPrice ? availableColor : unavailableColor;
        }
        else
            OnMouseExit();

        if (Input.GetMouseButton(0)) MouseHover();
    }

    private void OnMouseExit()
    {
        if (plotSpriteRenderer == null) return;
        plotSpriteRenderer.color = Color.white;
        if (!isBought) plotSpriteRenderer.sprite = null;
    }

    private void Harvest(Plant plant)
    {
        if (droppedPlant == null || canvasTransform == null || Camera.main == null) return;
        DroppedPlant drop = Instantiate(droppedPlant, canvasTransform).GetComponent<DroppedPlant>();
        drop.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        drop.Setup(plant);
        isPlanted = false;
        plantSpriteRenderer.gameObject.SetActive(false);
        isDry = true;
        plotSpriteRenderer.sprite = dryPlotSprite;
    }

    private void PlantSeed(Plant plant)
    {
        if (plant == null || plant.PlantStages == null || plant.PlantStages.Length == 0
            || !fm.TrySpend(plant.BuyPrice)) return;
        selectedPlant = plant;
        isPlanted = true;
        plantStage = 0;
        growTimer = plant.TimeBtwStages;
        UpdatePlant();
        plantSpriteRenderer.gameObject.SetActive(true);
    }

    private void UpdatePlant()
    {
        plantSpriteRenderer.sprite = isDry ? selectedPlant.DryPlanted : selectedPlant.PlantStages[plantStage];
    }
}
