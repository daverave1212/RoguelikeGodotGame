using Godot;

public class UnitStats : Node
{
	[Export] public float Health { get; set; } = 100;
	[Export] public float Speed { get; set; } = 50;
	[Export] public int Armor { get; set; } = 0;
}
