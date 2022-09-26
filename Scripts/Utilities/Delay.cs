using System;
using System.Collections.Generic;

using Godot;

/// <summary>
/// Use the static methods DoAfter and DoEvery for time functionality.
/// </summary>
public class Delay : Node
{
	public class Timer
	{
		private Action method;
		private readonly bool isLooping;
		private readonly float givenTime;
		public float Time { get; private set; }

		public Timer(float seconds, bool isLooping, Action method)
		{
			Time = seconds;
			givenTime = seconds;
			this.isLooping = isLooping;
			this.method = method;
		}

		public void AddDelay(float seconds)
		{
			Time += seconds;
		}
		public void Update(float dt)
		{
			Time -= dt;

			if(Time <= 0)
			{
				method();

				if(isLooping)
					Time = givenTime;
			}
		}
		/// <summary>
		/// Nulls out the method just in case so that it can be GC-ed.
		/// </summary>
		public void Dispose()
		{
			method = null;
		}
	}

	private readonly static List<Timer> timers = new List<Timer>();

	public override void _Process(float delta)
	{
		var removeTimer = new List<Timer>();
		for(int i = 0; i < timers.Count; i++)
		{
			var timer = timers[i];

			timer.Update(delta);

			if(timer.Time <= 0)
				removeTimer.Add(timer);
		}

		for(int i = 0; i < removeTimer.Count; i++)
		{
			var timer = removeTimer[i];
			timer.Dispose();
			timers.Remove(timer);
		}
	}

	public static Timer DoAfter(float seconds, Action method)
	{
		var timer = new Timer(seconds, false, method);
		timers.Add(timer);
		return timer;
	}
	public static Timer DoEvery(float seconds, Action method)
	{
		var timer = new Timer(seconds, true, method);
		timers.Add(timer);
		return timer;
	}
}
