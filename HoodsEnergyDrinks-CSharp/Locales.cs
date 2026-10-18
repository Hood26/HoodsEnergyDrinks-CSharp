namespace HoodsEnergyDrinks_CSharp;

public class Locales
{
    public required Dictionary<string, Dictionary<string, LocaleProps>> Items { get; set; }
}

public class LocaleProps
{
    public required string name { get; set; }
    public required string desc { get; set; }

}