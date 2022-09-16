using Godot;
using System;

public class UnitPlayerController : Node
{
	
	StatsComponent MyStats;
	Unit MyUnit;
	
	public override void _Ready()
	{
		MyUnit = GetParent<Unit>();
		MyStats = MyUnit.GetNode<StatsComponent>("StatsComponent");
	}

	public override void _Input(InputEvent @event)
	{
		if(@event is InputEventScreenTouch touch)
			GD.Print("touch");
	}

	public override void _PhysicsProcess(float delta)
	{
		var dirX = Input.GetAxis("ui_left", "ui_right");
		var dirY = Input.GetAxis("ui_up", "ui_down");
		
		var moveDirection = new Vector2(dirX, dirY).Normalized();
		MyUnit.MoveInDirection(moveDirection);
		
		__TestTargetAcquisition();
	}
	
	
	
	
	public void __TestTargetAcquisition()
	{
		if (Input.IsActionPressed("ui_accept")) {
			var targetAcquirer = GetNode<TargetAcquirer>("TargetAcquirer");
			var foundKBody = targetAcquirer.AcquireTarget(exclude: MyUnit);
			if (foundKBody != null)
				GD.Print(foundKBody.Name);
		}
	}
	
}
