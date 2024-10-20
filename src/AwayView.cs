using Godot;
using System;

public partial class AwayView : Control
{
	[Export] SubViewport Viewport;
	[Export] Camera2D Camera;
	[Export] Camera2D WorldCamera;
	[Export] Player Player;
    public override void _Ready()
    {
        Viewport.World2D = GetViewport().World2D;
    }
    public override void _Process(double delta)
    {
        Camera.GlobalPosition = Player.GlobalPosition;
		Vector2 relativePosition = Player.GlobalPosition - WorldCamera.GetScreenCenterPosition();
		var size = GetViewportRect().Size;
		Visible = Math.Abs(relativePosition.X) > size.X/2 || Math.Abs(relativePosition.Y) > size.Y/2;
		var screenPosition = relativePosition + (size/2);
		float xClamped = Math.Clamp(screenPosition.X, 16, size.X - Viewport.Size.X);
		float yClamped = Math.Clamp(screenPosition.Y, 16, size.Y - Viewport.Size.Y);
		GlobalPosition = new Vector2(xClamped, yClamped);
    }
}
