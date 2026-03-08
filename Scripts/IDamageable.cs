// IDamageable.cs
using Godot;

public interface IDamageable
{
    float CurrentHealth { get; }
    float MaxHealth { get; }
    bool IsAlive { get; }

    void TakeDamage(float amount, Vector3 hitDirection, Node damageSource);
    void Heal(float amount);
    void Die();
}