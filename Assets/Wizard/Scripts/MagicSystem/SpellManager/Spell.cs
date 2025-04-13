using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "MagicData/SpellConfig")]
public class Spell : ScriptableObject
{
    public int spellId;
    public string spellName;

    [Tooltip("需要的元素种类与数量")]
    public List<Element> requiredElementCombo = new List<Element>();
    [Tooltip("法术类型：投射物类/AOE类")]
    public SpellType spellType;
    [Tooltip("施法前摇")]
    public float prepareTime;
    [Tooltip("动画时间")]
    public float animationTime;
    [Tooltip("弹道速度-投射物类")]
    public float speed;
    [Tooltip("伤害")]
    public float damage;
    [Tooltip("技能实体")]
    public GameObject prefab;
    [Tooltip("前摇特效")]
    public GameObject prepareVFX;

}

public enum SpellType { procjectile, AOE, Fusion }



