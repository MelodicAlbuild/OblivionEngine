using Newtonsoft.Json;
using SDL2;

namespace OblivionEngine.GameSystems;

public class Locations
{
    public SortedDictionary<int, Location> allLocations;
    
    public Locations()
    {
        allLocations = new SortedDictionary<int, Location>();
    }

    public void LoadLocations(List<JSONLocation> jsLocs)
    {
        foreach (JSONLocation location in jsLocs)
        {
            allLocations.Add(location.id, new Location(location));
        }
        
        Console.WriteLine(allLocations.Count);
        
        foreach (KeyValuePair<int,Location> valuePair in allLocations)
        {
            valuePair.Value.MergeConnections(allLocations);
        }
    }
}