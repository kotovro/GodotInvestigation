using Godot;
using System;
using System.Threading;

public partial class Player : CharacterBody3D
{
	private float stamina = 0;
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	private Node3D _Head;

	public override void _Ready()
	{
		_Head = GetNode<Node3D>("Head");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			_Head.RotateY(mouseMotion.Relative.X * 0.01f);
			_Head.RotateX(mouseMotion.Relative.Y * 0.01f);
		}
	}
	
	private void Die()
	{	
		//Emit signal to retser the game or smt
		QueueFree();
	}
}
