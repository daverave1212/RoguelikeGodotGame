using Godot;
using System;

public class DumbBanditCrossbowWeapon : Weapon
{
	public override void Shoot(Vector2 fromPos, Vector2 toPos)
	{
		float angle = Utils.Angle(fromPos, toPos);
		angle += Utils.Random(-30, 30);
		
		ShootBulletAtAngle("EnemyBanditArrow", fromPos, angle, (unitHit) =>
		{
			unitHit.ReceiveHit(3, MyUnit);
		});
	}
}
