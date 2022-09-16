using Godot;
using System;

public class Utils
{
	public static T GetNodeByType<T>(Node parentNode) where T : Node
	{
		var allChildren = parentNode.GetChildren();
		foreach (Node child in allChildren)
		{
			if (child is T)
				return (T)child;
		}
		return null;
	}
	
	public static int RandomIntBetween(int low, int high)
	{
		return (new Random()).Next(low, high);
	}

}
