using Godot;

public class UnitPlayerController : Node
{

	StatsComponent MyStats;
	Unit MyUnit;

	public override void _Ready()
	{
		MyUnit = GetParent<Unit>();
		MyStats = MyUnit.GetNode<StatsComponent>("StatsComponent");
	}

	public override void _PhysicsProcess(float delta)
	{
		var dirX = Input.GetAxis("ui_left", "ui_right");
		var dirY = Input.GetAxis("ui_up", "ui_down");

		var moveDirection = MobileJoystick.MoveDirection;//new Vector2(dirX, dirY).Normalized();
		MyUnit.MoveInDirection(moveDirection);

		// __TestTargetAcquisition();
		__TestWeaponShoot();
		// __TestTargetFollowingUnit();
		// __TestWeaponDatabase();

	}



	// Test methods; should remove these later
	public void __TestTargetAcquisition()
	{
		if(Input.IsActionPressed("ui_accept"))
		{
			var targetAcquirer = GetNode<TargetAcquirer>("TargetAcquirer");
			var foundKBody = targetAcquirer.AcquireTarget(exclude: MyUnit);
			if(foundKBody != null)
				GD.Print(foundKBody.Name);
		}
	}
	public void __TestWeaponShoot()
	{
		if(Input.IsActionPressed("ui_accept"))
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

}
