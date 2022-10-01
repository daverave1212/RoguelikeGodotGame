using Godot;

public class InputMobileJoystick : InputHandler
{
	private Sprite joystick;
	private Node2D handle;
	private static bool isHeld;

	public static bool IsHeld => isHeld;

	public override void _Ready()
	{
		joystick = GetNode<Sprite>("/root/World/YSort/RatPlayer/Camera2D/Joystick");
		handle = (Node2D)joystick.GetChild(0);
	}

	public override void _Input(InputEvent @event)
	{
		if(@event is InputEventMouseButton)
			joystick.GlobalPosition = joystick.GetGlobalMousePosition();
		else if(@event is InputEventScreenTouch e)
			joystick.GlobalPosition = joystick.ToGlobal(e.Position);
	}

	public override void _Process(float delta)
	{
		var mousePos = joystick.GetGlobalMousePosition();
		var radius = joystick.Texture.GetWidth() / 2f * joystick.GlobalScale.x;
		var dist = mousePos.DistanceTo(joystick.GlobalPosition);
		var lmb = Input.IsMouseButtonPressed((int)ButtonList.Left);

		if((lmb && dist > radius * 0.05f).Once("started-dragging"))
			isHeld = true;
		if((lmb == false).Once("lmb-release"))
			isHeld = false;

		joystick.Visible = isHeld;
		if(isHeld == false)
		{
			handle.Position = Vector2.Zero;
			MoveDirection = Vector2.Zero;
			return;
		}

		var dir = joystick.GlobalPosition.Direction(mousePos);
		var maxDist = Mathf.Min(dist, radius);
		var handlePos = joystick.GlobalPosition.MoveInDirection(dir, maxDist);

		handle.GlobalPosition = handlePos;
		MoveDirection = dir.Normalized();
	}
}
