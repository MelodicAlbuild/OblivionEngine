using Newtonsoft.Json;

namespace OblivionEngine.GameSystems.Anim;

public class Animations
{
    public List<Animation> animations;
    
    public Animations()
    {
        animations = new List<Animation>();
    }

    public void LoadAnimations(List<Animation> anims)
    {
        animations = anims;
    } 
}