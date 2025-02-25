namespace OblivionEngine.GameSystems;

public class JSONLocation
{
    public int id { get; set; }
    public string name { get; set; }
    public LocationTypes type { get; set; }
    public string description { get; set; }
    public int[] connections { get; set; }
}