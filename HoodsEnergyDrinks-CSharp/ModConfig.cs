using System.Text.Json;
using System.Text.Json.Serialization;

namespace HoodsEnergyDrinks_CSharp;

public class ModConfig
{
    public bool enable_alternate_buffs { get; set; } = false;

    public int alternate_trader_price { get; set; } = 0;

    public int alternate_flea_price { get; set; } = 0;

    public int alternate_handbook_price { get; set; } = 0;

    // monster_blue
    public bool monster_blue_enable { get; set; }
    public bool monster_blue_effect_toggle { get; set; }
    public bool monster_blue_sold_by_trader { get; set; }
    public bool monster_blue_flea_banned { get; set; }
    public int monster_blue_trader_price { get; set; }
    public int monster_blue_flea_price { get; set; }
    public int monster_blue_handbook_price { get; set; }
    public int monster_blue_stock { get; set; }
    public int monster_blue_loyalty_level { get; set; }
    public double monster_blue_loot_duffle_bag_multiplier { get; set; }
    public double monster_blue_loot_dead_scav_multiplier { get; set; }
    public double monster_blue_loot_jacket_multiplier { get; set; }
    public double monster_blue_loot_ration_supply_crate_multiplier { get; set; }
    public double monster_blue_loot_ground_cache_multiplier { get; set; }
    public double monster_blue_loose_loot_multiplier { get; set; }

    // monster_green
    public bool monster_green_enable { get; set; }
    public bool monster_green_effect_toggle { get; set; }
    public bool monster_green_sold_by_trader { get; set; }
    public bool monster_green_flea_banned { get; set; }
    public int monster_green_trader_price { get; set; }
    public int monster_green_flea_price { get; set; }
    public int monster_green_handbook_price { get; set; }
    public int monster_green_stock { get; set; }
    public int monster_green_loyalty_level { get; set; }
    public double monster_green_loot_duffle_bag_multiplier { get; set; }
    public double monster_green_loot_dead_scav_multiplier { get; set; }
    public double monster_green_loot_jacket_multiplier { get; set; }
    public double monster_green_loot_ration_supply_crate_multiplier { get; set; }
    public double monster_green_loot_ground_cache_multiplier { get; set; }
    public double monster_green_loose_loot_multiplier { get; set; }

    // monster_white
    public bool monster_white_enable { get; set; }
    public bool monster_white_effect_toggle { get; set; }
    public bool monster_white_sold_by_trader { get; set; }
    public bool monster_white_flea_banned { get; set; }
    public int monster_white_trader_price { get; set; }
    public int monster_white_flea_price { get; set; }
    public int monster_white_handbook_price { get; set; }
    public int monster_white_stock { get; set; }
    public int monster_white_loyalty_level { get; set; }
    public double monster_white_loot_duffle_bag_multiplier { get; set; }
    public double monster_white_loot_dead_scav_multiplier { get; set; }
    public double monster_white_loot_jacket_multiplier { get; set; }
    public double monster_white_loot_ration_supply_crate_multiplier { get; set; }
    public double monster_white_loot_ground_cache_multiplier { get; set; }
    public double monster_white_loose_loot_multiplier { get; set; }

    // monster_strawberry
    public bool monster_strawberry_enable { get; set; }
    public bool monster_strawberry_effect_toggle { get; set; }
    public bool monster_strawberry_sold_by_trader { get; set; }
    public bool monster_strawberry_flea_banned { get; set; }
    public int monster_strawberry_trader_price { get; set; }
    public int monster_strawberry_flea_price { get; set; }
    public int monster_strawberry_handbook_price { get; set; }
    public int monster_strawberry_stock { get; set; }
    public int monster_strawberry_loyalty_level { get; set; }
    public double monster_strawberry_loot_duffle_bag_multiplier { get; set; }
    public double monster_strawberry_loot_dead_scav_multiplier { get; set; }
    public double monster_strawberry_loot_jacket_multiplier { get; set; }
    public double monster_strawberry_loot_ration_supply_crate_multiplier { get; set; }
    public double monster_strawberry_loot_ground_cache_multiplier { get; set; }
    public double monster_strawberry_loose_loot_multiplier { get; set; }

    // ghost
    public bool ghost_enable { get; set; }
    public bool ghost_effect_toggle { get; set; }
    public bool ghost_sold_by_trader { get; set; }
    public bool ghost_flea_banned { get; set; }
    public int ghost_trader_price { get; set; }
    public int ghost_flea_price { get; set; }
    public int ghost_handbook_price { get; set; }
    public int ghost_stock { get; set; }
    public int ghost_loyalty_level { get; set; }
    public double ghost_loot_duffle_bag_multiplier { get; set; }
    public double ghost_loot_dead_scav_multiplier { get; set; }
    public double ghost_loot_jacket_multiplier { get; set; }
    public double ghost_loot_ration_supply_crate_multiplier { get; set; }
    public double ghost_loot_ground_cache_multiplier { get; set; }
    public double ghost_loose_loot_multiplier { get; set; }

    // nos (note: json uses "nos_blue_flea_banned" key)
    public bool nos_enable { get; set; }
    public bool nos_effect_toggle { get; set; }
    public bool nos_sold_by_trader { get; set; }
    public bool nos_blue_flea_banned { get; set; }
    public int nos_trader_price { get; set; }
    public int nos_flea_price { get; set; }
    public int nos_handbook_price { get; set; }
    public int nos_stock { get; set; }
    public int nos_loyalty_level { get; set; }
    public double nos_loot_duffle_bag_multiplier { get; set; }
    public double nos_loot_dead_scav_multiplier { get; set; }
    public double nos_loot_jacket_multiplier { get; set; }
    public double nos_loot_ration_supply_crate_multiplier { get; set; }
    public double nos_loot_ground_cache_multiplier { get; set; }
    public double nos_loose_loot_multiplier { get; set; }

    // stalker
    public bool stalker_enable { get; set; }
    public bool stalker_effect_toggle { get; set; }
    public bool stalker_sold_by_trader { get; set; }
    public bool stalker_flea_banned { get; set; }
    public int stalker_trader_price { get; set; }
    public int stalker_flea_price { get; set; }
    public int stalker_handbook_price { get; set; }
    public int stalker_stock { get; set; }
    public int stalker_loyalty_level { get; set; }
    public double stalker_loot_duffle_bag_multiplier { get; set; }
    public double stalker_loot_dead_scav_multiplier { get; set; }
    public double stalker_loot_jacket_multiplier { get; set; }
    public double stalker_loot_ration_supply_crate_multiplier { get; set; }
    public double stalker_loot_ground_cache_multiplier { get; set; }
    public double stalker_loose_loot_multiplier { get; set; }

    // bang
    public bool bang_enable { get; set; }
    public bool bang_effect_toggle { get; set; }
    public bool bang_sold_by_trader { get; set; }
    public bool bang_flea_banned { get; set; }
    public int bang_trader_price { get; set; }
    public int bang_flea_price { get; set; }
    public int bang_handbook_price { get; set; }
    public int bang_stock { get; set; }
    public int bang_loyalty_level { get; set; }
    public double bang_loot_duffle_bag_multiplier { get; set; }
    public double bang_loot_dead_scav_multiplier { get; set; }
    public double bang_loot_jacket_multiplier { get; set; }
    public double bang_loot_ration_supply_crate_multiplier { get; set; }
    public double bang_loot_ground_cache_multiplier { get; set; }
    public double bang_loose_loot_multiplier { get; set; }

    // ghost_swedish_fish
    public bool ghost_swedish_fish_enable { get; set; }
    public bool ghost_swedish_fish_effect_toggle { get; set; }
    public bool ghost_swedish_fish_sold_by_trader { get; set; }
    public bool ghost_swedish_flea_banned { get; set; }
    public int ghost_swedish_fish_trader_price { get; set; }
    public int ghost_swedish_fish_flea_price { get; set; }
    public int ghost_swedish_fish_handbook_price { get; set; }
    public int ghost_swedish_fish_stock { get; set; }
    public int ghost_swedish_fish_loyalty_level { get; set; }
    public double ghost_swedish_fish_loot_duffle_bag_multiplier { get; set; }
    public double ghost_swedish_fish_loot_dead_scav_multiplier { get; set; }
    public double ghost_swedish_fish_loot_jacket_multiplier { get; set; }
    public double ghost_swedish_fish_loot_ration_supply_crate_multiplier { get; set; }
    public double ghost_swedish_fish_loot_ground_cache_multiplier { get; set; }
    public double ghost_swedish_fish_loose_loot_multiplier { get; set; }

    // c4_starburst
    public bool c4_starburst_enable { get; set; }
    public bool c4_starburst_effect_toggle { get; set; }
    public bool c4_starburst_sold_by_trader { get; set; }
    public bool c4_starburst_flea_banned { get; set; }
    public int c4_starburst_trader_price { get; set; }
    public int c4_starburst_flea_price { get; set; }
    public int c4_starburst_handbook_price { get; set; }
    public int c4_starburst_stock { get; set; }
    public int c4_starburst_loyalty_level { get; set; }
    public double c4_starburst_loot_duffle_bag_multiplier { get; set; }
    public double c4_starburst_loot_dead_scav_multiplier { get; set; }
    public double c4_starburst_loot_jacket_multiplier { get; set; }
    public double c4_starburst_loot_ration_supply_crate_multiplier { get; set; }
    public double c4_starburst_loot_ground_cache_multiplier { get; set; }
    public double c4_starburst_loose_loot_multiplier { get; set; }

    // starbucks
    public bool starbucks_enable { get; set; }
    public bool starbucks_effect_toggle { get; set; }
    public bool starbucks_sold_by_trader { get; set; }
    public bool starbucks_flea_banned { get; set; }
    public int starbucks_trader_price { get; set; }
    public int starbucks_flea_price { get; set; }
    public int starbucks_handbook_price { get; set; }
    public int starbucks_stock { get; set; }
    public int starbucks_loyalty_level { get; set; }
    public double starbucks_loot_duffle_bag_multiplier { get; set; }
    public double starbucks_loot_dead_scav_multiplier { get; set; }
    public double starbucks_loot_jacket_multiplier { get; set; }
    public double starbucks_loot_ration_supply_crate_multiplier { get; set; }
    public double starbucks_loot_ground_cache_multiplier { get; set; }
    public double starbucks_loose_loot_multiplier { get; set; }

    // monster_punch
    public bool monster_punch_enable { get; set; }
    public bool monster_punch_effect_toggle { get; set; }
    public bool monster_punch_sold_by_trader { get; set; }
    public bool monster_punch_flea_banned { get; set; }
    public int monster_punch_trader_price { get; set; }
    public int monster_punch_flea_price { get; set; }
    public int monster_punch_handbook_price { get; set; }
    public int monster_punch_stock { get; set; }
    public int monster_punch_loyalty_level { get; set; }
    public double monster_punch_loot_duffle_bag_multiplier { get; set; }
    public double monster_punch_loot_dead_scav_multiplier { get; set; }
    public double monster_punch_loot_jacket_multiplier { get; set; }
    public double monster_punch_loot_ration_supply_crate_multiplier { get; set; }
    public double monster_punch_loot_ground_cache_multiplier { get; set; }
    public double monster_punch_loose_loot_multiplier { get; set; }

    // rockstar
    public bool rockstar_enable { get; set; }
    public bool rockstar_effect_toggle { get; set; }
    public bool rockstar_sold_by_trader { get; set; }
    public bool rockstar_flea_banned { get; set; }
    public int rockstar_trader_price { get; set; }
    public int rockstar_flea_price { get; set; }
    public int rockstar_handbook_price { get; set; }
    public int rockstar_stock { get; set; }
    public int rockstar_loyalty_level { get; set; }
    public double rockstar_loot_duffle_bag_multiplier { get; set; }
    public double rockstar_loot_dead_scav_multiplier { get; set; }
    public double rockstar_loot_jacket_multiplier { get; set; }
    public double rockstar_loot_ration_supply_crate_multiplier { get; set; }
    public double rockstar_loot_ground_cache_multiplier { get; set; }
    public double rockstar_loose_loot_multiplier { get; set; }

    // monster_doctor
    public bool monster_doctor_enable { get; set; }
    public bool monster_doctor_effect_toggle { get; set; }
    public bool monster_doctor_sold_by_trader { get; set; }
    public bool monster_doctor_flea_banned { get; set; }
    public int monster_doctor_trader_price { get; set; }
    public int monster_doctor_flea_price { get; set; }
    public int monster_doctor_handbook_price { get; set; }
    public int monster_doctor_stock { get; set; }
    public int monster_doctor_loyalty_level { get; set; }
    public double monster_doctor_loot_duffle_bag_multiplier { get; set; }
    public double monster_doctor_loot_dead_scav_multiplier { get; set; }
    public double monster_doctor_loot_jacket_multiplier { get; set; }
    public double monster_doctor_loot_ration_supply_crate_multiplier { get; set; }
    public double monster_doctor_loot_ground_cache_multiplier { get; set; }
    public double monster_doctor_loose_loot_multiplier { get; set; }

    // monster_pacific_punch
    public bool monster_pacific_punch_enable { get; set; }
    public bool monster_pacific_punch_effect_toggle { get; set; }
    public bool monster_pacific_punch_sold_by_trader { get; set; }
    public bool monster_pacific_punch_flea_banned { get; set; }
    public int monster_pacific_punch_trader_price { get; set; }
    public int monster_pacific_punch_flea_price { get; set; }
    public int monster_pacific_punch_handbook_price { get; set; }
    public int monster_pacific_punch_stock { get; set; }
    public int monster_pacific_punch_loyalty_level { get; set; }
    public double monster_pacific_punch_loot_duffle_bag_multiplier { get; set; }
    public double monster_pacific_punch_loot_dead_scav_multiplier { get; set; }
    public double monster_pacific_punch_loot_jacket_multiplier { get; set; }
    public double monster_pacific_punch_loot_ration_supply_crate_multiplier { get; set; }
    public double monster_pacific_punch_loot_ground_cache_multiplier { get; set; }
    public double monster_pacific_punch_loose_loot_multiplier { get; set; }

    // ghost_peaches
    public bool ghost_peaches_enable { get; set; }
    public bool ghost_peaches_effect_toggle { get; set; }
    public bool ghost_peaches_sold_by_trader { get; set; }
    public bool ghost_peaches_flea_banned { get; set; }
    public int ghost_peaches_trader_price { get; set; }
    public int ghost_peaches_flea_price { get; set; }
    public int ghost_peaches_handbook_price { get; set; }
    public int ghost_peaches_stock { get; set; }
    public int ghost_peaches_loyalty_level { get; set; }
    public double ghost_peaches_loot_duffle_bag_multiplier { get; set; }
    public double ghost_peaches_loot_dead_scav_multiplier { get; set; }
    public double ghost_peaches_loot_jacket_multiplier { get; set; }
    public double ghost_peaches_loot_ration_supply_crate_multiplier { get; set; }
    public double ghost_peaches_loot_ground_cache_multiplier { get; set; }
    public double ghost_peaches_loose_loot_multiplier { get; set; }

    // monster_lemonade
    public bool monster_lemonade_enable { get; set; }
    public bool monster_lemonade_effect_toggle { get; set; }
    public bool monster_lemonade_sold_by_trader { get; set; }
    public bool monster_lemonade_flea_banned { get; set; }
    public int monster_lemonade_trader_price { get; set; }
    public int monster_lemonade_flea_price { get; set; }
    public int monster_lemonade_handbook_price { get; set; }
    public int monster_lemonade_stock { get; set; }
    public int monster_lemonade_loyalty_level { get; set; }
    public double monster_lemonade_loot_duffle_bag_multiplier { get; set; }
    public double monster_lemonade_loot_dead_scav_multiplier { get; set; }
    public double monster_lemonade_loot_jacket_multiplier { get; set; }
    public double monster_lemonade_loot_ration_supply_crate_multiplier { get; set; }
    public double monster_lemonade_loot_ground_cache_multiplier { get; set; }
    public double monster_lemonade_loose_loot_multiplier { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraData { get; set; }
}
