// Player.cs
using Godot;

public partial class Player : CharacterBody3D, IEntity
{

	[Signal] public delegate void LeftGroundEventHandler();
	[Signal] public delegate void LandedEventHandler();

	private bool _wasOnFloor;
	[Export] public float Gravity { get; set; } = 25f;
	[Export] public float JumpVelocity { get; set; } = 7.0f;

	// Coyote Time & Buffering Config
	[Export] public float CoyoteTime { get; set; } = 0.1f;      // 100ms grace period
	[Export] public float JumpBufferTime { get; set; } = 0.1f;  // 100ms input queue

	private float _coyoteTimer = 0f;
	private float _jumpBufferTimer = 0f;

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
	public bool CanJump => IsTouchingFloor || _coyoteTimer > 0;

	public bool ConsumeJumpBuffer()
	{
		if (_jumpBufferTimer > 0)
		{
			_jumpBufferTimer = 0;
			return true;
		}
		return false;
	}

	public void ResetJumpBuffer() => _jumpBufferTimer = 0;

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
	}

	public override void _PhysicsProcess(double delta)
	{
		
		// 3. Let StateMachine handle movement logic
		GetNodeOrNull<StateMachine>("StateMachine")?._PhysicsProcess(delta);

		bool onFloor = IsOnFloor();

		if (_wasOnFloor && !onFloor)
			EmitSignal(SignalName.LeftGround);

		if (!_wasOnFloor && onFloor)
			EmitSignal(SignalName.Landed);

		_wasOnFloor = onFloor;
		
		if (!onFloor)
			Velocity += Vector3.Down * Gravity * (float)delta;

		MoveAndSlide();
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
