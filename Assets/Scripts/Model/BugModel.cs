using System;
using UnityEngine;

[Serializable]
public class BugModel
{
    [Header("Bug Information")]
    public string name;
    public int price;
    public int amount;
    public BugType bugType;
    public Rarity rarity;
}

public enum BugType
{
    Ant
}

public enum Rarity
{
    Basic,
    Rare
}
