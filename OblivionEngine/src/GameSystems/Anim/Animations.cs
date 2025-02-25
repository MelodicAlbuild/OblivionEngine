using Newtonsoft.Json;

namespace OblivionEngine.GameSystems.Anim;

public class Animations
{
    public List<Animation> animations;
    
    public Animations()
    {
        animations = new List<Animation>();
        LoadAnimations();
    }
    
    private void LoadAnimations()
    {
        DirectoryInfo d = new DirectoryInfo("resources/animations");
        FileInfo[] f = d.GetFiles("*.json");

        foreach (FileInfo fileInfo in f)
        {
            JSONAnimation anim = JsonConvert.DeserializeObject<JSONAnimation>(fileInfo.OpenText().ReadToEnd());
            animations.Add(new Animation(anim));
        }
    }
}