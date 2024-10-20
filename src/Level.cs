using Godot;
using System;
using System.Collections.Generic;

public partial class Level : Node2D
{
	[ExportGroup("Level Settings")]
	[Export] int Seed;
	[Export] Vector2 Size;
	[Export] public int TotalEggs = 8;
	[ExportGroup("Components")]
	[Export] TerrainGenerator Terrain;
    [Export] PlayerGoal Goal;
	[Export] Player Player;
	[Export] Camera2D Camera;
	[Export] Minimap Minimap;
	[Export] PackedScene EggTemplate;
	//
	private List<Node2D> objectInstances = new List<Node2D>();
    public override void _Ready()
    {
        base._Ready();
		Generate();
    }
    public async void Generate(bool randomize = true) {
		Player.Reset();
		Minimap.Setup(Size);
		if (randomize) Seed = (int)Random.Shared.NextInt64();
		Camera.LimitRight = (int)Size.X;
		Camera.LimitBottom = (int)Size.Y;
		Terrain.Size = Size;
		Terrain.Seed = Seed;
		Terrain.GenerateTerrain();
		// Reposition player.
		float playerPosition = (float)GD.RandRange(16f, Size.X-16f);
		Player.GlobalPosition = new Vector2(playerPosition, 16f);
		// Wait for next frame
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		// Position elements that require raycasts.
		PlaceElements();
	}

	private async void PlaceElements() {
		// Clear all objects.
		foreach (var o in objectInstances) o.QueueFree();
		objectInstances.Clear();
		// Reposition goal.
		float goalPosition = (float)GD.RandRange(48f, Size.X-48f);
		Goal.Place(goalPosition, Size.Y);
		Terrain.Flatten(goalPosition, Goal.Size.X, 0.8f);
		// Put eggs.
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		for (int i = 0; i < TotalEggs; i++) {
			float horzPos = (float)GD.RandRange(16f, Size.X-16f);
			Vector2 location = Goal.LookForPlace(horzPos, Size.Y);
			Node2D newEgg = EggTemplate.Instantiate<Node2D>();
			AddChild(newEgg);
			newEgg.GlobalPosition = location;
			objectInstances.Add(newEgg);
		}
	}
}
