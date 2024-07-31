using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "System", menuName = "System")]
public class SystemData : ScriptableObject
{
    public Vector3Int wallPosition;
    public Vector3Int floorPosition;
    public bool vertical;
}
