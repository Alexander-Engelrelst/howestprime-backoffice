namespace Howestprime.Backoffice.ViewModels;

public class LegendViewModel
{
    public Dictionary<string, string> RoomsColorMap { get; private init; }= new()
    {
        { "Blue Room", "room-legend-yellow" },
        { "Yellow Room", "room-legend-yellow" }
    };
}