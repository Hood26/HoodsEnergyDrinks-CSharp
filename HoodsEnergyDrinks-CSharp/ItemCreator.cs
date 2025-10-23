
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services.Mod;
using System.Reflection;

namespace HoodsEnergyDrinks_CSharp;

class ItemCreator
{
    private ModConfig config;
    private DrinkInfo drinkInfo;

    public ItemCreator(ModConfig config, DrinkInfo drinkInfo)
    {
        this.config = config;
        this.drinkInfo = drinkInfo;
    }
    public void BuildItems(DatabaseServer db, CustomItemService customItem, ModHelper modHelper)
    {
        var pathToMod = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        var buffInfo = modHelper.GetJsonDataFromFile<EnergyDrinkBuffs>(pathToMod, "Buffs.json");
        var tableData = db.GetTables();
        tableData.Globals.Configuration.Health.Effects.Stimulator.Buffs["alternate_buffs"] = buffInfo.buffs["alternate_buffs"];

        foreach (KeyValuePair<string, DrinkProps> drink in drinkInfo.drinks)
        {
            tableData.Globals.Configuration.Health.Effects.Stimulator.Buffs[drink.Key] = config.drinks[drink.Key].effect_toggle ? buffInfo.buffs[drink.Key] : [];

            var newItem = new NewItemFromCloneDetails
            {
                ItemTplToClone = "5d40407c86f774318526545a",
                OverrideProperties = new TemplateItemProperties
                {
                    Prefab = new Prefab
                    {
                        Path = $"assets/{drink.Key}.bundle",
                        Rcid = ""
                    },
                    UsePrefab = new Prefab
                    {
                        Path = $"assets/{drink.Key}_container.bundle",
                        Rcid = ""
                    },
                    DiscardLimit = -1,
                    Weight = -0.6,
                    FoodUseTime = 5,
                    StimulatorBuffs = config.enable_alternate_buffs ? "alternate_buffs" : drink.Key,
                    EffectsHealth = { },
                    EffectsDamage = { }
                },
                ParentId = "5448e8d64bdc2dce718b4568",
                NewId = drink.Value._id,
                FleaPriceRoubles = config.enable_alternate_buffs ? config.alternate_flea_price : config.drinks[drink.Key].flea_price,
                HandbookPriceRoubles = config.enable_alternate_buffs ? config.alternate_handbook_price : config.drinks[drink.Key].handbook_price,
                HandbookParentId = "5b47574386f77428ca22b335",
                Locales = new Dictionary<string, LocaleDetails> {
                    {
                         "en",
                         new LocaleDetails
                         {
                            Name = drink.Value.name,
                            ShortName = drink.Value.shortName,
                            Description = drink.Value.desc
                         }
                    }
                }
            };
            customItem.CreateItemFromClone(newItem);
        }
    }
}