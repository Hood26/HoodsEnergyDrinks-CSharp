
using SPTarkov.Server.Core.Models.Spt.Mod;

namespace HoodsEnergyDrinks_CSharp;
public record ModMetadata : AbstractModMetadata
{
    public override string Name { get; init; } = "Hoods Energy Drinks";
    public override string Author { get; init; } = "Hood";
    public override List<string>? Contributors { get; init; }
    public override SemanticVersioning.Version Version { get; init; } = new("1.1.1");
    public override SemanticVersioning.Range SptVersion { get; init; } = new("~4.0.0");
    public override List<string>? Incompatibilities { get; init; }
    public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
    public override string? Url { get; init; } = "https://github.com/Hood26/HoodsEnergyDrinks-CSharp/tree/master";
    public override bool? IsBundleMod { get; init; } = true;
    public override string? License { get; init; } = "MIT";
    public override string ModGuid { get; init; } = "com.hood.moreenergydrinks";
}