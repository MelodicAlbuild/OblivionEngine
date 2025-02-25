namespace OblivionEngine.GameSystems;

public class Location
{
    public int id { get; set; }
    public string name { get; set; }
    public LocationTypes type { get; set; }
    public string description { get; set; }
    public List<Location> connections { get; set; }
}