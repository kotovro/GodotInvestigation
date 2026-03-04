// IEntity.cs
using Godot;

public interface IEntity
{
    Vector3 Velocity { get; set; }
    bool IsOnFloor { get; }
    bool CanJump { get; }           // True if on floor OR in coyote time
    bool ConsumeJumpBuffer();       // Returns true if buffered input exists
    void ResetJumpBuffer();         // Clear buffer on state exit

    void PlayAnimation(string name);
    Node AsNode();
}