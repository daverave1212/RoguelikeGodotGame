using System;

using Godot;

public class UnitAIController : Node
{
	protected Unit MyUnit;

	public override void _Ready()
	{
		MyUnit = GetParent<Unit>();
	}

	public override void _PhysicsProcess(float delta)
	{
		if(MyUnit.IsDead)
			return;

		var random = new Random();
		var dirX = (float)(random.NextDouble() * 2 - 1);
		var dirY = (float)(random.NextDouble() * 2 - 1);

		var moveDirection = new Vector2(dirX, dirY).Normalized();
		MyUnit.MoveInDirection(moveDirection);
	}
}
