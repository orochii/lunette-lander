using Godot;
using System;

public partial class GameMenu : Control
{
	[Export] Control FirstFocus;
	public void Refresh() {
		GetTree().Paused = Visible;
		if (Visible) {
			//
			FirstFocus.GrabFocus();
		}
	}
}
