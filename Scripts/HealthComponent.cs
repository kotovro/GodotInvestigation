using Godot;

public partial class HealthComponent : Node
{
	[Signal]
	public delegate void HealthChangedEventHandler(float current, float max);

	[Signal]
	public delegate void DiedEventHandler();

	[Export] public float MaxHealth { get; set; } = 100f;
	[Export] public float KnockbackForce { get; set; } = 10f;

	private float _currentHealth;
	private IDamageable _owner;

	public float CurrentHealth => _currentHealth;
	public bool IsAlive => _currentHealth > 0;

	public override void _Ready()
	{
		_currentHealth = MaxHealth;
		_owner = GetParent() as IDamageable;

		if (_owner == null)
		{
			GD.PrintErr($"[HealthComponent] Parent must implement IDamageable!");
		}
	}

	public void TakeDamage(float amount, Vector3 hitDirection, Node damageSource)
	{
		if (!IsAlive) return;

		_currentHealth = Mathf.Max(0, _currentHealth - amount);

		// Emit signal for UI, effects, etc.
		EmitSignal(SignalName.HealthChanged, _currentHealth, MaxHealth);

		// Apply knockback if entity has Velocity
		if (GetParent() is CharacterBody3D body)
		{
			var velocityProp = body.GetType().GetProperty("Velocity");
			if (velocityProp != null)
			{
				var currentVel = (Vector3)velocityProp.GetValue(body);
				var knockback = hitDirection.Normalized() * KnockbackForce;
				velocityProp.SetValue(body, currentVel + knockback);
			}
		}

		// Play hit effect (flash, sound, etc.)
		PlayHitEffect();

		if (_currentHealth <= 0)
		{
			Die();
		}
	}

	public void Heal(float amount)
	{
		_currentHealth = Mathf.Min(MaxHealth, _currentHealth + amount);
		EmitSignal(SignalName.HealthChanged, _currentHealth, MaxHealth);
	}

	private void Die()
	{
		EmitSignal(SignalName.Died);

		if (_owner != null)
			_owner.Die();
	}

	private void PlayHitEffect()
	{
		// Optional: Flash sprite, play sound, spawn particles
		// Example: GetParent().GetNode<AnimationPlayer>("AnimationPlayer").Play("hit");
		GD.Print($"[Health] Hit! {_currentHealth}/{MaxHealth} HP");
	}
}
