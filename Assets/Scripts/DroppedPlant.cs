using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DroppedPlant : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private Image icon;
    private Plant plant;

    public void Setup(Plant plant)
    {
        this.plant = plant;
        icon.sprite = plant.Icon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        FarmManager.Instance.AddPlant(plant, 1);
        Destroy(gameObject);
    }
}
