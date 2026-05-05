namespace Howestprime.Backoffice.ViewModels;

public class LegendViewModel
{
    // TODO if time allows it this must become dynamic to allow the option to add or remove rooms at will
    public Dictionary<string, string> RoomsColorMap { get; private init; }= new()
    {
        { "Blue Room", "#ADD8E6" },
        { "blue", "#FFFF00" }
    };
}