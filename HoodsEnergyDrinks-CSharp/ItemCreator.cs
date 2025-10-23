
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services.Mod;
using System.Reflection;

namespace HoodsEnergyDrinks_CSharp;

class ItemCreator
{
    private ModConfig config;

    public ItemCreator(ModConfig config)
    {
        this.config = config;
    }
    public void BuildItems(DatabaseServer db, CustomItemService customItem, ModHelper modHelper)
    {
        var pathToMod = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        var buffInfo = modHelper.GetJsonDataFromFile<EnergyDrinkBuffs>(pathToMod, "Buffs.json");

        var tableData = db.GetTables();
        //tableData.Globals.

        tableData.Globals.Configuration.Health.Effects.Stimulator.Buffs["alternate_buffs"] = buffInfo.ToBuff("alternate_buffs");

    }
}