using System.Text.Json;
using System.Text.Json.Serialization;

namespace HoodsEnergyDrinks_CSharp;

public class DrinkInfo
{
    public required Dictionary<string, DrinkProps> drinks { get; set; }
}

public class DrinkProps
{
    public string _id { get; set; }
    public string name { get; set; }
    public string shortName { get; set; }
    public string desc { get; set; }
}

