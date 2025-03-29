public class BugModel
{
    public int id;
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
