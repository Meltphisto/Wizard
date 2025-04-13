using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSwitcher : MonoBehaviour
{
    [SerializeField] CinemachineBrain brain;

    public GameObject menuShot,mainShot,wizardFocusShot,enemyFocusShot,gateFocusShot;
    GameObject currentShot;
    private void Start()
    {
        currentShot = menuShot;
    }
    public void SwitchShot(int index, float blendTime)
    {
       ChangeBrainBlend(blendTime);
       currentShot.SetActive(false);
       currentShot = transform.GetChild(index).gameObject;
       currentShot.SetActive(true);
    }

    public void ChangeBrainBlend(float time)
    {
        brain.m_DefaultBlend.m_Time = time;
    }
}
