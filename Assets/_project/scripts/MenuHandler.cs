using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GameCreator;

public class MenuHandler : MonoBehaviour
{
    public GameObject focusCanvas, inventoryCanvas, selectedVerb, selectedItem;

    void Start()
    {
        
    }

    private void SelectedItemHandler() {

        if (focusCanvas.activeInHierarchy == true)
        {
            if (selectedVerb.activeInHierarchy == false)
            {
                selectedVerb.SetActive(true);
                selectedItem.SetActive(true);
            }
        }
        else {
            selectedVerb.SetActive(false);
            selectedItem.SetActive(false);
        }

    }
    void Update()
    {
        SelectedItemHandler();
    }
}
