using System.Reflection;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Services.Modding.Custom;
using SPTarkov.Common.Models.Logging;
using SPTarkov.Server.Core.Helpers.Server;
using SPTarkov.Server.Core.Models.Spt.Tables;

namespace HoodsEnergyDrinks_CSharp;

[Injectable(TypePriority = OnLoadOrder.PostLoad + 5)]
public class HoodsEnergyDrinks(
    RagfairConfig ragfairConfig,
    ISptLogger<HoodsEnergyDrinks> logger,
    CustomItemService customItemService,
    ModHelper modHelper,
    TradersTable tradersTable,
    TemplateTable templateTable,
    LocationTable locationTable,
    GlobalTable globalTable,
    ModConfig config
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

    public Task OnLoadAsync(CancellationToken cancellationToken)
    {
        var pathToMod = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        var configPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(pathToMod, "config"));
        var drinks = modHelper.GetJsonDataFromFile<Drink>(pathToMod, "EnergyDrinkInfo.json");
        var assortCreator = new FluentTraderAssortCreator(tradersTable, logger);
        var traderHelper = new TraderHelper(assortCreator, config, drinks, logger);
        var itemCreator = new ItemCreator(config, drinks);
        itemCreator.BuildItems(globalTable, customItemService, modHelper);
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
            templateTable.Items["63dbd45917fff4dee40fe16e"], // lvl 1
            templateTable.Items["65424185a57eea37ed6562e9"], // lvl 2
            templateTable.Items["6542435ea57eea37ed6562f0"], // lvl 3
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

        MongoId hotRodEnergyDrinkId = "5751496424597720a27126da";
        foreach (var map in maps) 
        {
            string mapName = locationTable.GetMappedKey(map);
            Location location = locationTable.GetDictionary()[mapName];

            // Loose Loot Insertion
            location.LooseLoot?.AddTransformer(lazyLoadedLooseLoot =>
            {
                if (lazyLoadedLooseLoot == null) return lazyLoadedLooseLoot;

                foreach (var point in lazyLoadedLooseLoot.Spawnpoints)
                {
                    foreach (var itm in point.Template.Items)
                    {
                        if (itm.Template == hotRodEnergyDrinkId)
                        {
                            foreach (var dist in point.ItemDistribution) {
                                if(dist.ComposedKey.Key == itm.ComposedKey) {
                                    double? originalProbability = dist.RelativeProbability;

                                    //var origin = point.ItemDistribution
                                    foreach (var (name, props) in drinks.Items)
                                    {
                                        if (!config.drinks[name].enable) continue;

                                        var lootComposedKeyString = props._id + "_composedkey";
                                        var lootComposedKey = new ComposedKey { Key = lootComposedKeyString };
                                        var lootNewId = new MongoId();

                                        var newItem = new SptLootItem
                                        {
                                            Id = lootNewId,
                                            Template = props._id,
                                            ComposedKey = lootComposedKeyString
                                        };
                                        var itemsList = point.Template.Items.ToList() ?? new List<SptLootItem>();
                                        itemsList.Add(newItem);
                                        point.Template.Items = itemsList;

                                        var newLooseLootItemDistribution = new LooseLootItemDistribution
                                        {
                                            ComposedKey = lootComposedKey,
                                            RelativeProbability = originalProbability * config.drinks[name].loose_loot_multiplier
                                        };
                                        var itemDistribution = point.ItemDistribution?.ToList() ?? new List<LooseLootItemDistribution>();
                                        itemDistribution.Add(newLooseLootItemDistribution);
                                        point.ItemDistribution = itemDistribution;
                                    }
                                }
                            }
                        }
                    }
                }
                return lazyLoadedLooseLoot;
            });

            // Static Loot Insertion
            location.StaticLoot?.AddTransformer(lazyLoadedStaticLoot => 
            {
                if (lazyLoadedStaticLoot == null) return lazyLoadedStaticLoot;

                foreach (var (name, props) in drinks.Items)
                {
                    if (!config.drinks[name].enable) continue;

                    var staticLootProbabilities = itemCreator.loot.StaticLoot[props._id].Weights;

                    foreach (var (lootContainerString, probability) in staticLootProbabilities)
                    {
                        if (lootContainerMap.TryGetValue(lootContainerString, out var lootContainer))
                        {
                            float hot_rod_energy_prob = getProbability(lazyLoadedStaticLoot, lootContainerString, hotRodEnergyDrinkId);
                            try
                            {
                                var newItem = new ItemDistribution
                                {
                                    Tpl = props._id,
                                    RelativeProbability = probability * hot_rod_energy_prob
                                };

                                var list = lazyLoadedStaticLoot[lootContainer].ItemDistribution?.ToList() ?? new List<ItemDistribution>();
                                list.Add(newItem);
                                lazyLoadedStaticLoot[lootContainer].ItemDistribution = list;
                            }
                            catch
                            {
                                //logger.Error($"[Hoods Energy Drinks] Could not add {props._id} to container {getLootContainerString(lootContainer)} on map {map}");
                            }
                        }
                    }
                }
                return lazyLoadedStaticLoot;
            });
        }

        logger.Success("[Hoods Energy Drinks] Successfully added to server!");
        return Task.CompletedTask;
    }

    // Returns the relative probability of an item in a chosen loot container
    private float getProbability(Dictionary<MongoId, StaticLootDetails> mapStaticLoot, string lootContainerString, MongoId _id)
    {
        float defaultWeight = 400;
        MongoId lootContainer = lootContainerMap[lootContainerString];

        foreach (var (containerId, props) in mapStaticLoot)
        {
            if (containerId != lootContainer) continue;

            foreach (var item in props.ItemDistribution)
            {
                if (item.Tpl == _id)
                {
                    //logger.Success("Found Item in Item Distribution");
                    //logger.Info($"Found Relative Probability = {item.RelativeProbability} In container = {getLootContainerString(lootContainer)}");
                    return item.RelativeProbability ?? 0f;
                }
            }
        }
        //logger.Error($"Could Not Find Hot Rod Relative Probability in map = {map} lootContainer = {getLootContainerString(lootContainer)} Setting default weight value to {defaultWeight}");
        return defaultWeight;
    }

    //  used for logging only
    private string? getLootContainerString(MongoId _id)
    {
        foreach (var (key, value) in lootContainerMap)
        {
            if (value == _id) return key;
        }
        return null;
    }
}
