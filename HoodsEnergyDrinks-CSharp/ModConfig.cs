using System.Reflection;
using SPTarkov.DI.Annotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers.Server;
using SPTarkov.Server.Web.Services;
using SPTarkov.Server.Web.Models.Configs;

namespace HoodsEnergyDrinks_CSharp;

public class ModConfig
{
    public required bool instant_energy_and_hydration { get; set; }
    public required bool use_alternate_buffs { get; set; }
    public required int alternate_trader_price { get; set; }
    public required int alternate_flea_price { get; set; }
    public required int alternate_handbook_price { get; set; }
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

[Injectable]
public class ModConfigRegistration : IOnDIConstruct
{
    public static async Task OnDIConstructAsync(IServiceCollection serviceCollection, CancellationToken cancellationToken)
    {
        var pathToMod = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var configPath = Path.Combine(pathToMod ?? ".", "config", "config.json");

        if (File.Exists(configPath))
        {
            var json = await File.ReadAllTextAsync(configPath, cancellationToken);
            ModConfig modConfig = JsonSerializer.Deserialize<ModConfig>(json)!;
            serviceCollection.AddSingleton<ModConfig>(modConfig);
        }
        else
        {
            throw new InvalidOperationException("[Hoods More Energy Drinks] Config file does not exist! Reinstall this mod to fix this issue!");
        }
    }
}

[Injectable(InjectionType.Singleton)]
public class MyModConfigEditorProvider(ModConfig config) : IConfigEditorConfigProvider
{
    public IEnumerable<ConfigEditorConfigRegistration> GetConfigs()
    {
        yield return ConfigEditorConfigRegistration.Create(
            "com.hood.moreenergydrinks",
            "Hoods More Energy Drinks Config",
            config,
            Path.Combine("user", "mods", "HoodsMoreEnergyDrinks", "config", "config.json")
        );
    }
}






