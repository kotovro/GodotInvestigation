using Godot;

public interface IEntity
{
    Vector3 Velocity { get; set; }
    bool IsOnFloor { get; }
    Node AsNode();
}