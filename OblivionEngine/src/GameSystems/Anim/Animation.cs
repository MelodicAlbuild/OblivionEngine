namespace OblivionEngine.GameSystems.Anim;

public class Animation
{
    public int id;
    public int height;
    public int width;
    public int columns;
    public int rows;
    public int maxFrames;
    public string[] rowDefs;
    public string texturePath;
    
    public Animation(JSONAnimation json)
    {
        id = json.id;
        height = json.height;
        width = json.width;
        columns = json.columns;
        rows = json.rows;
        maxFrames = json.maxFrames;
        rowDefs = json.rowDefs;
        texturePath = json.texturePath;
    }
}