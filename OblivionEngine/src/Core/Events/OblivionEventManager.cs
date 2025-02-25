using OblivionEngine.Core.Events.EventArgs;
using OblivionEngine.Core.Extras;
using OblivionEngine.Render;

namespace OblivionEngine.Core.Events;

public class OblivionEventManager
{
    #region Instance

    public static OblivionEventManager Instance { get; set; }

    public OblivionEventManager()
    {
        Instance = this;
    }

    #endregion

    #region OnDraw

    public event EventHandler<OblivionRenderer> OnDraw;
    public void InvokeDraw(object sender, OblivionRenderer renderer)
    {
        OnDraw?.Invoke(sender, renderer);
    }

    #endregion
    
    #region OnLateDraw

    public event EventHandler<OblivionRenderer> OnLateDraw;
    public void InvokeLateDraw(object sender, OblivionRenderer renderer)
    {
        OnLateDraw?.Invoke(sender, renderer);
    }

    #endregion
    
    #region OnMove

    public event EventHandler<Direction> OnMove;
    public void InvokeMove(object sender, Direction direction)
    {
        OnMove?.Invoke(sender, direction);
    }

    #endregion
    
    #region OnUpdate

    public event EventHandler OnUpdate;
    public void InvokeUpdate(object sender)
    {
        OnUpdate?.Invoke(sender, System.EventArgs.Empty);
    }

    #endregion
}