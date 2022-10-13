using Godot;

public class DumbBanditAIController : UnitAIController
{
	bool IsMoving, IsShooting;
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
		if(MyUnit.IsDead)
		{
			Delay.Cancel(MoveRandomly);
			return;
		}

		var dirX = Utils.RandomFloat(-1, 1);
		var dirY = Utils.RandomFloat(-1, 1);
		MoveDirection = new Vector2(dirX, dirY);
		IsMoving = true;
		Delay.DoAfter(1f, () => IsMoving = false);
	}
	void TryShoot()
	{
		if(MyUnit.IsMoving)
			return;

		if(MyUnit.IsDead)
		{
			Delay.Cancel(TryShoot);
			return;
		}

		var targetPlayerUnit = MyUnit.AcquireTarget();
		if(targetPlayerUnit == null)
			return;

		IsShooting = true;
		Delay.DoAfter(0.5f, () =>
		{
			MyUnit.PlayAnimation("Idle");
			IsShooting = false;
		});
		MyUnit.ShootWeaponAt(targetPlayerUnit);
		MyUnit.PlayAnimation("Attack");
	}

	public override void _PhysicsProcess(float delta)
	{
		if(IsMoving && IsShooting == false && MyUnit.IsDead == false)
			MyUnit.MoveInDirection(MoveDirection);
	}
}
