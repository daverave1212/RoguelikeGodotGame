using Godot;

public class InputKeyboardMouse : InputHandler
{
	public override void _Process(float delta)
	{
		var dirX = Input.GetAxis("ui_left", "ui_right");
		var dirY = Input.GetAxis("ui_up", "ui_down");

		if(InputMobileJoystick.IsVisible == false)
			MoveDirection = new Vector2(dirX, dirY).Normalized();
	}
}
