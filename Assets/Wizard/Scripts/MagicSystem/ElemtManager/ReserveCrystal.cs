using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReserveCrystal : MonoBehaviour
{
    [SerializeField] private ElementConfig element; // The element object this crystal points to.
    [SerializeField] ElementManager elementManager;


    private void Awake()
    {
        elementManager = GameObject.Find("ElementManager").GetComponent<ElementManager>();
        
        this.gameObject.name = element.elementName;
    }

    private void Update()
    {
        UpdateElementCount();
    }

    private void UpdateElementCount()
    {
        string elementCount = elementManager.GetElement(element).elementReserve.ToString(); // Obtain element count in reserve.
        this.GetComponentInChildren<TMP_Text>().SetText(" x " + elementCount);
    }


    

}
