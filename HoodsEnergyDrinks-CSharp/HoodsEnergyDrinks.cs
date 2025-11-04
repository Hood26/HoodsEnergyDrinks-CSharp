using System.Reflection;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Utils;
using SPTarkov.Server.Core.Services.Mod;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Eft.Common;
using Microsoft.VisualBasic;
using SPTarkov.Server.Core.Models.Common;
using System.ComponentModel.Design;
using JetBrains.Annotations;

namespace HoodsEnergyDrinks_CSharp;

public record ModMetadata : AbstractModMetadata
{
    public override string Name { get; init; } = "HoodsEnergyDrinks";
    public override string Author { get; init; } = "Hood";
    public override List<string>? Contributors { get; init; }
    public override SemanticVersioning.Version Version { get; init; } = new("1.1.0");
    public override SemanticVersioning.Range SptVersion { get; init; } = new("~4.0.3");


    public override List<string>? Incompatibilities { get; init; }
    public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
    public override string? Url { get; init; } = "https://github.com/Hood26/HoodsEnergyDrinks-CSharp/tree/master";
    public override bool? IsBundleMod { get; init; } = true;
    public override string? License { get; init; } = "MIT";
    public override string ModGuid { get; init; } = "com.hood.moreenergydrinks";
}

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class HoodsEnergyDrinks(
    ISptLogger<HoodsEnergyDrinks> logger,
    ConfigServer configServer,
    CustomItemService customItemService,
    ModHelper modHelper,
    DatabaseService databaseService,
    DatabaseServer db,
    JsonUtil jsonUtil,
    FluentTraderAssortCreator fluentAssortCreator
    )
    : IOnLoad
{

    private readonly Dictionary<string, MongoId> lootContainerMap = new()
    {
        { "duffle_bag", "578f87a3245977356274f2cb" },
        { "dead_scav", "5909e4b686f7747f5b744fa4" },
        { "jacket", "578f8778245977358849a9b5" },
        { "ration_supply_crate", "5d6fd13186f77424ad2a8c69" },
        { "ground_cache", "5d6d2b5486f774785c2ba8ea" }
    };

    public Task OnLoad()
    {
        var pathToMod = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        var configPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(pathToMod, "config"));
        var config = modHelper.GetJsonDataFromFile<ModConfig>(configPath, "config.jsonc");
        var drinks = modHelper.GetJsonDataFromFile<Drink>(pathToMod, "drinkInfo.json");
        //logger.Success($"test drink info: {drink.drinks["monster_green"].desc}");
        var tables = db.GetTables();
        var ragfairConfig = configServer.GetConfig<RagfairConfig>();
        var traderHelper = new TraderHelper();
        var assortCreator = new FluentTraderAssortCreator(databaseService, logger);
        var itemCreator = new ItemCreator(config, drinks);
        itemCreator.BuildItems(db, customItemService, modHelper);
        traderHelper.addSingleItemsToTrader(assortCreator, "54cb57776803fa99248b456e", config, drinks, logger);

        string[] maps = [
            "bigmap",      // customs
            "factory4_day",
            "factory4_night",
            "woods",
            "rezervbase",
            "shoreline",
            "interchange",
            "tarkovstreets",
            "lighthouse",
            "laboratory",
            "sandbox",     // groundzero
            "sandbox_high" // groundzero_lvl_20+
        ];

        List<TemplateItem> hallOfFameIds = [
            tables.Templates.Items["63dbd45917fff4dee40fe16e"], // lvl 1
            tables.Templates.Items["65424185a57eea37ed6562e9"], // lvl 2
            tables.Templates.Items["6542435ea57eea37ed6562f0"], // lvl 3
        ];


        // flea ban energy drinks
        foreach (var drink in config.drinks)
        {
            if (drink.Value.flea_banned)
            {
                ragfairConfig.Dynamic.Blacklist.Custom.Add(drinks.Props[drink.Key]._id);
            }
        }

        // Add all energy drinks to all levels of Hall Of Fame
        foreach (var drink in drinks.Props)
        {
            hallOfFameIds.ForEach((hall) =>
            {
                foreach (var slot in hall.Properties.Slots)
                {
                    foreach (var filter in slot.Properties.Filters)
                    {
                        if (filter.Filter.Contains(drink.Value._id))
                        {
                            filter.Filter.Add(drink.Value._id);
                        }
                    }
                }
            });
        }

        /* Loose Loot Insertion
        foreach (var drink in drinks.Props)
        {
            var lootComposedKey = drink.Value._id;
            foreach (var map in maps)
            {
                string mapName = tables.Locations.GetMappedKey(map);
                Location location = tables.Locations.GetDictionary()[mapName];
                //logger.Success($"map =  {map}");
                //logger.Success($"mapName =  {mapName}");

            }
        }
        */

        // Static Loot Insertion
        foreach (var drink in drinks.Props)
        {
            foreach (var map in maps)
            {
                string mapName = tables.Locations.GetMappedKey(map);
                Location location = tables.Locations.GetDictionary()[mapName];
                var mapStaticLoot = location.StaticLoot.Value;
                var staticLootProbabilities = itemCreator.loot.StaticLoot[drink.Value._id].Weights;

                foreach (var (lootContainerString, probability) in staticLootProbabilities)
                {
                    if (lootContainerMap.TryGetValue(lootContainerString, out var lootContainer))
                    {
                        float hot_rod_energy_prob = getProbability(mapStaticLoot, lootContainerString, "5751496424597720a27126da", map);

                        try
                        {
                            var newItem = new ItemDistribution
                            {
                                Tpl = drink.Value._id,
                                RelativeProbability = MathF.Ceiling(probability * hot_rod_energy_prob)
                            };

                            var list = mapStaticLoot[lootContainer].ItemDistribution?.ToList() ?? new List<ItemDistribution>();
                            list.Add(newItem);
                            mapStaticLoot[lootContainer].ItemDistribution = list;

                            //Add to Lazy Loaded Loot
                            location.StaticLoot.AddTransformer(lazyLoadedStaticLoot =>
                            {
                                if (lazyLoadedStaticLoot == null) return lazyLoadedStaticLoot;
                                if (!lazyLoadedStaticLoot.TryGetValue(lootContainer, out StaticLootDetails? details)) return lazyLoadedStaticLoot;

                                logger.Info($"Adding item Tpl={newItem.Tpl} RelativeProbability={newItem.RelativeProbability} LootContainer = {getLootContainerString(lootContainer)} Map = {map}");
                                var updatedItemDistribution = details.ItemDistribution?.ToList() ?? new List<ItemDistribution>();
                                updatedItemDistribution.Add(newItem);
                                lazyLoadedStaticLoot[lootContainer].ItemDistribution = updatedItemDistribution;

                                return lazyLoadedStaticLoot;
                            });
                        }
                        catch
                        {
                            logger.Error($"[Hoods Energy Drinks] Could not add {drink.Value._id} to container {getLootContainerString(lootContainer)} on map {map}");
                        }
                    }
                }
                //break;
            }
            //break;
        }


        logger.Success("[Hoods Energy Drinks] Energy Drinks successfully added to server!");
        return Task.CompletedTask;
    }




    // Returns the relative probability of an item in a chosen lootContainer and map.
    private float getProbability(Dictionary<MongoId, StaticLootDetails> mapStaticLoot, string lootContainerString, MongoId _id, string map)
    {
        MongoId lootContainer = lootContainerMap[lootContainerString];

        foreach (var (key, value) in mapStaticLoot)
        {
            if (key != lootContainer) continue;

            foreach (var item in value.ItemDistribution)
            {
                if (item.Tpl == _id)
                {
                    logger.Success("Found Item in Item Distribution");
                    logger.Info($"Found Relative Probability = {item.RelativeProbability} In container = {getLootContainerString(lootContainer)}");
                    return item.RelativeProbability ?? 0f;
                }
            }
        }

        float defaultWeight = 500;
        logger.Error($"Could Not Find Hot Rod Relative Probability in map = {map} lootContainer = {getLootContainerString(lootContainer)} Setting default weight value to {defaultWeight}");
        return defaultWeight;
    }


    private string? getLootContainerString(MongoId _id)
    {
        foreach (var (key, value) in lootContainerMap)
        {
            if (value == _id) return key;
        }
        return null;
    }




}
