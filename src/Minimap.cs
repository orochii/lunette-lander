using Godot;
using System;

public partial class Minimap : Control
{
	[Export] SubViewport MinimapViewport;
	[Export] Camera2D MinimapCamera;
	public void Setup(Vector2 size) {
		MinimapViewport.World2D = GetViewport().World2D;
		// Set up camera.
		MinimapCamera.GlobalPosition = size / 2;
		float zoomX = size.X / MinimapViewport.Size.X;
		float zoomY = size.Y / MinimapViewport.Size.Y;
		float zoom = Math.Max(zoomX, zoomY);
		MinimapCamera.Zoom = Vector2.One / zoom;
	}
}
