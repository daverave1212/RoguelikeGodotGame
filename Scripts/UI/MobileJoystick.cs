using Godot;

public class MobileJoystick : Sprite
{
	private Node2D handle;
	private bool isHeld;

	public static Vector2 MoveDirection;

	public override void _Ready()
	{
		handle = (Node2D)GetChild(0);
	}

	public override void _Input(InputEvent @event)
	{
		if(@event is InputEventScreenTouch touch)
			isHeld = touch.Pressed;
	}

	public override void _Process(float delta)
	{
		var mousePos = GetGlobalMousePosition();
		var radius = Texture.GetWidth() / 2f * GlobalScale.x;
		var dist = mousePos.DistanceTo(GlobalPosition);
		var lmb = Input.IsMouseButtonPressed((int)ButtonList.Left);

		if(lmb.Once("left-press-ui-joystick") && dist < radius)
			isHeld = true;
		if((lmb == false).Once("left-release-ui-joystick"))
			isHeld = false;

		if(isHeld == false)
		{
			handle.Position = Vector2.Zero;
			MoveDirection = Vector2.Zero;
			return;
		}

		var dir = GlobalPosition.DirectionBetweenPoints(mousePos);
		var maxDist = Mathf.Min(dist, radius);
		var handlePos = GlobalPosition.MoveInDirection(dir, maxDist);

		handle.GlobalPosition = handlePos;
		MoveDirection = dir;
	}
}
