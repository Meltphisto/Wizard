using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionElement : MonoBehaviour
{
    ElementManager manager;

    [SerializeField] ElementConfig element;

    private void Start()
    {
        manager = FindObjectOfType<ElementManager>();

        manager.GainElement(element);
    }
}
