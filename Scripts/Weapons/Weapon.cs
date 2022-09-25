using Godot;

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
	public Bullet SpawnBullet(OwnerTag bulletOwner, Vector2 atPos, Vector2 direction, float bulletSpeed = -1)
	{
		return SpawnBullet(bulletOwner, atPos, Utils.ToAngle(direction.Normalized()), bulletSpeed);
	}
	/// <summary> Variant parameters for the same function </summary>
	public Bullet SpawnBullet(OwnerTag bulletOwner, Vector2 atPos, float angleDegrees, float bulletSpeed = 1.2f)
	{
		var sceneNode = GetNode<Node>("/root");
		var bulletPath = "res://Prefabs/Weapons/Bullets/Bullet.tscn";
		var bullet = (Bullet) Utils.SpawnNodeOn(sceneNode, bulletPath);
		bullet.SetupAfterSpawn(bulletOwner, position: atPos, angleDegrees, bulletSpeed);
		return bullet;
	}
}
