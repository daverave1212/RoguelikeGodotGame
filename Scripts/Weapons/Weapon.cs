using Godot;
using System;

/// <summary>
/// Inherit this to make a new type of <see cref="Weapon"/>.
/// </summary>
public abstract class Weapon : Sprite
{
	public abstract void Shoot(Vector2 fromPos, Vector2 toPos);

	/// <summary>
	/// Use this to spawn a standard Bullet that moves on its own.
	/// bulletOwner should be OwnerTag.Player or OwnerTag.Enemy (or even OwnerTag.Nobody).
	/// If you give it the Player tag, it will only seek Enemies (and viceversa).
	/// </summary>
	public Bullet SpawnBullet(string bulletName, Vector2 atPos, Vector2 direction, Action<Unit> onHit)
	{
		return SpawnBullet(bulletName, atPos, Utils.ToAngle(direction.Normalized()), onHit);
	}
	/// <summary> Variant parameters for the same function </summary>
	public Bullet SpawnBullet(string bulletName, Vector2 atPos, float angleDegrees, Action<Unit> onHit)
	{		
		var bullet = BulletsDatabase.Create(bulletName, atPos, angleDegrees, onHit);
		GetNode<Node>(World.WorldNodePath).AddChild(bullet);
		return bullet;
	}
}
