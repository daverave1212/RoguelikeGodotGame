using System;
using System.Linq;
using Godot;

public class TargetAcquirer : Area2D
{
	/// <summary>
	/// Returns the closest Unit object in the area using the range of the CollisionShape2D
	/// The Unit returned is never on the same side as the caster unit (since this component will always be attached to a Unit).
	/// This range can be seen in the inspector.
	/// - Unit exclude is an exception for target acquisition - it will ignore this Node
	/// NOTE: The exclude parameter should not be the same as the unit with this component, or on the same side.
	/// </summary>
	public Unit AcquireTarget(Unit exclude = null)
	{
		var colShape = GetNode<CollisionShape2D>("Range");
		var myCircleShape = (CircleShape2D)colShape.Shape;
		return AcquireTarget(myCircleShape.Radius, exclude);
	}

	/// <summary>
	/// Returns the closest Unit object in the area.
	/// The Unit returned is never on the same side as the caster unit (since this component will always be attached to a Unit).
	/// - Unit exclude is an exception for target acquisition - it will ignore this Node
	/// - range is an optional parameter that will force use a specific range
	/// NOTE: The exclude parameter should not be the same as the unit with this component, or on the same side.
	/// </summary>
	public Unit AcquireTarget(float range, Unit exclude = null)
	{
		var myUnit = (Unit) GetParent();
		var colShape = GetNode<CollisionShape2D>("Range");
		var myCircleShape = (CircleShape2D)colShape.Shape;
		var radiusBackup = myCircleShape.Radius;
		myCircleShape.Radius = range;
		if (range != 75) GD.Print($"Set range to {myCircleShape.Radius}");
		
		if (exclude != null && exclude.Tag == myUnit.Tag)
		{
			GD.Print($"WARNING: Exclude unit tag ${exclude.Tag} is the same as this unit's tag {myUnit.Tag}.");
			GD.Print($"The parameter is fixed inside this function, but it indicates this function may be used incorrectly.");
			exclude = null;
		}

		Node[] nodesInRange = GetOverlappingBodies().OfType<Node>().ToArray();
		if (range != 75) GD.Print($"Acquired units within {myCircleShape.Radius} = {nodesInRange.Length}");
		if (nodesInRange.Length == 1)
		{
			if (range != 75) GD.Print($"-- {nodesInRange[0].Name}");
		}

		if(exclude != null && nodesInRange.Length == 1)
		{
			if (range != 75) GD.Print("YES; EXCLUDE WAS GIVEN!");
			return null; // If exclude is the only found node
		}

		var closestDistanceFound = Math.Pow(range, 2);  // To avoid using SQRT, I calculate distances at power of 2
		Unit closestNodeFound = null;

		foreach(var node in nodesInRange)
		{
			if(node == exclude)
				continue;
			
			if (!(node is Unit))
				continue;

			if(node is Unit unit)
			{
				if (unit.Tag == myUnit.Tag)
					continue;
				var distanceToNode =
					Math.Pow(unit.GlobalPosition.x - GlobalPosition.x, 2) +
					Math.Pow(unit.GlobalPosition.y - GlobalPosition.y, 2);

				if(distanceToNode < closestDistanceFound)
				{
					closestDistanceFound = distanceToNode;
					closestNodeFound = unit;
				}
			}
		}
		myCircleShape.Radius = radiusBackup;
		
		if (range != 75) GD.Print($"Returning node: {closestNodeFound?.Name}");
		return closestNodeFound;

	}
}
