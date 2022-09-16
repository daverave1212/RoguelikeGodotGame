using Godot;
using System;

public class TargetAcquirer : Area2D
{
	/// <summary>
	/// Returns the closest KinematicBody2D object in the area using the range of the CollisionShape2D
	/// This range can be seen in the inspector.
	/// - KinematicBody2D exclude is an exception for target acquisition - it will ignore this Node
	/// </summary>
	public KinematicBody2D AcquireTarget(KinematicBody2D exclude = null)
	{
		CollisionShape2D colShape = GetNode<CollisionShape2D>("CollisionShape2D");
		CircleShape2D myCircleShape = (CircleShape2D)colShape.Shape;
		return AcquireTarget(myCircleShape.Radius, exclude);
	}

	/// <summary>
	/// Returns the closest KinematicBody2D object in the area.
	/// - KinematicBody2D exclude is an exception for target acquisition - it will ignore this Node
	/// - range is an optional parameter that will force use a specific range
	/// </summary>
	public KinematicBody2D AcquireTarget(float range, KinematicBody2D exclude = null)
	{
		var nodesInRange = GetOverlappingBodies();
		if (exclude != null && nodesInRange.Count == 1)
			return null; // If exclude is the only found node
		
		CollisionShape2D colShape = GetNode<CollisionShape2D>("CollisionShape2D");
		CircleShape2D myCircleShape = (CircleShape2D)colShape.Shape;
		var radiusBackup = myCircleShape.Radius;
		myCircleShape.Radius = range;
		var myRadius = range;
		var closestDistanceFound = Math.Pow(myRadius, 2);	// To avoid using SQRT, I calculate distances at power of 2
		var closestNodeFound = exclude;
		
		foreach (KinematicBody2D node in nodesInRange)
		{
			if (node == exclude)
				continue;

			var distanceToNode =
				Math.Pow((node.GlobalPosition.x - GlobalPosition.x), 2) +
				Math.Pow((node.GlobalPosition.y - GlobalPosition.y), 2);
			if (distanceToNode < closestDistanceFound)
			{
				closestDistanceFound = distanceToNode;
				closestNodeFound = node;
			}
		}
		myCircleShape.Radius = radiusBackup;
		
		return closestNodeFound;
		
	}
}
