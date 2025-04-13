using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellBook : MonoBehaviour
{
    public List<Spell> spellList = new List<Spell>();

    [SerializeField] GameObject spellBookOpen;
    [SerializeField] GameObject elementCanvas;
     bool isOpening = false;

    public Spell GetValidSpell(List<Element> elementOnDiskList)
    {
        foreach(Spell spell in spellList)
        {
            if(spell.requiredElementCombo.All(item => elementOnDiskList.Contains(item)))
            {
                return spell;
            }
        }
        return null;
    }

    public void OpenOrCloseSpellBook()
    {
        isOpening = !isOpening;
        spellBookOpen.SetActive(isOpening);
        elementCanvas.SetActive(!isOpening);
    }

    

    
}
