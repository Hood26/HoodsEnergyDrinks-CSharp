namespace HoodsEnergyDrinks_CSharp;

class Loot
{
    public Dictionary<string, StaticLoot> StaticLoot { get; set; } = new();
    public Dictionary<string, float> LooseLoot { get; set; } = new();
}

public class StaticLoot
{
    public Dictionary<string, float> Weights { get; set; } = new();
}