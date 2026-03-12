using Godot;
using System;

public partial class GameStateController : Node
{
	[Export] public PackedScene EnemySpawnerScene;
	[Export] public PackedScene PlayerSpawnerScene;
	private EnemySpawner _enemySpawner;
	private PlayerSpawner _playerSpawner;
	private CanvasLayer _uiLayer;

	public override void _Ready()
	{
		AddPlayerSpawner();
		AddEnemySpawner();
	}
	
	private void AddPlayerSpawner()
	{
		if (PlayerSpawnerScene == null)
		{
			GD.PrintErr("Player spawner is not assigned in inspector!");
			return;
		}

		_playerSpawner = PlayerSpawnerScene.Instantiate<PlayerSpawner>();
		AddChild(_playerSpawner);
		//_uiLayer.AddChild();
		GD.Print("Yo, we added player spawner");
	}

	private void AddEnemySpawner()
	{
		if (EnemySpawnerScene == null)
		{
			GD.PrintErr("EnemySpawnerScene is not assigned in inspector!");
			return;
		}

		_enemySpawner = EnemySpawnerScene.Instantiate<EnemySpawner>();
		AddChild(_enemySpawner);
	}
	
	public override void _Process(double delta)
	{
	}
}
