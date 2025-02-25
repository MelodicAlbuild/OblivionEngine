using System.Numerics;

namespace OblivionEngine.GameSystems;

public interface ILocation {}

public class Location : ILocation
{
    public int id;
    public string name;
    public LocationTypes type;
    public string description;
    public LocationZones zone;
    public List<Building> buildings;
    public List<Location> connections;

    public string mapPath;
    
    private JSONLocation jsLoc;

    public Location(JSONLocation jsLoc)
    {
        this.jsLoc = jsLoc;
        id = jsLoc.id;
        name = jsLoc.name;
        type = jsLoc.type;
        description = jsLoc.description;
        zone = jsLoc.zone;
        buildings = new List<Building>();
        connections = new List<Location>();
        
        string zoneStr = "";
        string typeStr = "";

        zoneStr = zone switch
        {
            LocationZones.Aurelian => "Aurelian Zone",
            LocationZones.Opening => "Opening Zone",
            LocationZones.Scorched => "Scorched Zone",
            LocationZones.Submerged => "Submerged Zone",
            LocationZones.Tidecliff => "Tidecliff Zone",
            LocationZones.Voltwood => "Voltwood Zone",
            LocationZones.Zenith => "Zenith Zone",
            LocationZones.MountAethoria => "Mount Aethoria Zone",
            _ => zoneStr
        };

        typeStr = type switch
        {
            LocationTypes.City => "Cities",
            LocationTypes.Town => "Towns",
            LocationTypes.Keystone => "Keystones",
            LocationTypes.Landmark => "Landmarks",
            LocationTypes.Passage => "Passages",
            LocationTypes.Transit => "Transits",
            _ => typeStr
        };

        mapPath = $"resources/tilemaps/{zoneStr}/{typeStr}/{name.Replace(" ", "")}.tmx";
        
        foreach (JSONBuilding jsLocBuilding in jsLoc.buildings)
        {
            buildings.Add(new Building(jsLocBuilding, this));
        }
    }

    public string GetTypeStr()
    {
        string typeStr = "";
        
        typeStr = type switch
        {
            LocationTypes.City => "Cities",
            LocationTypes.Town => "Towns",
            LocationTypes.Keystone => "Keystones",
            LocationTypes.Landmark => "Landmarks",
            LocationTypes.Passage => "Passages",
            LocationTypes.Transit => "Transits",
            _ => typeStr
        };

        return typeStr;
    }
    
    public string GetZoneStr()
    {
        string zoneStr = "";

        zoneStr = zone switch
        {
            LocationZones.Aurelian => "Aurelian Zone",
            LocationZones.Opening => "Opening Zone",
            LocationZones.Scorched => "Scorched Zone",
            LocationZones.Submerged => "Submerged Zone",
            LocationZones.Tidecliff => "Tidecliff Zone",
            LocationZones.Voltwood => "Voltwood Zone",
            LocationZones.Zenith => "Zenith Zone",
            LocationZones.MountAethoria => "Mount Aethoria Zone",
            _ => zoneStr
        };

        return zoneStr;
    }

    public void MergeConnections(SortedDictionary<int, Location> allLocations)
    {
        foreach (int con in jsLoc.connections)
        {
            connections.Add(allLocations.First(o => o.Key == con).Value);
        }
    }
}

public class Building : ILocation
{
    public int id;
    public string name;
    public BuildingTypes type;
    public string description;
    public Vector2 spawn;
    public Vector2 exitPos;
    public List<Vector2> connections;
    public List<Vector2> internalConnections;

    public Location parent;
    
    public string mapPath;
    
    public Building(JSONBuilding jsBuild, Location loc)
    {
        id = jsBuild.id;
        name = jsBuild.name;
        type = jsBuild.type;
        description = jsBuild.description;
        if (jsBuild.spawn != null)
        {
            spawn = new Vector2(jsBuild.spawn.x, jsBuild.spawn.y);
        }
        if (jsBuild.exitPos != null)
        {
            exitPos = new Vector2(jsBuild.exitPos.x, jsBuild.exitPos.y);
        }
        connections = new List<Vector2>();
        internalConnections = new List<Vector2>();

        parent = loc;
        
        foreach (JSONVector vector in jsBuild.connections)
        {
            connections.Add(new Vector2(vector.x, vector.y));
        }
        
        foreach (JSONVector vector in jsBuild.internalConnections)
        {
            internalConnections.Add(new Vector2(vector.x, vector.y));
        }
        
        mapPath = $"resources/tilemaps/{loc.GetZoneStr()}/{loc.GetTypeStr()}/{loc.name.Replace(" ", "")}.{id}.tmx";
    }
}