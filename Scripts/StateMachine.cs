using Godot;
using System;

public partial class StateMachine : Node
{
	[Export] public State InitialState { get; set; }

	private State _currentState;
	private IEntity _entity;

	public override void _Ready()
	{
		_entity = GetParent() as IEntity  ?? 
			throw new System.Exception("StateMachine must be child of IEntity");

		GD.Print(_entity.GetType());

		foreach (var child in GetChildren())
		{
			if (child is State state)
			{
				state.Finished += OnStateFinished;
				state.Initialize(_entity);
			}
		}

		_currentState = InitialState ?? GetChild(0) as State;
		_currentState.Enter();
	}


	private void OnStateFinished(string nextStatePath)
	{
		var nextState = GetNodeOrNull<State>(nextStatePath);
		GD.Print($"State finished:", _currentState.GetType());
		if (nextState == null)
		{
			GD.PrintErr($"State not found: {nextStatePath}");
			return;
		}

		_currentState?.Exit();
		_currentState = nextState;
		_currentState.Enter();
	}

	public override void _Process(double delta)
	{
		_currentState?.Update(delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		_currentState?.PhysicsUpdate(delta);
	}
}
