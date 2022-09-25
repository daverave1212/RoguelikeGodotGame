using Godot;
using System;

public class BroadswordWeapon : Weapon
{
	
	public override void Shoot(Vector2 fromPos, Vector2 toPos)
	{
		Unit unitOn = GetParent<Unit>();
		SpawnBullet(unitOn.GetOwnerTag(), fromPos, toPos);
	}
}
