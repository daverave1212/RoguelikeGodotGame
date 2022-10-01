using Godot;

public class InputKeyboardMouse : InputHandler
{
	private Node2D player;
	private bool isMoving;

	public override void _Ready()
	{
		player = GetNode<Node2D>("/root/World/YSort/RatPlayer");
	}
	public override void _Input(InputEvent @event)
	{
		if(InputMobileJoystick.IsHeld)
			return;

		if(@event is InputEventKey e)
			isMoving = e.Pressed;
	}
	public override void _Process(float delta)
	{
		if(isMoving)
			MoveDirection = player.Position.Direction(player.GetGlobalMousePosition());
	}
}
