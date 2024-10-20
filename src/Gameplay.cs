using Godot;
using System;

public partial class Gameplay : Control
{
	public override void _Process(double delta)
	{
		if (!IsVisibleInTree()) return;
		if (Input.IsActionJustPressed("ui_cancel")) {
			Main.Instance.UI.SetMode(0);
		}
	}
}
