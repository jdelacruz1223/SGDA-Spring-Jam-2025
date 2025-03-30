using UnityEngine;

[System.Serializable]
public class SeedModel
{

    [Header("Seed Information")]
    public string name;
    public int price;
    public int amount;
    public SeedType seedType;
}

public enum SeedType
{
    Fern
}
