using System;

using Godot;

/// <summary>
/// Inherit this to make a new type of <see cref="Weapon"/>.
/// </summary>
public abstract class Weapon : Sprite
{
	protected Unit MyUnit;

	[Export] public float AttackCooldownSeconds = 1f;
	[Export] public float Damage = 1f;
	[Export] public float AttackRange = 75f;

	public override void _Ready()
	{
		MyUnit = GetParent<Unit>();
	}

	public abstract void Shoot(Vector2 fromPos, Vector2 toPos);

	public Bullet ShootBulletTowardPosition(string name, Vector2 atPos, Vector2 toPos, Action<Unit> onHit)
	{
		return ShootBulletAtAngle(name, atPos, Utils.Angle(atPos, toPos), onHit);
	}
	public Bullet ShootBulletAtAngle(string name, Vector2 atPos, float angleDegrees, Action<Unit> onHit)
	{
		var bullet = BulletsDatabase.Create(name, MyUnit.Tag, atPos, angleDegrees, onHit);
		GetNode<Node>(World.WorldNodePath).AddChild(bullet);
		return bullet;
	}
}
