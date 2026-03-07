using Godot;
using System;

public partial class RunState : MovementState
{
	public override MovementMode MovementMode => MovementMode.Run;

	public override float StaminaConsumptionPerSecond => 10f;


	[Export] public float Speed { get; set; } = 50.0f;

	public override void Enter()
	{
		Vector3 velocity = Entity.Velocity;

		velocity.X = Speed * 10;

		Entity.Velocity = velocity;
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

		if (!CanEnter())
		{
			TransitionTo("WalkState");
		}

		Stamina.Consume(StaminaConsumptionPerSecond);
		Vector3 velocity = Entity.Velocity;
		velocity.X = inputDir.X * Speed;
		velocity.Z = inputDir.Y * Speed;
		Entity.Velocity = velocity;
		//// Rotate player to face direction
		//if (Entity.AsNode() is Player player)
		//{
		//    player.LookDirection(direction);
		//}

		//Entity.PlayAnimation("walk");
	}
	public override bool CanEnter()
	{
		GD.Print($"We know have stima:", Stamina.TryConsume(StaminaConsumptionPerSecond));
		return Stamina.TryConsume(StaminaConsumptionPerSecond);
	}
}
