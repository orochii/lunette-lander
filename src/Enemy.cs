using Godot;
using System;

public partial class Enemy : RigidBody2D
{
	[Export] Eye[] Eyes;
	[Export] int Life = 1;
	public override void _Process(double delta)
	{
		foreach (var eye in Eyes) {
			eye.TargetPosition = Main.Player.GlobalPosition;
		}
	}
	public void Damage(int v) {
		Life -= v;
		if (Life <= 0) Die();
	}
	public void Die() {
		Visible = false;
		Freeze = true;
		CollisionLayer = 0;
		CollisionMask = 0;
	}
}
