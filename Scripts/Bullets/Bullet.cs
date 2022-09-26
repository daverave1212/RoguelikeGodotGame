using Godot;

/// <summary>
/// Spawned by Weapon with some custom functions.
/// Speed has [Export] just for testing.
/// The weapon will set the Bullet's angle and speed.
/// </summary>
public class Bullet : Area2D
{
	/// <summary> Who created the bullet </summary>
	public OwnerTag BulletOwner = OwnerTag.Nobody;
	[Export] public float Speed = 100f;

	public override void _Ready()
	{
		GD.Print(RotationDegrees);
	}

	public override void _PhysicsProcess(float deltaTime)
	{
		Position = Position.MoveAtAngle(RotationDegrees, Speed, deltaTime);
	}

	public void SetupAfterSpawn(OwnerTag bulletOwner, Vector2 position, float angleDegrees, float speed)
	{
		BulletOwner = bulletOwner;
		GlobalPosition = position;
		RotationDegrees = angleDegrees;
		Speed = speed;
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

		QueueFree();
	}

}

