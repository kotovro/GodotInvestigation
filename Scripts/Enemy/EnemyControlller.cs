using Godot;
public partial class EnemyControlller : CharacterBody3D, IEntity
{
	
	[Signal] public delegate void DieEventHandler();

	[Export] public float Gravity { get; set; } = 25f;

	public bool IsTouchingFloor => IsOnFloor();

	public bool CanJump => IsOnFloor();

	EnemyStateMachine _stateMachine;

	public override void _Ready()
	{
		_stateMachine = GetNode<EnemyStateMachine>("EnemyStateMachine");
		
	}

	public override void _PhysicsProcess(double delta)
	{

		_stateMachine._PhysicsProcess(delta);

		if (!IsOnFloor())
			Velocity += Vector3.Down * 0 * (float)delta;

		MoveAndSlide();
	}

	public void PlayAnimation(string name)
	{
		throw new System.NotImplementedException();
	}

	public Node AsNode()
	{
		throw new System.NotImplementedException();
	}
}

//using Godot;
//using System;

//public partial class Hostilepyramid : CharacterBody3D, IEntity
//{
//    public const float Speed = 5.0f;
//    public const float JumpVelocity = 4.5f;

//    public override void _PhysicsProcess(double delta)
//    {
//        Vector3 velocity = Velocity;

//        // Add the gravity.
//        if (!IsOnFloor())
//        {
//            velocity += GetGravity() * (float)delta;
//        }

//        // Handle Jump.
//        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
//        {
//            velocity.Y = JumpVelocity;
//        }

//        // Get the input direction and handle the movement/deceleration.
//        // As good practice, you should replace UI actions with custom gameplay a/ctions.
//        //Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
//        Vector3 direction = (Transform.Basis * new Vector3(10, 0, 10)).Normalized();
//        if (direction != Vector3.Zero)
//        {
//            velocity.X = direction.X * Speed;
//            velocity.Z = direction.Z * Speed;
//        }
//        else
//        {
//            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
//            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
//        }

//        Velocity = velocity;
//        MoveAndSlide();
//    }
//}
