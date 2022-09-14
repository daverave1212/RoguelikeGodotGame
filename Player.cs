using Godot;

public class Player : KinematicBody2D
{
    [Export]
    public float Speed { get; set; } = 5f;

    public override void _Ready()
    {

    }

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventScreenTouch touch)
            GD.Print("touch");
    }

    public override void _PhysicsProcess(float delta)
    {
        var dirX = Input.GetAxis("ui_left", "ui_right");
        var dirY = Input.GetAxis("ui_up", "ui_down");
        MoveAndSlide(new Vector2(dirX, dirY).Normalized() * 200f);
    }
}
