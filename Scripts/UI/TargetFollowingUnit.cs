using Godot;

public class TargetFollowingUnit : Sprite
{
	public Node2D ObjectItIsFollowing;
	[Export] public float FollowSpeed = 5f;
	[Export] public float YOffsetOnTarget = 10f;

	public override void _PhysicsProcess(float deltaTime)
	{
		// Show or Hide when it acquires or loses a target
		var justAcquiredTarget = Visible == false && ObjectItIsFollowing != null;
		var justLostTarget = Visible && ObjectItIsFollowing == null;

		if(justLostTarget)
		{
			Visible = false;
			return;
		}

		if(justAcquiredTarget)
		{
			Visible = true;
			var randomOffset = new Vector2((-10).Random(10), (-10).Random(10));
			GlobalPosition = ObjectItIsFollowing.GlobalPosition + randomOffset;
		}

		// Don't update position if hidden
		if(Visible == false)
			return;

		var objectPosition = ObjectItIsFollowing.GlobalPosition + new Vector2(0, YOffsetOnTarget);

		var distanceToObjectByX = objectPosition.x - GlobalPosition.x;
		var distanceToObjectByY = objectPosition.y - GlobalPosition.y;
		var xIncrement = FollowSpeed * distanceToObjectByX * deltaTime;
		var yIncrement = FollowSpeed * distanceToObjectByY * deltaTime;

		var positionIncrement = new Vector2(xIncrement, yIncrement);
		if(positionIncrement.x != 0 && positionIncrement.y != 0)
			GD.Print(positionIncrement);
		GlobalPosition += positionIncrement;

	}
}
