using Godot;
using System;

public partial class Main : Node2D
{
	public static Main Instance;
	[Export] public UIManager UI;
	[Export] public Level Level;
	public static Player Player {
		get {
			if (Instance == null || Instance.Level == null) return null;
			return Instance.Level.Player;
		}
	}
    public override void _Ready()
    {
        Instance = this;
		UI.SetMode(0);
    }
	public void StartGame() {
		Level.Generate();
		UI.SetMode(1);
	}
	public void ExitGame() {
		GetTree().Quit();
	}
}
