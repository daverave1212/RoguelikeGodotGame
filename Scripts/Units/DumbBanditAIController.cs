using System;

using Godot;

public class DumbBanditAIController : UnitAIController
{
	bool IsMoving = false;
	Vector2 MoveDirection;

	public override void _Ready()
	{
		MyUnit = GetParent<Unit>();
		Delay.DoEvery(4f, MoveRandomly);
		Delay.DoEvery(1.5f, TryShoot);
		GD.Print($"My tag: {MyUnit.Tag}");
	}
	
	void MoveRandomly()
	{
		var dirX = Utils.RandomFloat(-1, 1);
		var dirY = Utils.RandomFloat(-1, 1);
		MoveDirection = new Vector2(dirX, dirY);
		IsMoving = true;
		Delay.DoAfter(1f, () =>
		{
			IsMoving = false;
		});
	}
	void TryShoot()
	{
		if (MyUnit.IsMoving)
			return;
		var targetPlayerUnit = MyUnit.AcquireTarget();
		if (targetPlayerUnit == null) return;
		MyUnit.PlayAnimation("Attack");
		MyUnit.ShootWeaponAt(targetPlayerUnit);
	}

	public override void _PhysicsProcess(float delta)
	{
		if (IsMoving)
			MyUnit.MoveInDirection(MoveDirection);
	}
}
