using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "MagicData/SpellConfig")]
public class Spell : ScriptableObject
{
    public int spellId;
    public string spellName;

    [Tooltip("��Ҫ��Ԫ������������")]
    public List<Element> requiredElementCombo = new List<Element>();
    [Tooltip("�������ͣ�Ͷ������/AOE��")]
    public SpellType spellType;
    [Tooltip("ʩ��ǰҡ")]
    public float prepareTime;
    [Tooltip("����ʱ��")]
    public float animationTime;
    [Tooltip("�����ٶ�-Ͷ������")]
    public float speed;
    [Tooltip("�˺�")]
    public float damage;
    [Tooltip("����ʵ��")]
    public GameObject prefab;
    [Tooltip("ǰҡ��Ч")]
    public GameObject prepareVFX;

}

public enum SpellType { procjectile, AOE, Fusion }



