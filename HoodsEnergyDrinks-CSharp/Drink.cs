namespace HoodsEnergyDrinks_CSharp;

public class Drink
{
    public required Dictionary<string, DrinkProps> Props { get; set; }
}

public class DrinkProps
{
    public string _id { get; set; }
    public string name { get; set; }
    public string shortName { get; set; }
    public string desc { get; set; }
}

