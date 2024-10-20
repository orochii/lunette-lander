using Godot;
using System;
using System.Security.Cryptography;

public partial class PlayerGoal : StaticBody2D
{
	[Export] Area2D GoalArea;
	[Export] CollisionShape2D Collider;
	[Export] CollisionShape2D GoalCollider;
	[Export] NinePatchRect Visuals;
	public Vector2 Size => Visuals.CustomMinimumSize;
    public override void _Ready()
    {
        GoalArea.BodyEntered += OnBodyEntered;
		GoalArea.BodyExited += OnBodyExited;
    }
	private void OnBodyEntered(Node2D body) {
		if (body is Player) {
			var player = body as Player;
			player.IsInGoal = true;
		}
	}
	private void OnBodyExited(Node2D body) {
		if (body is Player) {
			var player = body as Player;
			player.IsInGoal = false;
		}
	}
    public void Resize(float w, float h) {
		var r = new RectangleShape2D();
		var r2 = new RectangleShape2D();
		r.Size = new Vector2(w,h);
		r2.Size = new Vector2(w,1);
		Collider.Shape = r;
		GoalCollider.Shape = r2;
		Visuals.CustomMinimumSize = new Vector2(w,h);
	}
	public void Place(float x, float height) {
		GlobalPosition = LookForPlace(x, height);
	}
	public Vector2 LookForPlace(float x, float height) {
		var spaceState = GetWorld2D().DirectSpaceState;
    	var origin = new Vector2(x, 0);
		var target = new Vector2(x, height);
    	var query = PhysicsRayQueryParameters2D.Create(origin, target);
    	var result = spaceState.IntersectRay(query);
		GD.Print(result);
		if (result.Count > 0) {
			return result["position"].AsVector2();
		}
		else {
			return new Vector2(x, height);
		}
	}
}
