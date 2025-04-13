using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MagicData/ElementConfig")]
public class ElementConfig : ScriptableObject
{
    [SerializeField] public int elementId;
    [SerializeField] public string elementName;
    [SerializeField] public Color elementColor;
    [SerializeField] public int elementLoad;

}
