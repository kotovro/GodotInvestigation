using Godot;
using System;

public partial class StaminaComponent : Node
{

	[Signal] public delegate void StaminaConsumedEventHandler(float consumedStamina);
    [Signal] public delegate void StaminaRegenedEventHandler(float consumedStamina);
    
	[Export] public float MaxStamina = 100f;

    public float CurrentStamina { get; private set; }

	public override void _Ready()
	{
		CurrentStamina = MaxStamina;
	}

	public void Update(float delta)
	{
		//if (movementState == null)
		//	return;

		//if (movementState.StaminaConsumptionPerSecond > 0)
		//{
		//	Consume(movementState.StaminaConsumptionPerSecond * delta);
		//}
		//else
		//{
		//	Regenerate(movementState.StaminaRegenPerSecond * delta);
		//}
	}

    public bool CanConsume(float cost) => CurrentStamina >= cost;

	public void Consume(float amount)
	{
		CurrentStamina = Mathf.Max(CurrentStamina - amount, 0);
	}

	public void Regenerate(float amount)
	{
		CurrentStamina = Mathf.Min(CurrentStamina + amount, MaxStamina);
	}

	public void RegenerateFull()
	{
		CurrentStamina = MaxStamina;
	}

	public float Normalized()
	{
		return CurrentStamina / MaxStamina;
	}
}
