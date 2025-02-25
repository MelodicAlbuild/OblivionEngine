namespace OblivionEngine.GameSystems.Anim;

public class JSONAnimation
{
    public int id { get; set; }
    public int height { get; set; }
    public int width { get; set; }
    public int columns { get; set; }
    public int rows { get; set; }
    public int maxFrames { get; set; }
    public string[] rowDefs { get; set; }
    public string texturePath { get; set; }
}