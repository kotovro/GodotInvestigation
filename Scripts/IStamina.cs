using Godot;
using System;

public interface IStamina
{
	float CurrentStamina { get; }
	float MaxStamina { get; }
	bool CanConsume(float cost);
	void Regenerate(float amount);
}
