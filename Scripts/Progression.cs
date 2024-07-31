using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "Progression", menuName = "Progression")]
public class Progression : ScriptableObject
{
    public int highestLevelCompleted = 1;
    public List<int> tulips;
    public Vector3 spawnPoint;
}
