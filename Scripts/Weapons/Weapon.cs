using Godot;

/// <summary>
/// Inherit this to make a new type of <see cref="Weapon"/>.
/// </summary>
public abstract class Weapon : Sprite
{
	public abstract void Shoot(Vector2 fromPos, Vector2 toPos);
}
