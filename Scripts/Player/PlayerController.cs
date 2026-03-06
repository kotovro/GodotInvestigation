// Player.cs
using Godot;

public partial class PlayerController : CharacterBody3D, IEntity
{
	[Signal] public delegate void LeftGroundEventHandler();
	[Signal] public delegate void LandedEventHandler();

	private bool _wasOnFloor;
	[Export] public float Gravity { get; set; } = 25f;

	private CoyoteComponent _CoyoteTimer;
	private StateMachine _StateMachine;
	public StaminaComponent StaminaComponent;

	private Node3D _Head;
	private Camera3D _camera;

	[Export] public NodePath CameraPath { get; set; } = new NodePath("Head/Camera3D");

	public new Vector3 Velocity
	{
		get => base.Velocity;
		set => base.Velocity = value;
	}

	public bool IsTouchingFloor => IsOnFloor();
	// True if on ground OR still in coyote window
	public bool CanJump => IsTouchingFloor || _CoyoteTimer.IsActive;

	
	public void PlayAnimation(string name)
	{
		if (HasNode("AnimationPlayer"))
			GetNode<AnimationPlayer>("AnimationPlayer").Play(name);
	}

	public Node AsNode() => this;

	public override void _Ready()
	{
		_Head = GetNode<Node3D>("Head");
		if (HasNode(CameraPath))
			_camera = GetNode<Camera3D>(CameraPath);
		_CoyoteTimer = GetNode<CoyoteComponent>("CoyoteComponent");
		_StateMachine = GetNode<StateMachine>("StateMachine");
		StaminaComponent = GetNode<StaminaComponent>("StaminaComponent");

	}

	public override void _PhysicsProcess(double delta)
	{

		// 1. State logic (if needed)
		_StateMachine._PhysicsProcess(delta);

		StaminaComponent.Update((float)delta, _StateMachine.CurrentState);

		// 2. Apply gravity BEFORE move
		if (!IsOnFloor())
			Velocity += Vector3.Down * Gravity * (float)delta;

		// 3. Move physics body
		MoveAndSlide();

		// 4. Now check floor AFTER movement
		bool onFloor = IsOnFloor();

		if (_wasOnFloor && !onFloor)
			EmitSignal(SignalName.LeftGround);

		if (!_wasOnFloor && onFloor)
			EmitSignal(SignalName.Landed);

		_wasOnFloor = onFloor;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			RotateY(mouseMotion.Relative.X * 0.01f);
			_Head.RotateX(-mouseMotion.Relative.Y * 0.01f);
		}
	}
}
