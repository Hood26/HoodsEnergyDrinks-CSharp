using SPTarkov.Server.Core.Exceptions.Items;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
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
        var buffInfo = modHelper.GetJsonDataFromFile<EnergyDrinkBuffs>(pathToMod, "Buffs.json");
        var tableData = db.GetTables();
        tableData.Globals.Configuration.Health.Effects.Stimulator.Buffs["alternate_buffs"] = buffInfo.buffs["alternate_buffs"];

        foreach (var (name, props) in drinks.Items)
        {
            tableData.Globals.Configuration.Health.Effects.Stimulator.Buffs[name] = config.drinks[name].effect_toggle ? buffInfo.buffs[name] : [];

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
                    StimulatorBuffs = config.enable_alternate_buffs ? "alternate_buffs" : name,
                    EffectsHealth = new Dictionary<SPTarkov.Server.Core.Models.Enums.HealthFactor, EffectsHealthProperties>(),
                    EffectsDamage = new Dictionary<SPTarkov.Server.Core.Models.Enums.DamageEffectType, EffectsDamageProperties>(),
                },
                ParentId = "5448e8d64bdc2dce718b4568",
                NewId = props._id,
                FleaPriceRoubles = config.enable_alternate_buffs ? config.alternate_flea_price : config.drinks[name].flea_price,
                HandbookPriceRoubles = config.enable_alternate_buffs ? config.alternate_handbook_price : config.drinks[name].handbook_price,
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
}