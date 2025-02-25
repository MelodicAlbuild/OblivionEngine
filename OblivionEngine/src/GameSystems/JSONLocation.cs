namespace OblivionEngine.GameSystems;

public class JSONLocation
{
    public int id { get; set; }
    public string name { get; set; }
    public LocationTypes type { get; set; }
    public string description { get; set; }
    public LocationZones zone { get; set; }
    public JSONBuilding[] buildings { get; set; }
    
    public int[] connections { get; set; }
}

public class JSONBuilding
{
    public int id { get; set; }
    public string name { get; set; }
    public BuildingTypes type { get; set; }
    public string description { get; set; }
    public JSONVector? spawn { get; set; }
    public JSONVector? exitPos { get; set; }
    public JSONVector[] connections { get; set; }
    public JSONVector[] internalConnections { get; set; }
}

public class JSONVector
{
    public int x { get; set; }
    public int y { get; set; }
}