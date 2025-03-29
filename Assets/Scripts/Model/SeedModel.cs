[System.Serializable]
public class SeedModel
{
    public int id;
    public string name;
    public int price;
    public int amount;
    public SeedType seedType;
}

public enum SeedType
{
    Fern
}
