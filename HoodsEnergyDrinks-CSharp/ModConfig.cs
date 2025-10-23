using System.Text.Json;
using System.Text.Json.Serialization;

namespace HoodsEnergyDrinks_CSharp;

public class ModConfig
{
    public bool enable_alternate_buffs { get; set; } = false;
    public int alternate_trader_price { get; set; } = 0;
    public int alternate_flea_price { get; set; } = 0;
    public int alternate_handbook_price { get; set; } = 0;
    public required Dictionary<string, DrinkConfig> drinks { get; set; }
}

public class DrinkConfig
{
    public bool enable { get; set; }
    public bool effect_toggle { get; set; }
    public bool sold_by_trader { get; set; }
    public bool flea_banned { get; set; }
    public int trader_price { get; set; }
    public int flea_price { get; set; }
    public int handbook_price { get; set; }
    public int stock { get; set; }
    public int loyalty_level { get; set; }
    public Dictionary<string, double>? loot_multipliers { get; set; }
}
