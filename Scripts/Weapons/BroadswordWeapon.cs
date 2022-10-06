using Godot;

public class BroadswordWeapon : Weapon
{

	public override void Shoot(Vector2 fromPos, Vector2 toPos)
	{
		ShootBulletTowardPosition("Bullet", fromPos, toPos, (unitHit) =>
		{
			unitHit.ReceiveHit(MyUnit);
		});
	}
}
