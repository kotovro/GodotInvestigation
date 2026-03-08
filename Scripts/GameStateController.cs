using Godot;
using System;

public partial class GameStateController : Node
{
	[Export] public PackedScene EnemySpawnerScene;
	private EnemySpawner _spawner;
	public override void _Ready()
	{
		SpawnEnemySpawner();
	}

	private void SpawnEnemySpawner()
	{
		if (EnemySpawnerScene == null)
		{
			GD.PrintErr("EnemySpawnerScene is not assigned in inspector!");
			return;
		}

		_spawner = EnemySpawnerScene.Instantiate<EnemySpawner>();

		AddChild(_spawner);
	}

	public override void _Process(double delta)
	{
	}
}
