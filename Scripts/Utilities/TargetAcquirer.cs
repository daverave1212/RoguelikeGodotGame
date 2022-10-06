using System;

using Godot;

public class TargetAcquirer : Area2D
{
	/// <summary>
	/// Returns the closest KinematicBody2D object in the area using the range of the CollisionShape2D
	/// This range can be seen in the inspector.
	/// - KinematicBody2D exclude is an exception for target acquisition - it will ignore this Node
	/// </summary>
	public KinematicBody2D AcquireTarget(KinematicBody2D exclude = null)
	{
		var colShape = GetNode<CollisionShape2D>("Range");
		var myCircleShape = (CircleShape2D)colShape.Shape;
		return AcquireTarget(myCircleShape.Radius, exclude);
	}

	/// <summary>
	/// Returns the closest KinematicBody2D object in the area.
	/// - KinematicBody2D exclude is an exception for target acquisition - it will ignore this Node
	/// - range is an optional parameter that will force use a specific range
	/// </summary>
	public KinematicBody2D AcquireTarget(float range, KinematicBody2D exclude = null)
	{
		var colShape = GetNode<CollisionShape2D>("Range");
		var myCircleShape = (CircleShape2D)colShape.Shape;
		var radiusBackup = myCircleShape.Radius;
		myCircleShape.Radius = range;

		var nodesInRange = GetOverlappingBodies();

		if(exclude != null && nodesInRange.Count == 1)
			return null; // If exclude is the only found node

		var closestDistanceFound = Math.Pow(range, 2);  // To avoid using SQRT, I calculate distances at power of 2
		KinematicBody2D closestNodeFound = null;

		foreach(var node in nodesInRange)
		{
			if(node == exclude)
				continue;

			if(node is KinematicBody2D kb)
			{
				var distanceToNode =
					Math.Pow(kb.GlobalPosition.x - GlobalPosition.x, 2) +
					Math.Pow(kb.GlobalPosition.y - GlobalPosition.y, 2);

				if(distanceToNode < closestDistanceFound)
				{
					closestDistanceFound = distanceToNode;
					closestNodeFound = kb;
				}
			}
		}
		myCircleShape.Radius = radiusBackup;

		return closestNodeFound;

	}
}
