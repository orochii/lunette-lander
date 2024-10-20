using Godot;
using System;

public partial class Player : RigidBody2D
{
	[Export] float TorqueForce = 100f;
	[Export] float ThrusterForce = 100f;
	[ExportGroup("Components")]
	[Export] GpuParticles2D ThrustParticles;
	[Export] PackedScene ExplosionTemplate;
	public int EggRescued;
	public int EggDelivered;
	public bool IsDelivering;
	private float DeliverCooldown;
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
	}
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        ProcessEggDeliver(delta);
        var torque = Input.GetAxis("ui_left", "ui_right");
        var thrust = Input.IsActionPressed("ui_down");
        if (torque != 0) ApplyTorque(torque * TorqueForce);
        if (thrust)
        {
            var force = Vector2.Up.Rotated(this.Rotation) * ThrusterForce;
            ApplyForce(force);
        }
        ThrustParticles.Emitting = thrust;
    }
    private void ProcessEggDeliver(double delta)
    {
        if (DeliverCooldown <= 0)
        {
            if (IsDelivering && EggRescued > 0)
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
