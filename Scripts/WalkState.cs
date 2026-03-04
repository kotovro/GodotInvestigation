using Godot;

public partial class WalkState : State
{
	[Export] public float Speed { get; set; } = 5.0f;

	public override void Enter()
	{
		GD.Print("We entered walk state");
	}


	public override void PhysicsUpdate(double delta)
	{	
		if (Input.IsActionPressed("jump") && Entity.IsOnFloor)
		{
			TransitionTo("JumpState");
		}
	
		Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");

		// Stop if no input
		if (inputDir == Vector2.Zero)
		{
			TransitionTo("IdleState");
			return;
		}
		// Camera-relative movement
		


		// Convert 2D input to 3D direction (Y is 0)
		Vector3 direction = new Vector3(inputDir.X, 0, inputDir.Y).Normalized();

		// Apply velocity
		Entity.Velocity = new Vector3(direction.X * Speed, Entity.Velocity.Y, direction.Z * Speed);

		//// Rotate player to face direction
		//if (Entity.AsNode() is Player player)
		//{
		//    player.LookDirection(direction);
		//}

		//Entity.PlayAnimation("walk");
	}
}
