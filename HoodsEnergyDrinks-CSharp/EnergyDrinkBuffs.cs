
using SPTarkov.Server.Core.Models.Eft.Common;

namespace HoodsEnergyDrinks_CSharp;

public class EnergyDrinkBuffs
{
    public required Dictionary<string, IEnumerable<Buff>> buffs { get; set; }
}
