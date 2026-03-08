using Godot;
using System;

public partial class RunState : MovementState
{
	public override MovementMode MovementMode => MovementMode.Run;

	public override float StaminaConsumptionPerSecond => 10f;


	[Export] public float Speed { get; set; } = 10.0f;

	public override void Enter()
	{
		Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
		Vector3 moveDirection = Entity.GetMovementDirection(inputDir);

		Entity.Velocity = new Vector3(
			moveDirection.X * Speed,
			Entity.Velocity.Y,
			moveDirection.Z * Speed
		);
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

		if (Entity is IStamina)
		{
			var _entityAsStamina = (IStamina)Entity;
			_entityAsStamina.CanConsume(StaminaConsumptionPerSecond);
		}
		Vector3 moveDirection = Entity.GetMovementDirection(inputDir);

		Entity.Velocity = new Vector3(
			moveDirection.X * Speed,
			Entity.Velocity.Y,
			moveDirection.Z * Speed
		);
		//// Rotate player to face direction
		//if (Entity.AsNode() is Player player)
		//{
		//    player.LookDirection(direction);
		//}

		//Entity.PlayAnimation("walk");
	}
	public override bool CanEnter()
	{
		var _entityAsStamina = (IStamina)Entity;
		GD.Print($"We know have stima:", _entityAsStamina.CanConsume(StaminaConsumptionPerSecond));
		return _entityAsStamina.CanConsume(StaminaConsumptionPerSecond);
	}
}
