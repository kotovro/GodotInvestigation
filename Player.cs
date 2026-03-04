using Godot;
using System;
using System.Threading;

public partial class Player : CharacterBody3D, IEntity
{
	private float stamina = 0;
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	private Node3D _Head;

	// Interface property: wraps the built-in Velocity
	public new Vector3 Velocity
	{
		get => base.Velocity;
		set => base.Velocity = value;
	}

	// Interface property: wraps the built-in IsOnFloor()
	public new bool IsOnFloor => IsOnFloor();

	//// Interface method: Hook for animations (optional for now)
	//public void PlayAnimation(string name)
	//{
		//GD.Print($"[Player] PlayAnimation: {name}");
	//}

	// Interface method: Returns self as Node
	public Node AsNode() => this;
	public override void _Ready()
	{
		_Head = GetNode<Node3D>("Head");
	}

	public override void _PhysicsProcess(double delta)
	{
		// 1. Let the StateMachine handle movement logic first
		// The active state will modify 'Velocity' via the IEntity interface
		if (HasNode("StateMachine"))
		{
			GetNode<StateMachine>("StateMachine")._PhysicsProcess(delta);
		}
		GD.Print($"Curresnt vekocity is:", Velocity.X);

		// 2. Apply Gravity (Fallback)
		// Note: If your JumpState handles gravity, you might skip this, 
		// but keeping it ensures the player falls if no state does.
		if (!IsOnFloor)
		{
			Velocity += GetGravity() * (float)delta;
		}

		// 3. Move the character (Godot's built-in physics)
		MoveAndSlide();
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			this.RotateY(mouseMotion.Relative.X * 0.01f);
			_Head.RotateX(-mouseMotion.Relative.Y * 0.01f);
		}
	}
	
	private void Die()
	{	
		//Emit signal to retser the game or smt
		QueueFree();
	}
}
