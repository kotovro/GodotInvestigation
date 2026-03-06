// JumpState.cs
using Godot;

public partial class JumpState : MovementState
{
	[Export] public float JumpVelocity { get; set; } = 7.0f;
	[Export] public float AirControl { get; set; } = 0.3f;
	[Export] public float AirAcceleration { get; set; } = 15f;
	[Export] public float VariableJumpCut { get; set; } = 0.5f;  

	public override void Enter()
	{
		Entity.Velocity = Vector3.Up * 5.0f;
	}

	public override void Exit()
	{
		
	}


	public override void PhysicsUpdate(double delta)
	{
		Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");

		Vector3 velocity = Entity.Velocity;

		// horizontal air control
		Vector3 direction = new Vector3(inputDir.X, 0, inputDir.Y).Normalized();
		velocity.X = direction.X * 10;
		velocity.Z = direction.Z * 10;

		Entity.Velocity = velocity;

		// ground transition
		if (Entity.IsTouchingFloor)
		{
			if (inputDir.Length() > 0.1f)
				TransitionTo("WalkState");
			else
				TransitionTo("IdleState");
		}
	}
}
