using Godot;
using System.Security.Cryptography.X509Certificates;
public abstract partial class MovementState : State
{
    public virtual MovementMode MovementMode => MovementMode.Idle;

    public virtual float StaminaConsumptionPerSecond => 0f;
    public virtual float StaminaRegenPerSecond => 0f;

    protected StaminaComponent Stamina;

    public void Initialize(IEntity entity, StaminaComponent stamina)
    {
        base.Initialize(entity);
        Stamina = stamina;
    }

    public virtual bool CanEnter() => true;
}
