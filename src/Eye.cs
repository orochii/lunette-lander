using Godot;
using System;

public partial class Eye : Control
{
	[Export] public Vector2 TargetPosition;
	[Export] public Control EyeObject;
	public override void _Process(double delta)
	{
		// Get the target local position by changing the global. Let engine do it.
		EyeObject.GlobalPosition = TargetPosition;
		// Clamp to container's size.
		float x = Math.Clamp(EyeObject.Position.X, 0, Size.X-EyeObject.Size.X);
		float y = Math.Clamp(EyeObject.Position.Y, 0, Size.Y-EyeObject.Size.Y);
		EyeObject.Position = new Vector2(x,y);
	}
}
