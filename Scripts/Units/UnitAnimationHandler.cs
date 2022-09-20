using Godot;

public class UnitAnimationHandler : Node
{
	Unit UnitParent;
	Vector2 PreviousFramePosition;

	public override void _Ready()
	{
		UnitParent = GetParent<Unit>();
		PreviousFramePosition = UnitParent.GlobalPosition;
	}

	enum AnimationStates { Walking, Idle };
	AnimationStates CurrentAnimationState = AnimationStates.Idle;

	public override void _PhysicsProcess(float deltaTime)
	{
		var myPositionNow = UnitParent.GlobalPosition;
		var deltaX = myPositionNow.x - PreviousFramePosition.x;
		var deltaY = myPositionNow.y - PreviousFramePosition.y;

		var didMove = deltaX != 0 || deltaY != 0;
		var movedLeft = deltaX < 0;

		var animatedSprite = UnitParent.GetNode<AnimatedSprite>("AnimatedSprite");

		if(didMove)
		{
			if(CurrentAnimationState == AnimationStates.Idle)
			{
				CurrentAnimationState = AnimationStates.Walking;
				animatedSprite.Play("Walk");
			}
			animatedSprite.FlipH = movedLeft;
		}

		if(didMove == false && CurrentAnimationState == AnimationStates.Walking)
		{
			CurrentAnimationState = AnimationStates.Idle;
			animatedSprite.Play("Idle");
		}

		PreviousFramePosition = myPositionNow;
	}

}
