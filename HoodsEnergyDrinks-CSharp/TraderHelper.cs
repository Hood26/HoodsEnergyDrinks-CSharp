using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Utils;

namespace HoodsEnergyDrinks_CSharp;

class TraderHelper
{
    public void addSingleItemsToTrader(
        FluentTraderAssortCreator assortCreator,
        string traderId,
        ModConfig config,
        DrinkInfo info,
        ISptLogger<HoodsEnergyDrinks> logger)
    {
        foreach (KeyValuePair<string, DrinkProps> drink in info.drinks)
        {
            if (config.drinks[drink.Key].enable)
            {
                assortCreator.CreateSingleAssortItem(drink.Value._id)
                    .AddUnlimitedStackCount()
                    .AddBuyRestriction(config.drinks[drink.Key].stock)
                    .AddMoneyCost(Money.ROUBLES, config.enable_alternate_buffs ? config.alternate_trader_price : config.drinks[drink.Key].trader_price)
                    .AddLoyaltyLevel(config.drinks[drink.Key].loyalty_level)
                    .Export(traderId);
            }
        }
    }
}