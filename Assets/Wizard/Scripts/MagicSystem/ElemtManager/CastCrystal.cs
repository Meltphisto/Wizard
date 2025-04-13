using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastCrystal : MonoBehaviour
{
    public ElementConfig element;

    private void Update()
    {
        if (element != null)
        {
            this.GetComponent<Image>().color = element.elementColor;
        }
        else this.GetComponent<Image>().color = Color.white;
    }
    
}
