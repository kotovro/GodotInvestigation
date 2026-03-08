using Godot;

public partial class PlayerController : CharacterBody3D, IEntity, IStamina, IDamageable
{
	[Signal] public delegate void LeftGroundEventHandler();
	[Signal] public delegate void LandedEventHandler();

	private bool _wasOnFloor;
	[Export] public float Gravity { get; set; } = 25f;

	private StateMachine _stateMachine;
	private HealthComponent _health;
	public StaminaComponent _staminaComponent;
	private CoyoteComponent _coyoteTimer;


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
	public bool CanJump => IsTouchingFloor || _coyoteTimer.IsActive;
	public Node AsNode() => this;



	public float CurrentStamina => _staminaComponent?.CurrentStamina ?? 0;
	public float MaxStamina => _staminaComponent?.MaxStamina ?? 100;

	public bool CanConsume(float cost) => _staminaComponent?.CanConsume(cost) ?? true;
	public void Regenerate(float amount) => _staminaComponent?.Regenerate(amount);

	public float CurrentHealth => _health?.CurrentHealth ?? 0;
	public float MaxHealth => _health?.MaxHealth ?? 100;
	public bool IsAlive => _health?.IsAlive ?? true;

	public void TakeDamage(float amount, Vector3 hitDirection, Node damageSource)
	{
		_health?.TakeDamage(amount, hitDirection, damageSource);
	}

	public void Heal(float amount) => _health?.Heal(amount);

	public void Die()
	{
		GD.Print("[Enemy] Died!");
		// Drop loot, play death animation, etc.
		QueueFree();
	}
	public void PlayAnimation(string name)
	{
		if (HasNode("AnimationPlayer"))
			GetNode<AnimationPlayer>("AnimationPlayer").Play(name);
	}

	
	public override void _Ready()
	{
		_Head = GetNode<Node3D>("Head");
		if (HasNode(CameraPath))
			_camera = GetNode<Camera3D>(CameraPath);
		_coyoteTimer = GetNode<CoyoteComponent>("CoyoteComponent");
		_stateMachine = GetNode<StateMachine>("StateMachine");
		_staminaComponent = GetNode<StaminaComponent>("StaminaComponent");

	}

	public Vector3 GetMovementDirection(Vector2 input)
	{
		if (_camera == null || input.Length() < 0.1f)
			return Vector3.Zero;

		Basis camBasis = _camera.GlobalTransform.Basis;
		Vector3 camForward = -camBasis.Z; // Godot camera forward is -Z
		Vector3 camRight = camBasis.X;

		camForward.Y = 0;
		camRight.Y = 0;

		camForward = camForward.Normalized();
		camRight = camRight.Normalized();


		Vector3 direction = (camRight * input.X) + (camForward * -input.Y);

		return direction.Normalized();
	}
	public void SetLookDirection(Vector3 direction)
	{
		if (direction.Length() > 0.1f)
		{
			LookAt(GlobalTransform.Origin + direction, Vector3.Up);
		}
	}


	public override void _PhysicsProcess(double delta)
	{

		// 1. State logic (if needed)
		_stateMachine._PhysicsProcess(delta);

		_staminaComponent.Update((float)delta);

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
