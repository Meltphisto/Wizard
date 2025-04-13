using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Rendering.FilterWindow;

[System.Serializable]
public class ElementDisk : MonoBehaviour
{
    
    [SerializeField] List<Element> elementsOnDisk = new List<Element>();
    [SerializeField] List<CastCrystal> crystals = new List<CastCrystal>();
    
    ElementManager elementManager;
    SpellBook book;
    Wizard player;
    

    private void Awake()
    {
        elementManager = FindObjectOfType<ElementManager>();
        book = FindObjectOfType<SpellBook>();
        player = FindObjectOfType<Wizard>();
    }

    public bool IsDiskFull()
    {
        for (int i = 0; i < crystals.Count; i++)
        {
            if (crystals[i].element == null)
            {
                return false;
            }
        }
        return true;
    }


    public void GainElementToDisk(ElementConfig e)
    {
        elementsOnDisk[FindElement(e)].elementReserve++;
        for(int i = 0; i < crystals.Count; i++)
        {
            if (crystals[i].element == null)
            {
                crystals[i].element = e;
                break;
            }
        }
    }

    public void CancleElementOnDisk(CastCrystal crystal)
    {
        if (crystal.element != null)
        {
            elementsOnDisk[FindElement(crystal.element)].elementReserve--;
            elementManager.GainElement(crystal.element);
            crystal.element = null;
        }
    }

    public void ResetDisk()
    {
        for (int i = 0; i < crystals.Count; i++)
        {
            if (crystals[i].element != null)
            {
                CancleElementOnDisk(crystals[i]);
            }
        }
    }

    public void ConsumeElement()
    {
        //获取是否为可以释放的法术
        Spell spellToCast = book.GetValidSpell(elementsOnDisk);
        if (spellToCast != null)
        {
                //传递法术信息
                player.Cast(spellToCast);
                //清空元素盘
                foreach (CastCrystal crystal in crystals)
                {
                    if (crystal.element != null)
                    {
                        elementsOnDisk[FindElement(crystal.element)].elementReserve--;
                        crystal.element = null;
                    }
                }
                Debug.Log(spellToCast.spellName);
        }
        else Debug.Log("Not a valid spell");
    }


    public int FindElement(ElementConfig e)
    {
        //Find the position of the element in list.
        for (int i = 0; i < elementsOnDisk.Count; i++)
        {
            if (elementsOnDisk[i].elementConfig.Equals(e))
            {
                return i;
            }
        }
        return -1;
    }

    public Element GetElement(ElementConfig e)
    {
        return elementsOnDisk[FindElement(e)];
    }

}
