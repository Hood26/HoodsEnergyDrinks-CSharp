
using System.Text.Json;
using SPTarkov.Server.Core.Models.Eft.Common;

namespace HoodsEnergyDrinks_CSharp;

public class EnergyDrinkBuffs
{
    public required Dictionary<string, List<BuffProps>> buffs { get; set; }

    public List<Buff> ToBuff(string buffKey)
    {
        if (!buffs.TryGetValue(buffKey, out var props))
            return new List<Buff>();

        return props.Select(p => new Buff
        {
            BuffType = p.BuffType,
            Chance = p.Chance,
            Delay = p.Delay,
            Duration = p.Duration,
            Value = p.Value,
            AbsoluteValue = p.AbsoluteValue,
            SkillName = p.SkillName
        }).ToList();
    }

}

public class BuffProps
{
    public bool AbsoluteValue { get; set; }
    public string BuffType { get; set; }
    public int Chance { get; set; }
    public int Delay { get; set; }
    public int Duration { get; set; }
    public string SkillName { get; set; }
    public double Value { get; set; }
}