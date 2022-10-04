using Godot;

public class Unit : KinematicBody2D
{
	StatsComponent MyStats;
	Vector2 PreviousFramePosition;
	Vector2 CurrentFramePosition;

	public override void _Ready()
	{
		MyStats = GetNode<StatsComponent>("StatsComponent");
		CurrentFramePosition = GlobalPosition;
		PreviousFramePosition = GlobalPosition;
	}
	public override void _PhysicsProcess(float deltaTime)
	{
		PreviousFramePosition = CurrentFramePosition;
		CurrentFramePosition = GlobalPosition;
	}






	// --------- API ----------
	public OwnerTag GetOwnerTag()
	{
		if(IsPlayerUnit()) return OwnerTag.Player;
		return OwnerTag.Enemy;
	}
	public bool IsPlayerUnit()
	{
		return HasNode("UnitPlayerController");
	}
	public bool IsEnemyUnit()
	{
		return HasNode("UnitAIController");
	}

	/// <summary>
	/// Moves 1 Frame in the given direction.
	/// Vector2 direction should only have -1, 0 and 1 as values.
	/// Moves using own speed from StatsComponent.
	/// This should be used inside a _Process function.
	/// </summary>
	public void MoveInDirection(Vector2 direction)
	{
		MoveAndSlide(direction.Normalized() * MyStats.Speed);
	}
	public bool IsNotMoving()
	{
		return (PreviousFramePosition.x == CurrentFramePosition.x && PreviousFramePosition.y == CurrentFramePosition.y);
	}
	public bool IsMoving()
	{
		return !IsNotMoving();
	}
	/// <summary>
	/// Returns the closest Unit within range of me.
	/// This should be used inside a _Process function.
	/// </summary>
	public Unit AcquireTarget(float range)
	{
		TargetAcquirer targetAcquirer = GetNode<TargetAcquirer>("TargetAcquirer");
		return (Unit)targetAcquirer.AcquireTarget(range, this);
	}
	public void ShootWeaponAt(Unit targetToShootAt)
	{
		var weaponEquipped = Utils.GetNodeByType<Weapon>(this);
		if(targetToShootAt == null)
		{
			GD.Print($"Unit {Name} can't shoot a null targetToShootAt, silly!");
			return;
		}
		if(weaponEquipped == null)
		{
			GD.Print($"Unit {Name} has no weapon equipped. Add as child node a Sprite with a T: Weapon script.");
			return;
		}
		weaponEquipped.Shoot(GlobalPosition, targetToShootAt.GlobalPosition);
	}
	public Weapon GetEquippedWeapon()
	{
		return Utils.GetNodeByType<Weapon>(this);
	}
}
