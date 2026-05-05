namespace Howestprime.Backoffice.ViewModels;

public class LegendViewModel
{
    // TODO if time allows it this must become dynamic to allow the option to add or remove rooms at will
    // to do this we will probably need to generate hex values at will but with a predictable pattern,
    // such that refreshing or something doesn't just change all the colors
    public Dictionary<string, string> RoomsColorMap { get; private init; }= new()
    {
        { "Blue Room", "#ADD8E6" },
        { "Yellow Room", "#FFFF00" }
    };
}