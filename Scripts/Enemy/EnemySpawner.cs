using Godot;
using System;

public partial class EnemySpawner : Node3D
{
	[Export] PackedScene EnemyScene;

	public override void _Ready()
	{
		Spawn(new Vector3(0, 10, 0));
	}

	private void Spawn(Vector3 position)
	{
		var enemy = EnemyScene.Instantiate<Node3D>();
		enemy.Position = position;
		AddChild(enemy);
	}
}
