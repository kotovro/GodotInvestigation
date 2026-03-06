using Godot;
using System;

public partial class StaminaComponent : Node
{
	[Export] public float MaxStamina = 100f;

	public float CurrentStamina { get; private set; }

	public override void _Ready()
	{
		CurrentStamina = MaxStamina;
	}

	public void Update(float delta, MovementState movementState)
	{
		if (movementState == null)
			return;

		if (movementState.StaminaConsumptionPerSecond > 0)
		{
			Consume(movementState.StaminaConsumptionPerSecond * delta);
		}
		else
		{
			Regenerate(movementState.StaminaRegenPerSecond * delta);
		}
	}

	public bool TryConsume(float amount)
	{
		if (CurrentStamina < amount)
			return false;

		CurrentStamina -= amount;
		return true;
	}

	public void Consume(float amount)
	{
		CurrentStamina = Mathf.Max(CurrentStamina - amount, 0);
	}

	public void Regenerate(float amount)
	{
		CurrentStamina = Mathf.Min(CurrentStamina + amount, MaxStamina);
	}

	public void Restore(float amount)
	{
		CurrentStamina = Mathf.Min(CurrentStamina + amount, MaxStamina);
	}

	// Instant full refill
	public void RestoreFull()
	{
		CurrentStamina = MaxStamina;
	}

	public float Normalized()
	{
		return CurrentStamina / MaxStamina;
	}
}
