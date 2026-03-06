using Godot;

public partial class WalkState : State
{
	[Export] public float Speed { get; set; } = 5.0f;

	public override void Enter()
	{
	}


	public override void PhysicsUpdate(double delta)
	{
		if (Input.IsActionJustPressed("jump") && Entity.CanJump)
		{
			GD.Print($"We jumped!", Entity.CanJump);
			TransitionTo("JumpState");
		}
		
		Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
		if (inputDir == Vector2.Zero)
		{
			TransitionTo("IdleState");
			return;
		}
		Vector3 velocity = Entity.Velocity;
		velocity.X = inputDir.X * Speed;
		velocity.Z = inputDir.Y * Speed;
		Entity.Velocity = velocity;
	}
}
