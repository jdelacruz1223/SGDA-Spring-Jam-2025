using System;
using UnityEngine;

[Serializable]
public class BugModel
{
    [Header("Bug Information")]
    public string id;
    public string name;
    public bool stackable;
    public int price;
    public string description;
    public float spawnTime;
}

