using UnityEngine;

[CreateAssetMenu(fileName = "Create Item", menuName = "ScriptableObjects/Create Item", order = 1)]
public class Item : ScriptableObject
{
    [Header("Both")]
    public Sprite image;

    [Header("Gameplay")]
    public bool Stackable = true;
    public int maxStack = 64;
    public ItemType type;

    [Header("Type Information")]
    public SeedModel seedData;
    public BugModel bugData;
}

public enum ItemType
{
    Seed,
    Bug
}
