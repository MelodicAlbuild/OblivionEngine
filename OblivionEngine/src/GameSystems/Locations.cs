using Newtonsoft.Json;
using SDL2;

namespace OblivionEngine.GameSystems;

public class Locations
{
    public SortedDictionary<int, Location> allLocations;
    
    public Locations()
    {
        allLocations = new SortedDictionary<int, Location>();
        LoadLocations();
    }

    private void LoadLocations()
    {
        DirectoryInfo d = new DirectoryInfo("resources/locations");
        FileInfo[] f = d.GetFiles("*.json");

        foreach (FileInfo fileInfo in f)
        {
            JSONLocation[] locs = JsonConvert.DeserializeObject<JSONLocation[]>(fileInfo.OpenText().ReadToEnd());
            foreach (JSONLocation location in locs)
            {
                allLocations.Add(location.id, new Location(location));
            }
        }
        
        foreach (KeyValuePair<int,Location> valuePair in allLocations)
        {
            valuePair.Value.MergeConnections(allLocations);
        }
    }
}