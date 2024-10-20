using Godot;
using System;

public partial class Egg : Area2D
{
	//
	public override void _Ready()
	{
		this.BodyEntered += OnBodyEntered;
	}
	private void OnBodyEntered(Node2D body) {
		if (!Visible) return;
		if (body is Player) {
			var player = body as Player;
			player.EggRescued += 1;
			Visible = false;
		}
	}
}
