using Godot;

public class Unit : KinematicBody2D
{
	const float SHOOT_Y_OFFSET = -8f;

	private UnitStats myCurrentStats, myBaseStats;
	private Vector2 previousFramePosition, currentFramePosition;
	private TextureProgress healthBar;

	public bool IsDead => myCurrentStats.Health == 0;
	public bool IsMoving => previousFramePosition.x != currentFramePosition.x && previousFramePosition.y != currentFramePosition.y;
	public bool IsPlayer => HasNode("UnitPlayerController");
	public bool IsEnemy => HasNode("UnitAIController");
	public Tag Tag => IsPlayer ? Tag.Player : Tag.Enemy;
	public Weapon EquippedWeapon => Utils.GetNodeByType<Weapon>(this);

	public override void _Ready()
	{
		myCurrentStats = GetNode<UnitStats>("Stats/Current");
		myBaseStats = GetNode<UnitStats>("Stats/Base");
		healthBar = GetNode<TextureProgress>("HealthBar");
		currentFramePosition = GlobalPosition;
		previousFramePosition = GlobalPosition;
	}
	public override void _PhysicsProcess(float deltaTime)
	{
		previousFramePosition = currentFramePosition;
		currentFramePosition = GlobalPosition;
	}

	#region API
	/// <summary>
	/// Moves 1 Frame in the given direction.
	/// Vector2 direction should only have -1, 0 and 1 as values.
	/// Moves using own speed from StatsComponent.
	/// This should be used inside a _Process function.
	/// </summary>
	public void MoveInDirection(Vector2 direction)
	{
		MoveAndSlide(direction.Normalized() * myCurrentStats.Speed);
	}
	/// <summary>
	/// Returns the closest Unit within range of me.
	/// This should be used inside a _Process function.
	/// </summary>
	public Unit AcquireTarget(float range)
	{
		var targetAcquirer = GetNode<TargetAcquirer>("TargetAcquirer");
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
		weaponEquipped.Shoot(GlobalPosition, targetToShootAt.GlobalPosition + new Vector2(0, SHOOT_Y_OFFSET));
	}

	public void ReceiveHit(Unit fromUnit)
	{
		if(myCurrentStats.Health == 0)
			return;

		myCurrentStats.Health -= fromUnit.EquippedWeapon.Damage;
		myCurrentStats.Health = Mathf.Clamp(myCurrentStats.Health, 0, myBaseStats.Health);

		UpdateHealthBar();

		if(myCurrentStats.Health == 0)
			Die();
	}

	private void Die()
	{

	}
	private void UpdateHealthBar()
	{
		healthBar.MinValue = 0;
		healthBar.MaxValue = myBaseStats.Health;
		healthBar.Value = myCurrentStats.Health;
	}
	#endregion
}
