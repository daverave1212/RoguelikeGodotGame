using Godot;
using System;

public class UnitAIController : Node
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
		Random random = new Random();
		var dirX = (float) (random.NextDouble() * 2 - 1);
		var dirY = (float) (random.NextDouble() * 2 - 1);
		
		var moveDirection = new Vector2(dirX, dirY).Normalized();
		MyUnit.MoveInDirection(moveDirection);
		
	}
}
