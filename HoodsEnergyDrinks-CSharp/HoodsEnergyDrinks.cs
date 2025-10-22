using System.Diagnostics.CodeAnalysis;
using System;
using System.Reflection;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Utils;
using SPTarkov.Server.Core.Utils.Json;
using SPTarkov.Server.Core.Models.Eft.Dialog;

namespace HoodsEnergyDrinks_CSharp;

public record ModMetadata : AbstractModMetadata
{
    public override string Name { get; init; } = "HoodsEnergyDrinks";
    public override string Author { get; init; } = "SPTarkov";
    public override List<string>? Contributors { get; init; }
    public override SemanticVersioning.Version Version { get; init; } = new("1.0.7");
    public override SemanticVersioning.Range SptVersion { get; init; } = new("~4.0.1");


    public override List<string>? Incompatibilities { get; init; }
    public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
    public override string? Url { get; init; }
    public override bool? IsBundleMod { get; init; } = true;
    public override string? License { get; init; } = "MIT";
    public override string ModGuid { get; init; } = "hood.moreenergydrinks";
}

[Injectable(TypePriority = OnLoadOrder.PostSptModLoader + 1)]
public class HoodsEnergyDrinks(
    ISptLogger<HoodsEnergyDrinks> logger,
    ConfigServer configServer,
    ModHelper modHelper,
    DatabaseService databaseService,
    JsonUtil jsonUtil,
    FluentTraderAssortCreator fluentAssortCreator
    ) 
    : IOnLoad
{
    public Task OnLoad() 
    {
        logger.Success("EnergyDrinksStart...");
        var pathToMod = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        var configPath = Path.GetFullPath(Path.Combine(pathToMod, "config"));
        var config = modHelper.GetJsonDataFromFile<ModConfig>(configPath, "config.jsonc");
        var drinkInfo = modHelper.GetJsonDataFromFile<DrinkInfo>(pathToMod, "DrinkInfo.json");
        //logger.Success($"Here is result: {config.alternate_flea_price}"); // test use case
        //var blue = config.drinks["monster_blue"];
        //logger.Success($"test 3: {blue.loot_multipliers["ration_supply_crate"]}"); // test use case
        //logger.Success($"test drink info: {drinkInfo.drinks["monster_green"].desc}");






        logger.Success("More energy drinks have been added to the server!");
        return Task.CompletedTask;
    }
}
