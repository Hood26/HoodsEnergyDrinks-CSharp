
using SPTarkov.Server.Core.Models.Spt.Tables;

namespace HoodsEnergyDrinks_CSharp;

public class EnergyDrinkBuffs
{
    public required Dictionary<string, IEnumerable<Buff>> buffs { get; set; }
}
