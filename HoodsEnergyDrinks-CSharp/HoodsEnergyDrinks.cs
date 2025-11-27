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
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Diagnostics;

namespace HoodsEnergyDrinks_CSharp;

public record ModMetadata : AbstractModMetadata
{
    public override string Name { get; init; } = "Hoods Energy Drinks";
    public override string Author { get; init; } = "Hood";
    public override List<string>? Contributors { get; init; }
    public override SemanticVersioning.Version Version { get; init; } = new("1.1.0");
    public override SemanticVersioning.Range SptVersion { get; init; } = new("~4.0.0");


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
        var tables = db.GetTables();
        var ragfairConfig = configServer.GetConfig<RagfairConfig>();
        var assortCreator = new FluentTraderAssortCreator(databaseService, logger);
        var traderHelper = new TraderHelper(assortCreator, config, drinks, logger);
        var itemCreator = new ItemCreator(config, drinks);
        itemCreator.BuildItems(db, customItemService, modHelper);
        traderHelper.addSingleItemsToTrader("54cb57776803fa99248b456e");

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
                ragfairConfig.Dynamic.Blacklist.Custom.Add(drinks.Items[drink.Key]._id);
            }
        }

        // Add all energy drinks to all levels of Hall Of Fame
        foreach (var drink in drinks.Items)
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



        // Loose Loot Insertion (preload loose loot for maps in parallel to reduce wall-clock time)
        var startTime = Stopwatch.GetTimestamp();
        logger.Success("[Hoods Energy Drinks] Injecting energy drinks into loose loot spawns...");

        var mapLooseLoots = new ConcurrentDictionary<string, SPTarkov.Server.Core.Models.Eft.Common.LooseLoot?>();
        var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Math.Min(Environment.ProcessorCount, maps.Length) };

        Parallel.ForEach(maps, parallelOptions, map =>
        {
            string mapName = tables.Locations.GetMappedKey(map);
            Location location = tables.Locations.GetDictionary()[mapName];
            var loose = location.LooseLoot?.Value;
            mapLooseLoots[map] = loose;
        });

        foreach (var map in maps)
        {
            var mapLooseLoot = mapLooseLoots[map];

            foreach (var (name, props) in drinks.Items)
            {
                var lootComposedKeyString = props._id + "_composedkey";
                var lootComposedKey = new ComposedKey { Key = lootComposedKeyString };
                var lootNewId = new MongoId();
            }
        }

        var diff = Stopwatch.GetElapsedTime(startTime);
        logger.Info($"Loose Loot Algorithm Timer = {diff}");









        /*
            // Loose Loot Insertion
            foreach (var map in maps)
            {
                string mapName = tables.Locations.GetMappedKey(map);
                Location location = tables.Locations.GetDictionary()[mapName];
                var mapLooseLoot = location.LooseLoot.Value;

                foreach (var (name, props) in drinks.Items)
                {
                    var lootComposedKeyString = props._id + "_composedkey";
                    var lootComposedKey = new ComposedKey { Key = lootComposedKeyString};
                    var lootNewId = new MongoId();
                    //logger.Info($"Map Name = {map}");
                    //logger.Info("1");
                    //logger.Info("2");

                    //foreach (var point in mapLooseLoot.Spawnpoints)
                    //{
                        //foreach (var itm in point.Template.Items)
                        //{
                            //logger.Info($"item_tpl = {itm.Template}");
                            //if (itm.Template == "5751496424597720a27126da")
                            //{
                                /*
                                var originalItemComposedKey = itm.ComposedKey;
                                double? originRelativeProb;
                                foreach (var dist in point.ItemDistribution)
                                {
                                    if (dist.ComposedKey.Key == originalItemComposedKey)
                                    {
                                        originRelativeProb = dist.RelativeProbability;
                                        var newItem = new SptLootItem
                                        {
                                            Id = lootNewId,
                                            Template = props._id,
                                            ComposedKey = lootComposedKeyString
                                        };
                                        var itemsList = point.Template.Items.ToList() ?? new List<SptLootItem>();
                                        itemsList.Add(newItem);
                                        point.Template.Items = itemsList;
                                    }
                                }


                                var newLooseLootItemDistribution = new LooseLootItemDistribution
                                {
                                    ComposedKey = lootComposedKey,
                                    RelativeProbability = 9999999999
                                };
                                var itemDistribution = point.ItemDistribution.ToList() ?? new List<LooseLootItemDistribution>();
                                itemDistribution.Add(newLooseLootItemDistribution);
                                point.ItemDistribution = itemDistribution;

                                // Transformer
                                location.LooseLoot.AddTransformer(lazyLoadedLooseLoot =>
                                {
                                    if (lazyLoadedLooseLoot == null) return lazyLoadedLooseLoot;

                                    foreach (var point in lazyLoadedLooseLoot.Spawnpoints)
                                    {
                                        foreach (var itm in point.Template.Items)
                                        {
                                            if (itm.Template == "5751496424597720a27126da")
                                            {
                                                var originalItemComposedKey = itm.ComposedKey;
                                                double? originRelativeProb;
                                                foreach (var dist in point.ItemDistribution)
                                                {
                                                    if (dist.ComposedKey.Key == originalItemComposedKey)
                                                    {
                                                        originRelativeProb = dist.RelativeProbability;
                                                        var newItem = new SptLootItem
                                                        {
                                                            Id = lootNewId,
                                                            Template = props._id,
                                                            ComposedKey = lootComposedKeyString
                                                        };
                                                        var itemsList = point.Template.Items.ToList() ?? new List<SptLootItem>();
                                                        itemsList.Add(newItem);
                                                        point.Template.Items = itemsList;
                                                    }
                                                }

                                                var newLooseLootItemDistribution = new LooseLootItemDistribution
                                                {
                                                    ComposedKey = lootComposedKey,
                                                    RelativeProbability = 9999999999
                                                };
                                                var itemDistribution = point.ItemDistribution.ToList() ?? new List<LooseLootItemDistribution>();
                                                itemDistribution.Add(newLooseLootItemDistribution);
                                                point.ItemDistribution = itemDistribution;
                                            }
                                        }
                                    }
                                    return lazyLoadedLooseLoot;
                                });
                                */
        //}
        //}
        //}
        //}
        //}

        var startTime2 = Stopwatch.GetTimestamp();
        // Static Loot Insertion
        foreach (var (name, props) in drinks.Items)
        {
            foreach (var map in maps)
            {
                string mapName = tables.Locations.GetMappedKey(map);
                Location location = tables.Locations.GetDictionary()[mapName];
                var mapStaticLoot = location.StaticLoot.Value;
                var staticLootProbabilities = itemCreator.loot.StaticLoot[props._id].Weights;

                foreach (var (lootContainerString, probability) in staticLootProbabilities)
                {
                    if (lootContainerMap.TryGetValue(lootContainerString, out var lootContainer))
                    {
                        float hot_rod_energy_prob = getProbability(mapStaticLoot, lootContainerString, "5751496424597720a27126da", map);

                        try
                        {
                            var newItem = new ItemDistribution
                            {
                                Tpl = props._id,
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

                                //logger.Info($"Adding item Tpl={newItem.Tpl} RelativeProbability={newItem.RelativeProbability} LootContainer = {getLootContainerString(lootContainer)} Map = {map}");
                                var updatedItemDistribution = details.ItemDistribution?.ToList() ?? new List<ItemDistribution>();
                                updatedItemDistribution.Add(newItem);
                                lazyLoadedStaticLoot[lootContainer].ItemDistribution = updatedItemDistribution;

                                return lazyLoadedStaticLoot;
                            });
                        }
                        catch
                        {
                            //logger.Error($"[Hoods Energy Drinks] Could not add {props._id} to container {getLootContainerString(lootContainer)} on map {map}");
                        }
                    }
                }
                //break;
            }
            //break;
        }
        var diff2 = Stopwatch.GetElapsedTime(startTime2);
        logger.Info($"Static Loot Algorithm Timer = {diff2}");

        logger.Success("[Hoods Energy Drinks] Successfully added to server!");
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
                    //logger.Success("Found Item in Item Distribution");
                    //logger.Info($"Found Relative Probability = {item.RelativeProbability} In container = {getLootContainerString(lootContainer)}");
                    return item.RelativeProbability ?? 0f;
                }
            }
        }

        float defaultWeight = 400;
        //logger.Error($"Could Not Find Hot Rod Relative Probability in map = {map} lootContainer = {getLootContainerString(lootContainer)} Setting default weight value to {defaultWeight}");
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
