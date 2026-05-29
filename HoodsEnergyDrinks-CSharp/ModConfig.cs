using System.Text.Json;
using System.Text.Json.Serialization;

namespace HoodsEnergyDrinks_CSharp;

public class ModConfig
{
    public required bool instant_energy_and_hydration { get; set; }
    public required Dictionary<string, DrinkConfig> drinks { get; set; }
}

public class DrinkConfig
{
    public bool enable { get; set; }
    public bool buff_effect_enable { get; set; }
    public bool sold_by_trader { get; set; }
    public bool flea_banned { get; set; }
    public int trader_price { get; set; }
    public int flea_price { get; set; }
    public int handbook_price { get; set; }
    public int trader_stock { get; set; }
    public int loyalty_level { get; set; }
    public Dictionary<string, float>? loot_multipliers { get; set; }
    public float loose_loot_multiplier { get; set; }
}
