using Godot;
using System;

public partial class IdleState : State
{
	[Export] public float Speed { get; set; } = 0.0f;
	public override void Enter()
	{
		Entity.Velocity = new Vector3(0, 0, 0);
	}

	public override void PhysicsUpdate(double delta)
	{

		if (Input.IsActionPressed("jump") && Entity.IsTouchingFloor || GetNodeOrNull<CoyoteComponent>("../../CoyoteComponent").IsActive)
		{
			TransitionTo("JumpState");
		}
		Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
		if (inputDir != Vector2.Zero)
		{
			TransitionTo("WalkState");
		}   
	}
}
