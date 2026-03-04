using Godot;

public partial class JumpState : State
{
	[Export] public float Speed { get; set; } = 5.0f;
	[Export] public float JumpVelocity { get; set; } = 5.0f;
	[Export] public float AirControl { get; set; } = 0.3f; // 0 = no control, 1 = full control
	[Export] public float AirAcceleration { get; set; } = 10f;

	public override void Enter()
	{
		GD.Print("We entereded jump state");
		// Set Y velocity only, keep X/Z momentum
		Entity.Velocity = new Vector3(Entity.Velocity.X, JumpVelocity, Entity.Velocity.Z);
	}


	public override void PhysicsUpdate(double delta)
	{
		Vector2 inputDir = Input.GetVector("forward", "back", "left", "right");


		if (!Input.IsActionPressed("jump") && Entity.Velocity.Y < 0)
		{
			Entity.Velocity = new Vector3(Entity.Velocity.X, Entity.Velocity.Y * 0.9f, Entity.Velocity.Z);
		}

		if (Entity.IsOnFloor)
		{
			if (inputDir.Length() > 0.1f)
				TransitionTo("WalkState");
			else
				TransitionTo("IdleState");
		}
	}

}
