// JumpState.cs
using Godot;

public partial class JumpState : State
{
	[Export] public float JumpVelocity { get; set; } = 7.0f;
	[Export] public float AirControl { get; set; } = 0.3f;       // 0.0-1.0 steering authority
	[Export] public float AirAcceleration { get; set; } = 15f;   // How fast steering kicks in
	[Export] public float VariableJumpCut { get; set; } = 0.5f;  // Velocity multiplier on early release


	public override void Enter()
	{

		// Apply initial jump impulse
		Entity.Velocity = new Vector3(JumpVelocity, JumpVelocity, JumpVelocity);

	}

	public override void Exit()
	{
		// Reset buffer when leaving jump state
		if (Entity.AsNode() is Player player)
			player.ResetJumpBuffer();
	}


	public override void PhysicsUpdate(double delta)
	{
		Vector2 inputDir = Input.GetVector("forward", "back", "left", "right");

		Vector3 direction = new Vector3(inputDir.X, 0, inputDir.Y).Normalized();
		Entity.Velocity = new Vector3(direction.X * 10, Entity.Velocity.Y, direction.Z * 10);

		if (Entity.IsOnFloor)
		{

			if (inputDir.Length() > 0.1f)
				TransitionTo("WalkState");
			else
				TransitionTo("IdleState");
		}
	}
}
