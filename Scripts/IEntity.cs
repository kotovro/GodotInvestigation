// IEntity.cs
using Godot;

public interface IEntity
{
    Vector3 Velocity { get; set; }
    bool IsTouchingFloor { get; }
    bool CanJump { get; }
    void PlayAnimation(string name);
    Node AsNode();
}