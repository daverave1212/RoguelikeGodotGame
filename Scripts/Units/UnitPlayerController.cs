using Godot;

public class UnitPlayerController : Node
{
	public static Unit PlayerUnitInstance { get; private set; }
	Unit MyUnit, CurrentTargetUnit;

	public override void _Ready()
	{
		MyUnit = GetParent<Unit>();
		PlayerUnitInstance = MyUnit;

		Delay.DoEvery(0.25f, TryAcquireTarget);
		Delay.DoEvery(0.1f, () => TryShootCurrentTarget(0.1f));
	}
	public override void _PhysicsProcess(float delta)
	{
		var moveDirection = InputHandler.MoveDirection;
		MyUnit.MoveInDirection(moveDirection);

		// __TestTargetAcquisition();
		// __TestWeaponShoot();
		// __TestTargetFollowingUnit();
		// __TestWeaponDatabase();

	}

	/// <summary> Tries to get a new target unit and moves the visual target over that unit </summary>
	void TryAcquireTarget()
	{
		var scene = GetNode(World.WorldNodePath);
		var nodeTarget = scene.GetNode<TargetFollowingUnit>("TargetFollowingUnit");
		var previousTargetUnit = CurrentTargetUnit;
		CurrentTargetUnit = MyUnit.AcquireTarget();

		if(CurrentTargetUnit == null || CurrentTargetUnit.IsDead)
		{
			nodeTarget.ObjectItIsFollowing = null;
			CurrentTargetUnit = null;
			return;
		}
		if(CurrentTargetUnit != previousTargetUnit)
		{
			nodeTarget.ObjectItIsFollowing = CurrentTargetUnit;
		}
	}

	float timeSinceLastAttack = 0f;
	void TryShootCurrentTarget(float deltaTime)
	{
		timeSinceLastAttack += deltaTime;

		var weapon = MyUnit.EquippedWeapon;

		if(weapon == null || CurrentTargetUnit == null || MyUnit.IsMoving || timeSinceLastAttack < weapon.AttackCooldownSeconds)
			return; // If attack not yet ready, return

		// Else, if the attack is ready to shoot...
		MyUnit.ShootWeaponAt(CurrentTargetUnit);
		timeSinceLastAttack = 0f;

	}

	#region Test Methods / should remove these later
	public void __TestTargetAcquisition()
	{
		if(Input.IsActionPressed("ui_accept").Once("target-key"))
		{
			var targetAcquirer = GetNode<TargetAcquirer>("Acquirer");
			var foundKBody = targetAcquirer.AcquireTarget(exclude: MyUnit);
			if(foundKBody != null)
				GD.Print(foundKBody.Name);
		}
	}
	public void __TestWeaponShoot()
	{
		if(Input.IsActionPressed("ui_accept").Once("shoot-key"))
		{
			MyUnit.ShootWeaponAt(null);
		}
	}
	bool IsFollowing = false;
	public void __TestTargetFollowingUnit()
	{
		if(Input.IsActionPressed("ui_accept"))
		{
			var ySortNode = MyUnit.GetParent();
			var scene = ySortNode.GetParent();
			var targetFollowingUnit = scene.GetNode<TargetFollowingUnit>("TargetFollowingUnit");

			IsFollowing = !IsFollowing;

			if(IsFollowing == false)
			{
				targetFollowingUnit.ObjectItIsFollowing = null;
				return;
			}
			else
			{
				targetFollowingUnit.ObjectItIsFollowing = MyUnit;
			}
		}
	}
	public void __TestWeaponDatabase()
	{
		if(Input.IsActionPressed("ui_accept"))
		{
			var weaponNode = WeaponsDatabase.Create("Broadsword");
			GD.Print(weaponNode.Name);
			// weaponNode.GlobalPosition = MyUnit.GlobalPosition;
			weaponNode.Position = new Vector2(0, 0);
			GD.Print(MyUnit.GlobalPosition);
			GD.Print(weaponNode.GlobalPosition);
			GD.Print(weaponNode.Position);
			GD.Print(weaponNode.Visible);
			MyUnit.AddChild(weaponNode);
			GD.Print("Hmm");
		}

	}
	#endregion

}
