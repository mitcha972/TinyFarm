using System.Collections.Generic;
using UnityEngine;

public class StallSelectionOverlay : MonoBehaviour
{
    public GameObject tab;

    public void CloseImage()
    {
        if (tab != null) tab.SetActive(true);
        tab = null;
        gameObject.SetActive(false);
    }
}
