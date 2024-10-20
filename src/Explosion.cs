using Godot;
using System;

public partial class Explosion : Node2D
{
	[Export] GpuParticles2D[] Particles;
	[Export] Area2D EffectArea;
	[Export] float Life;
	public override void _Ready()
	{
		foreach (var p in Particles) p.Emitting = true;
		EffectArea.BodyEntered += OnBodyEntered;
	}
	public void OnBodyEntered(Node body) {
		if (body is Enemy) {
			var enemy = body as Enemy;
			enemy.Damage(1);
		}
	}
	public override void _Process(double delta)
	{
		Life -= (float)delta;
		if (Life < 0) QueueFree();
	}
}
