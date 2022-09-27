using Godot;

public class InputMobileJoystick : InputHandler
{
	private Sprite joystick;
	private Node2D handle;
	private bool isHeld;

	public static bool IsVisible { get; private set; }

	public override void _Ready()
	{
		joystick = GetNode<Sprite>("/root/World/YSort/RatPlayer/Camera2D/Joystick");
		handle = (Node2D)joystick.GetChild(0);
	}

	public override void _Input(InputEvent @event)
	{
		if(@event is InputEventScreenTouch touch)
		{
			IsVisible = true;
			isHeld = touch.Pressed;
		}
	}

	public override void _Process(float delta)
	{
		joystick.Visible = IsVisible;

		if(IsVisible == false)
			return;

		var mousePos = joystick.GetGlobalMousePosition();
		var radius = joystick.Texture.GetWidth() / 2f * joystick.GlobalScale.x;
		var dist = mousePos.DistanceTo(joystick.GlobalPosition);
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

		var dir = joystick.GlobalPosition.Direction(mousePos);
		var maxDist = Mathf.Min(dist, radius);
		var handlePos = joystick.GlobalPosition.MoveInDirection(dir, maxDist);

		handle.GlobalPosition = handlePos;
		MoveDirection = dir.Normalized();
	}
}
