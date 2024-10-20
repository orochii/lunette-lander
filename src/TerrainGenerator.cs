using Godot;
using System;

public partial class TerrainGenerator : StaticBody2D
{
	const int CELL_W = 8;
	[ExportGroup("Settings")]
	[Export] public Vector2 Size = new Vector2(160,144);
	[Export] public int Seed = 0;
	[Export] float BaseHeight = 8f;
	[Export] float HeightVariation = 0.6f;
	[Export] float SampleScale = 1f;
	[Export] FastNoiseLite Noise;
	[ExportGroup("Components")]
	[Export] Polygon2D polygon;
	[Export] CollisionPolygon2D collider;
	public void GenerateTerrain() {
		//
		Noise.Seed = Seed;
		//
		int rows = ((int)Size.X / CELL_W) + 1;
		float height = HeightVariation * (Size.Y - BaseHeight);
		Vector2[] points = new Vector2[rows+2];
		for (int idx = 0; idx < rows; idx++) {
			float x = idx * CELL_W;
			float v = Noise.GetNoise1D(x * SampleScale) + 1;
			float h = (v * height);
			float y = Size.Y - BaseHeight - h;
			points[idx] = new Vector2(x,y);
		}
		points[rows] = new Vector2(Size.X, Size.Y+32);
		points[rows+1] = new Vector2(0, Size.Y+32);
		polygon.Polygon = points;
		// Replicate polygon in collider
		collider.Polygon = points;
	}

	public void Flatten(float x, float width, float factor) {
		var points = polygon.Polygon;
		int rows = points.Length-2;
		// Sample
		int cIdx = (int)(x / CELL_W);
		cIdx = Math.Clamp(cIdx, 0, rows-1);
		float sample = points[cIdx].Y;
		// Apply flatten
		int d = (int)(width / 2 / CELL_W);
		int sIdx = cIdx - d;
		int eIdx = cIdx + d;
		for (int idx = sIdx; idx <= eIdx; idx++) {
			if (idx >= 0 && idx < rows) {
				float v = points[idx].Y;
				float nY = Mathf.Lerp(v, sample, factor);
				float oX = points[idx].X;
				points[idx] = new Vector2(oX, nY);
			}
		}
		// Update collision
		polygon.Polygon = points;
		collider.Polygon = points;
	}
}
