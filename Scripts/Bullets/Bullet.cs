using System;

using Godot;

/// <summary>
/// Spawned by BulletsDatabase. Do not create bullets in a different way!
/// </summary>
public class Bullet : Area2D
{
	/// <summary> Who created the bullet </summary>
	[Export] public Tag BulletOwner = Tag.Nobody;
	[Export] public float Speed = 100f;

	public Action<Unit> OnHit;

	public override void _PhysicsProcess(float deltaTime)
	{
		Position = Position.MoveAtAngle(RotationDegrees, Speed, deltaTime);
	}
	void _OnBulletBodyEntered(Node nodeItCollidedWith)
	{
		var hasCollidedWithTerrain = nodeItCollidedWith.GetType() != typeof(StaticBody2D);

		if(hasCollidedWithTerrain)
		{
			QueueFree();
			return;
		}

		var unit = nodeItCollidedWith.GetParent<Unit>();
		if(unit.Tag == BulletOwner)
			return;

		OnHit?.Invoke(unit);
		QueueFree();
	}
}

