using Godot;

public abstract partial class State : Node
{
	[Signal]
	public delegate void FinishedEventHandler(string nextStatePath);

	protected IEntity Entity { get; set; }
	protected StateMachine StateMachine { get; private set; }

	// Called by StateMachine during initialization (Dependency Injection)
	public virtual void Initialize(IEntity entity)
	{
		Entity = entity;
		StateMachine = GetParent<StateMachine>();
	}

	public virtual void Enter() { }
	public virtual void Exit() { }
	public virtual void HandleInput(InputEvent @event) { }
	public virtual void Update(double delta) { }
	public virtual void PhysicsUpdate(double delta) { }

	protected void TransitionTo(string nextStatePath)
	{
		EmitSignal(SignalName.Finished, nextStatePath);
	}
}
