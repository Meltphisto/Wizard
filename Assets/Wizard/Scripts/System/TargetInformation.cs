using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetInformation : MonoBehaviour
{

    [Tooltip("UI to display the enemy that player is selecting.")]
    [SerializeField] GameObject targetInforSprite;
    [SerializeField] Wizard wizard;
    [SerializeField] Image icon;
    [SerializeField] TMP_Text hp;
    Enemy target;

    void GetTargetInformation()
    {
        icon.sprite = target.AccessData.icon;
        hp.SetText(target.CurrentHealth + " / " + target.AccessData.maximumHealth);
    }

    void GetTarget(GameObject target)
    {
        this.target = target.GetComponent<Enemy>();
    }

    private void Update()
    {
        if (wizard.Target!=null)
        {
            GetTarget(wizard.Target);
            GetTargetInformation();
        }
        else { target = null; }
        targetInforSprite.SetActive(target != null);
    }
}
