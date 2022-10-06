using System;
using System.Collections.Generic;

using Godot;

/// <summary>
/// Use <see cref="DoAfter"/> and <see cref="DoEvery"/> to delay method calls.
/// </summary>
public class Delay : Node
{
	private class Timer
	{
		public Action method;
		public bool isLooping;
		public float delay, time;
		public bool IsDisposed => method == null;

		public Timer(float seconds, bool isLooping, Action method)
		{
			delay = seconds;
			this.isLooping = isLooping;
			this.method = method;
		}

		public void Update(float dt)
		{
			time += dt;

			if(time < delay)
				return;

			Trigger(true);

			if(isLooping == false)
				Dispose();
		}
		public void Restart()
		{
			time = 0;
		}
		public void Trigger(bool reset)
		{
			method?.Invoke();

			if(reset)
				Restart();
		}

		private void Dispose()
		{
			method = null;
		}
	}
	private readonly static List<Timer> timers = new List<Timer>();

	public override void _Process(float delta)
	{
		var toBeRemoved = new List<Timer>();
		for(int i = 0; i < timers.Count; i++)
		{
			var timer = timers[i];

			timer.Update(delta);

			if(timer.IsDisposed)
				toBeRemoved.Add(timer);
		}

		for(int i = 0; i < toBeRemoved.Count; i++)
		{
			var timer = toBeRemoved[i];
			timers.Remove(timer);
		}
	}

	public static void DoAfter(float seconds, Action method)
	{
		timers.Add(new Timer(seconds, false, method));
	}
	public static void DoEvery(float seconds, Action method)
	{
		timers.Add(new Timer(seconds, true, method));
	}
	public static void Cancel(Action method)
	{
		var timersToRemove = new List<Timer>();
		for(int i = 0; i < timers.Count; i++)
			if(timers[i].method == method)
				timersToRemove.Add(timers[i]);

		for(int i = 0; i < timersToRemove.Count; i++)
			timers.Remove(timersToRemove[i]);
	}
	public static void AddOffset(float secondsOffset, Action method)
	{
		for(int i = 0; i < timers.Count; i++)
			if(timers[i].method == method)
				timers[i].delay += secondsOffset;
	}
}
