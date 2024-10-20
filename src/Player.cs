using Godot;
using System;

public partial class Player : RigidBody2D
{
	[Export] float TorqueForce = 100f;
	[Export] float ThrusterForce = 100f;
	[Export] float MaxFuel = 60f;
	[ExportGroup("Components")]
	[Export] GpuParticles2D ThrustParticles;
	[Export] PackedScene ExplosionTemplate;
	public int EggRescued;
	public int EggDelivered;
	public bool IsInGoal;
	private float DeliverCooldown;
	private float FuelUsed = 0f;
	public float FuelPercent {
		get {
			return 1f - (FuelUsed / MaxFuel);
		}
	}
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
    }
	private void OnBodyEntered(Node body) {
		if (body is StaticBody2D) {
			float speedMag = LinearVelocity.Length();
			if (speedMag > 100) {
				var explosion = ExplosionTemplate.Instantiate<Node2D>();
				GetParent().AddChild(explosion);
				explosion.GlobalPosition = GlobalPosition;
			}
		}
	}
	private void OnBodyExited(Node body) {
		//
	}
    public void Reset() {
		EggRescued = 0;
		EggDelivered = 0;
		FuelUsed = 0f;
	}
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        ProcessEggDeliver(delta);
        var torque = Input.GetAxis("ui_left", "ui_right");
        var thrust = Input.IsActionPressed("ui_down");
        if (torque != 0) ApplyTorque(torque * TorqueForce);
        if (thrust && FuelUsed<MaxFuel)
        {
            var force = Vector2.Up.Rotated(this.Rotation) * ThrusterForce;
            ApplyForce(force);
			FuelUsed += (float)delta;
			ThrustParticles.Emitting = true;
        }
		else {
			ThrustParticles.Emitting = false;
		}
    }
    private void ProcessEggDeliver(double delta)
    {
		if (IsInGoal) {
			FuelUsed -= (float)delta;
			if (FuelUsed < 0) FuelUsed = 0;
		}
        if (DeliverCooldown <= 0)
        {
            if (IsInGoal && EggRescued > 0)
            {
                DeliverCooldown = 1f;
                EggRescued -= 1;
                EggDelivered += 1;
            }
        }
        else
        {
            DeliverCooldown -= (float)delta;
        }
    }
}
