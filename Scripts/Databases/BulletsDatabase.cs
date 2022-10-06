using System;

using Godot;

public class BulletsDatabase
{
	public const string BulletsPrefabsPath = "res://Prefabs/Weapons/Bullets";

	public static Bullet Create(string bulletName, Tag bulletOwner, Vector2 position, float angle, Action<Unit> onHit)
	{
		// Normalize the path so that it accepts any type of bulletName
		var bulletPath =
			BulletsPrefabsPath +
			(bulletName.StartsWith("/") ? "" : "/") +
			bulletName +
			(bulletName.EndsWith(".tscn") ? "" : ".tscn");

		var bullet = (Bullet)Utils.SpawnNode(bulletPath);

		bullet.GlobalPosition = position;
		bullet.RotationDegrees = angle;
		bullet.OnHit = onHit;
		bullet.BulletOwner = bulletOwner;

		return bullet;
	}
}
