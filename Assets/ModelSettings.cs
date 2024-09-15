using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ModelSettings", menuName = "Custom/ModelSettings", order = 1)]
public class ModelSettings : ScriptableObject
{
    public GameObject model;
    public string modelName;
    public bool wasFinished;
    public string Description;
    public string[] hints;
}
