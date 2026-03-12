using Godot;

public partial class PlayerSpawner : Node3D
{
	[Export] PackedScene PlayerScene;

	public override void _Ready()
	{
		Spawn(new Vector3(0, 0, 0));
		//Spawn(new Vector3(0, 9, 0));
	}

	private void Spawn(Vector3 position)
	{
		var player = PlayerScene.Instantiate<PlayerController>();
		player.Position = position;
		AddChild(player);
	}
}
