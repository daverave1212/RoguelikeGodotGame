using Godot;
using System;

public class BroadswordWeapon : Weapon
{
	
	public override void Shoot(Vector2 fromPos, Vector2 toPos)
	{
		GD.Print($"Shooting from {fromPos} to {toPos}");
		SpawnBullet("Bullet", fromPos, toPos, delegate (Unit unitHit)
		{
			GD.Print("BAM!");
		});
	}
}
