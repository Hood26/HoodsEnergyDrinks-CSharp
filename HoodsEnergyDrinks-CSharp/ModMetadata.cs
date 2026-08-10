
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Web;

namespace HoodsEnergyDrinks_CSharp;
public sealed record ModMetadata : IModMetadata, IModBlazorMetadata
{
    public string Name { get; init; } = "Hood's More Energy Drinks";
    public string Author { get; init; } = "Hood";
    public List<string>? Contributors { get; init; }
    public SemanticVersioning.Version Version { get; init; } = new("1.2.1");
    public SemanticVersioning.Range SptVersion { get; init; } = new("~4.1.0");
    public List<string>? Incompatibilities { get; init; }
    public Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
    public string? Url { get; init; } = "https://github.com/Hood26/HoodsEnergyDrinks-CSharp/tree/master";
    public string? License { get; init; } = "MIT";
    public string ModGuid { get; init; } = "com.hood.moreenergydrinks";
    public bool HasPrepatcher { get; init; } = false;
    public string? WWWRootUrl { get; init; }
    public string? HomePage { get; init; } = "/HoodsMoreEnergyDrinks";
    public string? HomePageDescription { get; init; } = "Home Page for Hood's More Energy Drinks";

}