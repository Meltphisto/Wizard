using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Wizard : MonoBehaviour
{
    [SerializeField] GameObject playerTurnUI;
    [Tooltip("Target for projectile spells.")]
    [SerializeField] GameObject target;
    [Tooltip("LaunchPoint for projectile spells.")]
    [SerializeField] Transform launchPoint;

    [Tooltip("Area indicator for AOE spells.")]
    [SerializeField] GameObject marker;

    [Tooltip("Wizard state, used for wizard animation and script logic.")]
    [SerializeField] WizardState state;

    [SerializeField] Animator animator;
    [SerializeField] Spell spellToCast;

    ElementManager elementManager;

    ShotSwitcher shotSwitcher;
    public GameObject Target => target;
    enum WizardState
    {
        idle,
        isChantingProjectile,
        isChantingAOE,
        isCasting
    }
    private void Start()
    {
        state = WizardState.idle;
        target = null;
        animator = GetComponentInChildren<Animator>();
        shotSwitcher = FindObjectOfType<ShotSwitcher>();
        elementManager = FindObjectOfType<ElementManager>();
    }

    public void GetSpell(Spell spell)
    {
        spellToCast = spell;
    }

    #region Functions in idle state
    void GetEnemyTarget()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectTarget();
        }
        if (Input.GetMouseButtonDown(1))
        {
            DisSelectTarget();
        }
    }

    public void Cast(Spell spell)
    {
        GetSpell(spell);
        switch (spell.spellType)
        {
            case SpellType.procjectile:
                state = WizardState.isChantingProjectile;
                break;
            case SpellType.AOE:
                marker = Instantiate(spellToCast.prefab);
                state = WizardState.isChantingAOE;
                break;
            case SpellType.Fusion:
                StartCoroutine(FusionElement(spellToCast));
                state = WizardState.isCasting;
                break;
        }
    }
    #endregion

    #region Functions when is chanting a projectile spell
    private void SelectTargetForProjectile()
    {
        if(state == WizardState.isChantingProjectile)
        {
            SelectTarget();
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if(target != null)
            {
                state = WizardState.isCasting;
                StartCoroutine(CastProjectile(spellToCast));
            }
            else Debug.Log("Please select a target.");
        }
    }

    IEnumerator CastProjectile(Spell spell)
    {
        //播放动画
        animator.SetTrigger("Cast");
        animator.SetInteger("CastNumber", 0);
        //镜头特写
        shotSwitcher.SwitchShot(2,0);
        StartCoroutine(PlayMuzzleEffect());
        playerTurnUI.SetActive(false);
        yield return new WaitForSeconds(spell.prepareTime);
        shotSwitcher.SwitchShot(1, 0);
        GameObject theSpell = Instantiate(spell.prefab, launchPoint.position, Quaternion.identity);
        theSpell.GetComponent<Projectile>().SetTarget(target);
        yield return new WaitForSeconds(spell.animationTime);
        PlaySpellAnimation();
    }

    #endregion

    #region Functions when is chanting an AOE spell

    private void SelectAreaForAOE()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 7))
        {
            marker.transform.position = new Vector3(hit.point.x, hit.point.y + 0.1f, hit.point.z);
        }

        if (Input.GetMouseButtonDown(0))
        {
            state = WizardState.isCasting;

            StartCoroutine(CastAOE(spellToCast));
        }
    }

    IEnumerator CastAOE(Spell spell)
    {
        //播放动画
        animator.SetTrigger("Cast");
        animator.SetInteger("CastNumber",1);
        //切换镜头
        shotSwitcher.SwitchShot(3, 0);
        //播放特效
        StartCoroutine(PlayMuzzleEffect());
        playerTurnUI.SetActive(false);
        yield return new WaitForSeconds(spell.prepareTime);
        shotSwitcher.SwitchShot(1, 0);
        StartCoroutine(marker.GetComponent<AOESpell>().PlayAOE(spell.prepareTime));
        yield return new WaitForSeconds(spell.animationTime);
        PlaySpellAnimation();
    }

    #endregion
    void PlaySpellAnimation()
    {
        state = WizardState.idle;
        playerTurnUI.SetActive(true);
    }

    #region

    #endregion

    private void Update()
    {
        Test();
        switch (state)
        {
            case WizardState.idle:
                GetEnemyTarget();
                break;
            case WizardState.isChantingProjectile:
                //ViewProjectileMarker();
                SelectTargetForProjectile();
                break;
            case WizardState.isChantingAOE:
                SelectAreaForAOE();
                break;
            case WizardState.isCasting:
                break;
        }
        
    }

    IEnumerator PlayMuzzleEffect()
    {
        GameObject VFX = Instantiate(spellToCast.prepareVFX,launchPoint);
        yield return new WaitForSeconds(10F);
        Destroy(VFX);
    }

    IEnumerator FusionElement(Spell spell)
    {
        animator.SetTrigger("Cast");
        animator.SetInteger("CastNumber", 1);
        GameObject fusion = Instantiate(spell.prefab);
        playerTurnUI.SetActive(false);
        yield return new WaitForSeconds(2f);
        PlaySpellAnimation();
        Destroy(fusion);

    }

    void Test()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Cast(spellToCast);
        }
    }

    void SelectTarget()
    {
        if (target!=null)
        {
            DisSelectTarget();
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
        {
            target = hit.collider.gameObject;
            target.GetComponent<Enemy>().Selector.SetActive(true);
        }
        else { target = null; }
    }

    void DisSelectTarget()
    {
        if (target!=null)
        {
          target.GetComponent<Enemy>().Selector.SetActive(false);
        }
        target = null;
    }


}
