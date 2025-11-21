using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Utils;

namespace HoodsEnergyDrinks_CSharp;

class TraderHelper(FluentTraderAssortCreator assortCreator, ModConfig config, Drink drinks, ISptLogger<HoodsEnergyDrinks> logger)
{
    private readonly FluentTraderAssortCreator assortCreator = assortCreator;
    private readonly ModConfig config = config;
    private readonly Drink drinks = drinks;
    private readonly ISptLogger<HoodsEnergyDrinks> logger = logger;

    public void addSingleItemsToTrader(string traderId)
    {
        foreach (var (name, props) in drinks.Items)
        {
            if (config.drinks[name].sold_by_trader)
            {
                assortCreator.CreateSingleAssortItem(props._id)
                    .AddUnlimitedStackCount()
                    .AddBuyRestriction(config.drinks[name].trader_stock)
                    .AddMoneyCost(Money.ROUBLES, config.enable_alternate_buffs ? config.alternate_trader_price : config.drinks[name].trader_price)
                    .AddLoyaltyLevel(config.drinks[name].loyalty_level)
                    .Export(traderId);
            }
        }
    }
}