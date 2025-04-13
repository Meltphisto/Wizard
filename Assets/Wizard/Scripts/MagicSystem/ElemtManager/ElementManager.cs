using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class Element : IEquatable<Element>
{
    [SerializeField] public ElementConfig elementConfig;// Information of the element.
    [SerializeField] public int elementReserve = 0;

    public void GainElement()
    {
        if(elementReserve < elementConfig.elementLoad)
        {
           elementReserve++;
        }
    }

    public void LoseElement()
    {
        if (elementReserve > 0)
        {
            elementReserve--;
        }
    }
    
    public bool Equals(Element other)
    {
        if (other == null) return false;
        else return (other.elementConfig == elementConfig) && other.elementReserve.Equals(elementReserve);

    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        return Equals(obj as Element);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(elementConfig, elementReserve);
    }


}

public class ElementManager : MonoBehaviour
{
    public List<Element> elementResource = new List<Element>();
    

    [SerializeField] ElementDisk disk;
    [SerializeField] List<ElementConfig> basicElements = new List<ElementConfig>();

    public void GainElementAtPlayerTurn(int count)
    {
        while (count > 0)
        {
            foreach(ElementConfig e in basicElements)
            {
                GainElement(e);
            }
            count--;
        }
    }


    public int FindElement(ElementConfig e)
    {
        //Find the position of the element in list.
        for(int i = 0; i < elementResource.Count; i++)
        {
            if (elementResource[i].elementConfig.Equals(e))
            {
                return i;
            }
        }
        return -1;
    }

    public Element GetElement(ElementConfig e)
    {
        return elementResource[FindElement(e)];
    }
    public void GainElement(ElementConfig e)
    {
        elementResource[FindElement(e)].GainElement();
    }

    public void LoseElement(ElementConfig e)
    {
        elementResource[FindElement(e)].LoseElement();
    }

    public void SetElementToDisk(ElementConfig e)
    {
        if (!disk.IsDiskFull())
        {
            if (GetElement(e).elementReserve > 0)
            {
                LoseElement(e);
                disk.GainElementToDisk(e);
            }
        }
    }

    


}
