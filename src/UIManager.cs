using Godot;
using System;
using System.Reflection;

public partial class UIManager : Control
{
	public void SetMode(int idx) {
		var children = GetChildren();
		for (int i = 0; i < children.Count; i++) {
			if (children[i] is Control) {
				var c = children[i] as Control;
				c.Visible = (i == idx);
				var type = c.GetType();
				var m = type.GetMethod("Refresh");
				if (m != null) m.Invoke(c, null);
			}
		}
	}
}
