using Godot;
using System;

public partial class Enemy : RigidBody2D
{
	[Export] Eye[] Eyes;
	public override void _Process(double delta)
	{
		foreach (var eye in Eyes) {
			eye.TargetPosition = Main.Player.GlobalPosition;
		}
	}
}
