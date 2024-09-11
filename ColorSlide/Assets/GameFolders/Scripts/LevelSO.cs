using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu (fileName = "LevelSO", menuName = "ScriptableObjects/LevelSO", order = 51)]
public class LevelSO : ScriptableObject
{
    [SerializeField] private Material[] materials ;
    [SerializeField] private int sliderParts;
    
    public Material[] Materials => materials;
    public int SliderParts => sliderParts;
    
}
