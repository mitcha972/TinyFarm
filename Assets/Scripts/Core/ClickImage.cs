using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickImage : MonoBehaviour
{
    public GameObject tab;

    public void CloseImage()
    {
        tab.SetActive(true);
        tab = null;
    }
}
