using Godot;
using System;

public class Weapon : Sprite
{
	/// <summary>
	/// Always extend this. Never leave this non-overridden.
	/// Never use this as a script attached to a weapon.
	/// Always make another script that extends this one.
	/// Make another weapon by extending this Weapon class.
	/// </summary>
	public virtual void Shoot(Vector2 fromPos, Vector2 toPos) {
		GD.Print($"WARNING: Weapon {this.Name} does not override the Shoot method!");
		GD.Print("Create a new script that extends Weapon with a custom Shoot method.");
		GD.Print("Then assign it to this Weapon (which should be a Sprite).");
	}

}
