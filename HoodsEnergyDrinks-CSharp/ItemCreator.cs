using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services.Mod;
using System.Reflection;
namespace HoodsEnergyDrinks_CSharp;

class ItemCreator
{
    public Loot loot = new Loot();
    private ModConfig config;
    private Drink drinks;

    public ItemCreator(ModConfig config, Drink drinks)
    {
        this.config = config;
        this.drinks = drinks;
    }
    public void BuildItems(DatabaseServer db, CustomItemService customItemService, ModHelper modHelper)
    {
        var pathToMod = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        var buffInfo = modHelper.GetJsonDataFromFile<EnergyDrinkBuffs>(pathToMod, "EnergyDrinkBuffs.json");
        var tableData = db.GetTables();

        foreach (var (name, props) in drinks.Items)
        {
            var currentBuff = config.use_alternate_buffs ? buffInfo.buffs["alternate_buffs"] : buffInfo.buffs[name];
            var drinkName =  config.use_alternate_buffs ? "alternate_buffs" : name;

            if (config.instant_energy_and_hydration && config.use_alternate_buffs) 
            {
                tableData.Globals.Configuration.Health.Effects.Stimulator.Buffs[name] = config.drinks[name].buff_effect_enable ? removeEnergyHydration(buffInfo, drinkName) : [];
            }
            else 
            {
                tableData.Globals.Configuration.Health.Effects.Stimulator.Buffs[name] = config.drinks[name].buff_effect_enable ? currentBuff : [];
            }


            var newItem = new NewItemFromCloneDetails
            {
                ItemTplToClone = "5d40407c86f774318526545a",
                OverrideProperties = new TemplateItemProperties
                {
                    Prefab = new Prefab
                    {
                        Path = $"assets/{name}.bundle",
                        Rcid = ""
                    },
                    UsePrefab = new Prefab
                    {
                        Path = $"assets/{name}_container.bundle",
                        Rcid = ""
                    },
                    DiscardLimit = -1,
                    Weight = 0.6,
                    FoodUseTime = 5,
                    StimulatorBuffs = name,
                    EffectsHealth = config.instant_energy_and_hydration ? setInstantEnergyHydration(buffInfo, drinkName) : [],
                    EffectsDamage = [],
                },
                ParentId = "5448e8d64bdc2dce718b4568",
                NewId = props._id,
                FleaPriceRoubles = config.drinks[name].flea_price,
                HandbookPriceRoubles = config.drinks[name].handbook_price,
                HandbookParentId = "5b47574386f77428ca22b335",
                Locales = new Dictionary<string, LocaleDetails>
                {
                    {
                         "en",
                         new LocaleDetails
                         {
                            Name = props.name,
                            ShortName = props.shortName,
                            Description = props.desc
                         }
                    }
                }
            };
            this.loot.StaticLoot[props._id] = new StaticLoot { Weights = new(config.drinks[name].loot_multipliers) };
            this.loot.LooseLoot[props._id] = config.drinks[name].loose_loot_multiplier;
            customItemService.CreateItemFromClone(newItem);

        }
    }

    public Dictionary<HealthFactor, EffectsHealthProperties>? setInstantEnergyHydration(EnergyDrinkBuffs buffInfo, string name)
    {
        buffInfo.buffs.TryGetValue(name, out var buff);
        var list = buff.ToList();

        var effectsHealth = new Dictionary<HealthFactor, EffectsHealthProperties>
        {
            [HealthFactor.Energy] = new EffectsHealthProperties { Value = list[0].Duration * list[0].Value },
            [HealthFactor.Hydration] = new EffectsHealthProperties { Value = list[1].Duration * list[1].Value }
        };

        return effectsHealth;
    }

    public List<Buff> removeEnergyHydration(EnergyDrinkBuffs buffInfo, string name) 
    {
        buffInfo.buffs.TryGetValue(name, out var buff);
        var list = buff.ToList();
        list.RemoveRange(0, 2);
        return list;
    }
}