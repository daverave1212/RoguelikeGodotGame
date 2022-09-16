using Godot;
using System;

public class Unit : KinematicBody2D
{
	StatsComponent MyStats;
	
	public override void _Ready() {
		MyStats = GetNode<StatsComponent>("StatsComponent");
	}
	
	
	
	
	
	
	// --------- API ----------
	
	/// <summary>
	/// Moves 1 Frame in the given direction.
	/// Vector2 direction should only have -1, 0 and 1 as values.
	/// Moves using own speed from StatsComponent.
	/// This should be used inside a _Process function.
	/// </summary>
	public void MoveInDirection(Vector2 direction) {
		MoveAndSlide(direction.Normalized() * MyStats.Speed);
	}
	
	/// <summary>
	/// Returns the closest Unit within range of me.
	/// This should be used inside a _Process function.
	/// </summary>
	public Unit AcquireTarget(float range) {
		TargetAcquirer targetAcquirer = GetNode<TargetAcquirer>("TargetAcquirer");
		return (Unit)targetAcquirer.AcquireTarget(range, this);
	}
	
	public void ShootWeaponAt(Unit targetToShootAt) {
		var weaponEquipped = Utils.GetNodeByType<Weapon>(this);
		if (weaponEquipped == null)
		{
			GD.Print($"Unit ${Name} has no weapon equipped. Add as child node a Sprite with a T: Weapon script.");
			return;
		}
		weaponEquipped.Shoot(new Vector2(0, 0), new Vector2(0, 0));
	}
}
