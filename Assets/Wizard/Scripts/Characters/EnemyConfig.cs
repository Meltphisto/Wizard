using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Character/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    // Basic infor
    public int id;
    public string enemyNname;
    // Move related
    public float step;
    public float speed;
    // Health related
    public float maximumHealth;
    public float castDamage;
    // GameObject model
    public GameObject prefab;

    public Sprite icon;

}
