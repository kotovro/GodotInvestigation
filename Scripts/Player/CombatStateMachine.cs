using Godot;

/// TODO: сделать универсальнюу машину сосоятний с дженериками,
/// потому что по сути свойе машины определяют, как будт осуществляться переходж 
public partial class CombatStateMachine : Node
{
    [Export] public CombatState InitialState { get; set; }

    private CombatState CurrentState { get; set; }
    public override void _Ready()
    {
        var entity = GetParent() as IEntity ??
            throw new System.Exception("StateMachine must be child of IEntity");
        StaminaComponent staminaComponent = GetNode<StaminaComponent>("../StaminaComponent");
        GD.Print($"StamionaComponent is null: ", staminaComponent == null);
        foreach (var child in GetChildren())
        {
            if (child is CombatState state)
            {
                state.Finished += OnStateFinished;
                state.Initialize(entity, staminaComponent);
            }
        }

        CurrentState = InitialState ?? GetChild(0) as CombatState;
        CurrentState.Enter();

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

    private void OnStateFinished(string nextStatePath)
    {
        var nextState = GetNodeOrNull<CombatState>(nextStatePath);
        GD.Print($"State finished:", CurrentState.GetType());
        if (nextState == null)
        {
            GD.PrintErr($"State not found: {nextStatePath}");
            GD.Print("Current node:", GetPath());
            GD.Print("Looking for:", nextStatePath);
            return;
        }

        if (!nextState.CanEnter())
            return;

        CurrentState?.Exit();
        CurrentState = nextState;
        CurrentState.Enter();
    }

}
