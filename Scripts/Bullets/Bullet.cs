using Godot;
using System;

/// <summary>
/// Spawned by BulletsDatabase. Do not create bullets in a different way!
/// </summary>
public class Bullet : Area2D
{
	
	/// <summary> Who created the bullet </summary>
	[Export] public OwnerTag BulletOwner = OwnerTag.Nobody;
	[Export] public float Speed = 100f;
	
	public Action<Unit> OnHit;
	
	public override void _Ready()
	{
		GD.Print(RotationDegrees);
	}

	public override void _PhysicsProcess(float deltaTime)
	{
		Position = Position.MoveAtAngle(RotationDegrees, Speed, deltaTime);
	}
	void _OnBulletBodyEntered(Node nodeItCollidedWith)
	{
		var hasCollidedWithTerrain = nodeItCollidedWith.GetType() != typeof(Unit);

		if(hasCollidedWithTerrain)
		{
			GD.Print("WITH TERRAIN");
			QueueFree();
			return;
		}

		var unit = (Unit)nodeItCollidedWith;
		GD.Print($"WITH UNIT; owner == {unit.GetOwnerTag()}");
		if(unit.GetOwnerTag() == BulletOwner)
			return;
	
		if (OnHit != null)
			OnHit(unit);
		QueueFree();
	}

}

