using System;
using System.Collections.Generic;

using Godot;

/// <summary>
/// Use the static methods DoAfter and DoEvery for time functionality.
/// </summary>
public class Delay : Node
{
	public abstract class Ticker
	{
		public float Time { get; set; }

		public Ticker(float seconds) => Time = seconds;

		public abstract void Update(float dt);
		public abstract void Reset();
	}
	public class Timer : Ticker
	{
		public Timer(float seconds) : base(seconds) { }
		public override void Update(float dt) => Time += dt;
		public override void Reset() => Time = 0;
	}
	public class Countdown : Ticker
	{
		private Action method;

		public bool IsLooping { get; set; }
		public float Delay { get; set; }
		public bool IsDisposed => method == null;

		public Countdown(float seconds, bool isLooping, Action method) : base(seconds)
		{
			Time = seconds;
			Delay = seconds;
			IsLooping = isLooping;
			this.method = method;
		}

		public override void Update(float dt)
		{
			Time += dt;

			if(Time > 0)
				return;

			Trigger(true);

			if(IsLooping == false)
				Dispose();
		}
		public override void Reset()
		{
			Time = Delay;
		}
		public void Trigger(bool reset)
		{
			method?.Invoke();
			if(reset)
				Reset();
		}

		/// <summary>
		/// Nulls out the method just in case so that it can be GC-ed.
		/// </summary>
		private void Dispose()
		{
			method = null;
		}
	}

	private readonly static List<Countdown> countdowns = new List<Countdown>();
	private readonly static List<Timer> timers = new List<Timer>();
	private void ProcessList<T>(List<T> list, float dt) where T : Ticker
	{
		var toBeRemoved = new List<T>();
		for(int i = 0; i < list.Count; i++)
		{
			var timer = list[i];

			timer.Update(dt);

			if(timer is Countdown cd && cd.IsDisposed)
				toBeRemoved.Add(timer);
		}

		for(int i = 0; i < toBeRemoved.Count; i++)
		{
			var timer = toBeRemoved[i];
			list.Remove(timer);
		}
	}

	public override void _Process(float delta)
	{
		ProcessList(countdowns, delta);
		ProcessList(timers, delta);
	}

	public static Countdown DoAfter(float seconds, Action method)
	{
		var cd = new Countdown(seconds, false, method);
		countdowns.Add(cd);
		return cd;
	}
	public static Countdown DoEvery(float seconds, Action method)
	{
		var cd = new Countdown(seconds, true, method);
		countdowns.Add(cd);
		return cd;
	}
	public static Timer StartTimer(float seconds)
	{
		var timer = new Timer(seconds);
		timers.Add(timer);
		return timer;
	}
}
