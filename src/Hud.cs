using Godot;
using System;

public partial class Hud : MarginContainer
{
	[Export] Label EggLabel;
	[Export] Control FuelBack;
	[Export] Control FuelBar;
	[Export] Player Player;
	[Export] Level Level;
    public override void _Process(double delta)
    {
		var delivered = Player.EggDelivered;
		var rescued = Player.EggRescued;
		var left = Level.TotalEggs - delivered - rescued;
        EggLabel.Text = string.Format("{0} > {1} > {2}", left, rescued, delivered);
		//
		var size = new Vector2(FuelBack.Size.X * Player.FuelPercent, FuelBack.Size.Y);
		FuelBar.Size = size;
    }
}
