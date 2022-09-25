using System;
using System.Collections.Generic;
using System.Globalization;

using Godot;

using Vector2 = Godot.Vector2;

public static class Utils
{
	public static T GetNodeByType<T>(Node parentNode) where T : Node
	{
		var allChildren = parentNode.GetChildren();
		foreach(Node child in allChildren)
		{
			if(child is T t)
				return t;
		}
		return null;
	}

	/// <summary>
	/// The type of limitation used by <see cref="Limit(float, float, float, Limitation)"/> and <see cref="Limit(int, int, int, Limitation)"/>.
	/// </summary>
	public enum Limitation { ClosestBound, Overflow }
	/// <summary>
	/// The type of rounding direction used by <see cref="Round"/>.
	/// </summary>
	public enum RoundWay { Closest, Up, Down }
	/// <summary>
	/// The prefered case when the number is in the middle (ends on '5' or on '.5') and the direction is <see cref="RoundWay.Closest"/>.
	/// This is used by <see cref="Round"/>.
	/// </summary>
	public enum RoundWhenMiddle { TowardEven, AwayFromZero, TowardZero, TowardNegativeInfinity, TowardPositiveInfinity }
	/// <summary>
	/// The type of size convertion from one size unit to another. This is used by <see cref="ToDataSize"/>.
	/// </summary>
	public enum SizeConvertion
	{
		Bit_Byte, Bit_KB,
		Byte_Bit, Byte_KB, Byte_MB,
		KB_Bit, KB_Byte, KB_MB, KB_GB,
		MB_Byte, MB_KB, MB_GB, MB_TB,
		GB_KB, GB_MB, GB_TB,
		TB_MB, TB_GB
	}
	/// <summary>
	/// The type of number animations used by <see cref="AnimateUnit"/>. Also known as 'easing functions'.
	/// </summary>
	public enum Animation
	{
		BendWeak, // Sine
		Bend, // Cubic
		BendStrong, // Quint
		Circle, // Circ
		Elastic, // Elastic
		Swing, // Back
		Bounce // Bounce
	}
	/// <summary>
	/// The type of number animation direction used by <see cref="AnimateUnit"/>.
	/// </summary>
	public enum AnimationCurve { Backward, Forward, BackwardThenForward }

	/// <summary>
	/// The horizontal directions in the world.<br></br>
	/// # # #<br></br>
	/// 1 # 0<br></br>
	/// # # #<br></br>
	/// </summary>
	public enum DirectionH { Right, Left }
	/// <summary>
	/// The vertical directions in the world.<br></br>
	/// # 1 #<br></br>
	/// # # #<br></br>
	/// # 0 #<br></br>
	/// </summary>
	public enum DirectionV { Down, Up }
	/// <summary>
	/// The 4 directions in the world.<br></br>
	/// # 3 #<br></br>
	/// 2 # 0<br></br>
	/// # 1 #<br></br>
	/// </summary>
	public enum Direction4 { Right, Down, Left, Up }
	/// <summary>
	/// The 4 directions in the world + their diagonals.<br></br>
	/// 5 6 7<br></br>
	/// 4 # 0<br></br>
	/// 3 2 1<br></br>
	/// </summary>
	public enum Direction8 { Right, DownRight, Down, DownLeft, Left, UpLeft, Up, UpRight }

	/// <summary>
	/// Converts a <see cref="DirectionH"/> to a 360 degrees angle and returns it.
	/// </summary>
	public static float ToAngle(this DirectionH direction)
	{
		return ((int)direction) * 180f;
	}
	/// <summary>
	/// Converts a <see cref="DirectionV"/> to a 360 degrees angle and returns it.
	/// </summary>
	public static float ToAngle(this DirectionV direction)
	{
		return ((int)direction) * 180f + 90f;
	}
	/// <summary>
	/// Converts a <see cref="Direction4"/> to a 360 degrees angle and returns it.
	/// </summary>
	public static float ToAngle(this Direction4 direction)
	{
		return ((int)direction) * 90f;
	}
	/// <summary>
	/// Converts a <see cref="Direction8"/> to a 360 degrees angle and returns it.
	/// </summary>
	public static float ToAngle(this Direction8 direction)
	{
		return ((int)direction) * 45f;
	}
	/// <summary>
	/// Converts a <see cref="Direction4"/> to a directional unit (normalized) <see cref="Vector2"/> and returns it.
	/// </summary>
	public static Vector2 ToDirection(this Direction4 direction)
	{
		return ToAngle(direction).ToDirection();
	}
	/// <summary>
	/// Converts a <see cref="Direction8"/> to a directional unit (normalized) <see cref="Vector2"/> and returns it.
	/// </summary>
	public static Vector2 ToDirection(this Direction8 direction)
	{
		return ToAngle(direction).ToDirection();
	}
	/// <summary>
	/// Converts a <see cref="DirectionH"/> to a directional unit (normalized) <see cref="Vector2"/> and returns it.
	/// </summary>
	public static Vector2 ToDirection(this DirectionH direction)
	{
		return ToAngle(direction).ToDirection();
	}
	/// <summary>
	/// Converts a <see cref="DirectionV"/> to a directional unit (normalized) <see cref="Vector2"/> and returns it.
	/// </summary>
	public static Vector2 ToDirection(this DirectionV direction)
	{
		return ToAngle(direction).ToDirection();
	}

	/// <summary>
	/// Returns true only the first time a <paramref name="condition"/> is <see langword="true"/>.
	/// This is reset whenever the <paramref name="condition"/> becomes <see langword="false"/>.
	/// This process can be repeated <paramref name="max"/> amount of times, always returns <see langword="false"/> after that.<br></br>
	/// A <paramref name="uniqueID"/> needs to be provided that describes each type of condition in order to separate/identify them.
	/// </summary>
	public static bool Once(this bool condition, string uniqueID, uint max = uint.MaxValue)
	{
		if(gates.ContainsKey(uniqueID) == false && condition == false)
			return false;
		else if(gates.ContainsKey(uniqueID) == false && condition == true)
		{
			gates[uniqueID] = true;
			gateEntries[uniqueID] = 1;
			return true;
		}
		else
		{
			if(gates[uniqueID] == true && condition == true)
				return false;
			else if(gates[uniqueID] == false && condition == true)
			{
				gates[uniqueID] = true;
				gateEntries[uniqueID]++;
				return true;
			}
			else if(gateEntries[uniqueID] < max)
				gates[uniqueID] = false;
		}
		return false;
	}
	/// <summary>
	/// Switches the values of two variables.
	/// </summary>
	public static void Swap<T>(ref T a, ref T b)
	{
		(b, a) = (a, b);
	}

	/// <summary>
	/// Picks randomly a single <typeparamref name="T"/> value out of a <paramref name="list"/> and returns it.
	/// </summary>
	public static T Choose<T>(this IList<T> list)
	{
		return list[Random(0, list.Count - 1)];
	}
	/// <summary>
	/// Randomly shuffles the contents of a <paramref name="list"/>.
	/// </summary>
	public static void Shuffle<T>(this IList<T> list)
	{
		var n = list.Count;
		while(n > 1)
		{
			n--;
			var k = new Random().Next(n + 1);
			(list[n], list[k]) = (list[k], list[n]);
		}
	}
	/// <summary>
	/// Calculates the average <see cref="float"/> out of a <paramref name="list"/> of <see cref="float"/>s and returns it.
	/// </summary>
	public static float Average(this IList<float> list)
	{
		var sum = 0f;
		for(int i = 0; i < list.Count; i++)
			sum += list[i];
		return sum / list.Count;
	}

	/// <summary>
	/// Returns whether <paramref name="text"/> can be cast to a <see cref="float"/>.
	/// </summary>
	public static bool IsNumber(this string text)
	{
		return float.IsNaN(ToNumber(text)) == false;
	}
	/// <summary>
	/// Returns whether <paramref name="text"/> contains only letters.
	/// </summary>
	public static bool IsLetters(this string text)
	{
		for(int i = 0; i < text.Length; i++)
		{
			var isLetter = (text[i] >= 'A' && text[i] <= 'Z') || (text[i] >= 'a' && text[i] <= 'z');
			if(isLetter == false)
				return false;
		}
		return true;
	}
	/// <summary>
	/// Puts <paramref name="text"/> to the right with a set amount of <paramref name="spaces"/>
	/// if they are more than the <paramref name="text"/>'s length.<br></br>
	/// </summary>
	public static string Align(this string text, int spaces)
	{
		return string.Format("{0," + spaces + "}", text);
	}
	/// <summary>
	/// Adds <paramref name="text"/> to itself a certain amount of <paramref name="times"/> and returns it.
	/// </summary>
	public static string Repeat(this string text, int times)
	{
		var result = "";
		times = times.Limit(0, 999999);
		for(int i = 0; i < times; i++)
			result = $"{result}{text}";
		return result;
	}
	/// <summary>
	/// Tries to convert <paramref name="text"/> to a <see cref="float"/> and returns the result (<see cref="float.NaN"/> if unsuccessful).
	/// This also takes into account the system's default decimal symbol.
	/// </summary>
	public static float ToNumber(this string text)
	{
		var result = 0.0f;
		text = text.Replace(',', '.');
		var parsed = float.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out result);

		return parsed ? result : float.NaN;
	}

	/// <summary>
	/// Wraps a <paramref name="number"/> around 0 to <paramref name="range"/> and returns it.
	/// </summary>
	public static float Wrap(this float number, float range)
	{
		return ((number % range) + range) % range;
	}
	/// <summary>
	/// Transforms a <paramref name="progress"/> ranged [0-1] to an animated progress acording to an <paramref name="animation"/>
	/// and a <paramref name="curve"/>. The animation might be <paramref name="repeated"/> (if the provided progress is outside of the range [0-1].
	/// This is also known as easing functions.
	/// </summary>
	public static float Animate(this float progress, Animation animation, AnimationCurve curve, bool repeated = false)
	{
		var result = 0f;
		var x = progress.Limit(0, 1, repeated ? Limitation.Overflow : Limitation.ClosestBound);
		switch(animation)
		{
			case Animation.BendWeak:
				{
					result = curve == AnimationCurve.Backward ? 1 - Mathf.Cos(x * Mathf.Pi / 2f) :
						curve == AnimationCurve.Forward ? 1 - Mathf.Sin(x * Mathf.Pi / 2f) :
						-(Mathf.Cos(Mathf.Pi * x) - 1) / 2f;
					break;
				}
			case Animation.Bend:
				{
					result = curve == AnimationCurve.Backward ? x * x * x :
						curve == AnimationCurve.Forward ? 1 - Mathf.Pow(1 - x, 3) :
						(x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2);
					break;
				}
			case Animation.BendStrong:
				{
					result = curve == AnimationCurve.Backward ? x * x * x * x :
						curve == AnimationCurve.Forward ? 1 - Mathf.Pow(1 - x, 5) :
						(x < 0.5 ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2);
					break;
				}
			case Animation.Circle:
				{
					result = curve == AnimationCurve.Backward ? 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2)) :
						curve == AnimationCurve.Forward ? Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2)) :
						(x < 0.5 ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * x, 2))) / 2 : (Mathf.Sqrt(1 - Mathf.Pow(-2 * x + 2, 2)) + 1) / 2);
					break;
				}
			case Animation.Elastic:
				{
					result = curve == AnimationCurve.Backward ?
						(x == 0 ? 0 : x == 1 ? 1 : -Mathf.Pow(2, 10 * x - 10) * Mathf.Sin((x * 10 - 10.75f) * ((2 * Mathf.Pi) / 3))) :
						curve == AnimationCurve.Forward ?
						(x == 0 ? 0 : x == 1 ? 1 : Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10 - 0.75f) * (2 * Mathf.Pi) / 3) + 1) :
						(x == 0 ? 0 : x == 1 ? 1 : x < 0.5f ? -(Mathf.Pow(2, 20 * x - 10) * Mathf.Sin((20f * x - 11.125f) *
						(2 * Mathf.Pi) / 4.5f)) / 2f :
						(Mathf.Pow(2, -20 * x + 10) * Mathf.Sin((20 * x - 11.125f) * (2 * Mathf.Pi) / 4.5f)) / 2 + 1);
					break;
				}
			case Animation.Swing:
				{
					result = curve == AnimationCurve.Backward ? 2.70158f * x * x * x - 1.70158f * x * x :
						curve == AnimationCurve.Forward ? 1 + 2.70158f * Mathf.Pow(x - 1, 3) + 1.70158f * Mathf.Pow(x - 1, 2) :
						(x < 0.5 ? (Mathf.Pow(2 * x, 2) * ((2.59491f + 1) * 2 * x - 2.59491f)) / 2 :
						(Mathf.Pow(2 * x - 2, 2) * ((2.59491f + 1) * (x * 2 - 2) + 2.59491f) + 2) / 2);
					break;
				}
			case Animation.Bounce:
				{
					result = curve == AnimationCurve.Backward ? 1 - easeOutBounce(1 - x) :
						curve == AnimationCurve.Forward ? easeOutBounce(x) :
						(x < 0.5f ? (1 - easeOutBounce(1 - 2 * x)) / 2 : (1 + easeOutBounce(2 * x - 1)) / 2);
					break;
				}
		}
		return result;

		float easeOutBounce(float i)
		{
			return i < 1 / 2.75f ? 7.5625f * i * i : i < 2 / 2.75f ? 7.5625f * (i -= 1.5f / 2.75f) * i + 0.75f :
				i < 2.5f / 2.75f ? 7.5625f * (i -= 2.25f / 2.75f) * i + 0.9375f : 7.5625f * (i -= 2.625f / 2.75f) * i + 0.984375f;
		}
	}
	/// <summary>
	/// Restricts a <paramref name="number"/> in the inclusive range [<paramref name="rangeA"/> - <paramref name="rangeB"/>] with a certain type of
	/// <paramref name="limitation"/> and returns it. Also known as Clamping.<br></br><br></br>
	/// - Note when using <see cref="Limitation.Overflow"/>: <paramref name="rangeB"/> is not inclusive since <paramref name="rangeA"/> = <paramref name="rangeB"/>.
	/// <br></br>
	/// - Example for this: Range [0 - 10], (0 = 10). So <paramref name="number"/> = -1 would result in 9. Putting the range [0 - 11] would give the "real" inclusive
	/// [0 - 10] range.<br></br> Therefore <paramref name="number"/> = <paramref name="rangeB"/> would result in <paramref name="rangeA"/> but not vice versa.
	/// </summary>
	public static float Limit(this float number, float rangeA, float rangeB, Limitation limitation = Limitation.ClosestBound)
	{
		if(rangeA > rangeB)
			Swap(ref rangeA, ref rangeB);

		if(limitation == Limitation.ClosestBound)
		{
			if(number < rangeA)
				return rangeA;
			else if(number > rangeB)
				return rangeB;
			return number;
		}
		else if(limitation == Limitation.Overflow)
		{
			var d = rangeB - rangeA;
			return ((number - rangeA) % d + d) % d + rangeA;
		}
		return float.NaN;
	}
	/// <summary>
	/// Ensures a <paramref name="number"/> is <paramref name="signed"/> and returns the result.
	/// </summary>
	public static float Sign(this float number, bool signed)
	{
		return signed ? -Mathf.Abs(number) : Mathf.Abs(number);
	}
	/// <summary>
	/// Calculates <paramref name="number"/>'s precision (amount of digits after the decimal point) and returns it.
	/// </summary>
	public static int Precision(this float number)
	{
		var cultDecPoint = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
		var split = number.ToString().Split(cultDecPoint);
		return split.Length > 1 ? split[1].Length : 0;
	}
	/// <summary>
	/// Returns whether <paramref name="number"/> is in range [<paramref name="rangeA"/> - <paramref name="rangeB"/>].
	/// The ranges may be <paramref name="inclusiveA"/> or <paramref name="inclusiveB"/>.
	/// </summary>
	public static bool IsBetween(this float number, float rangeA, float rangeB, bool inclusiveA = false, bool inclusiveB = false)
	{
		if(rangeA > rangeB)
			Swap(ref rangeA, ref rangeB);
		var l = inclusiveA ? rangeA <= number : rangeA < number;
		var u = inclusiveB ? rangeB >= number : rangeB > number;
		return l && u;
	}
	/// <summary>
	/// Moves a <paramref name="number"/> in the direction of <paramref name="speed"/>. The result is then returned.
	/// </summary>
	public static float Move(this float number, float speed, float dt = 1)
	{
		speed *= dt;
		return number + speed;
	}
	/// <summary>
	/// Moves a <paramref name="number"/> toward a <paramref name="targetNumber"/> with <paramref name="speed"/>.
	/// The calculation ensures not to pass the <paramref name="targetNumber"/>. The result is then returned.
	/// </summary>
	public static float MoveToTarget(this float number, float targetNumber, float speed, float dt = 1)
	{
		var goingPos = number < targetNumber;
		var result = Move(number, goingPos ? Sign(speed, false) : Sign(speed, true), dt);

		if(goingPos && result > targetNumber)
			return targetNumber;
		else if(goingPos == false && result < targetNumber)
			return targetNumber;
		return result;
	}
	/// <summary>
	/// Maps a <paramref name="number"/> from one range to another ([<paramref name="a1"/> - <paramref name="a2"/>] to
	/// [<paramref name="b1"/> - <paramref name="b2"/>]) and returns it.<br></br>
	/// The <paramref name="b1"/> value is returned if the result is <see cref="float.NaN"/>,
	/// <see cref="float.NegativeInfinity"/> or <see cref="float.PositiveInfinity"/>.<br></br>
	/// - Example: 50 mapped from [0 - 100] and [0 - 1] results to 0.5<br></br>
	/// - Example: 25 mapped from [30 - 20] and [1 - 5] results to 3
	/// </summary>
	public static float Map(this float number, float a1, float a2, float b1, float b2)
	{
		var value = (number - a1) / (a2 - a1) * (b2 - b1) + b1;
		return float.IsNaN(value) || float.IsInfinity(value) ? b1 : value;
	}
	/// <summary>
	/// Rotates a 360 degrees <paramref name="angle"/> toward a <paramref name="targetAngle"/> with <paramref name="speed"/>
	/// taking the closest direction.
	/// The calculation ensures not to pass the <paramref name="targetAngle"/>. The result is then returned.
	/// </summary>
	public static float MoveToAngle(this float angle, float targetAngle, float speed, float dt = 1)
	{
		angle = Wrap(angle, 360);
		targetAngle = Wrap(targetAngle, 360);
		speed = Math.Abs(speed);
		var difference = angle - targetAngle;

		// stops the rotation with an else when close enough
		// prevents the rotation from staying behind after the stop
		var checkedSpeed = speed;
		checkedSpeed *= dt;
		if(Math.Abs(difference) < checkedSpeed) angle = targetAngle;
		else if(difference >= 0 && difference < 180) angle = Move(angle, -speed, dt);
		else if(difference >= -180 && difference < 0) angle = Move(angle, speed, dt);
		else if(difference >= -360 && difference < -180) angle = Move(angle, -speed, dt);
		else if(difference >= 180 && difference < 360) angle = Move(angle, speed, dt);

		// detects speed greater than possible
		// prevents jiggle when passing 0-360 & 360-0 | simple to fix yet took me half a day
		if(Math.Abs(difference) > 360 - checkedSpeed) angle = targetAngle;

		return angle;
	}
	/// <summary>
	/// Generates a random <see cref="float"/> number in the inclusive range [<paramref name="rangeA"/> - <paramref name="rangeB"/>] with
	/// <paramref name="precision"/> and an optional <paramref name="seed"/>. Then returns the result.
	/// </summary>
	public static float Random(this float rangeA, float rangeB, float precision = 0, float seed = float.NaN)
	{
		if(rangeA > rangeB)
			Swap(ref rangeA, ref rangeB);

		precision = (int)precision.Limit(0, 5);
		precision = Mathf.Pow(10, precision);

		rangeA *= precision;
		rangeB *= precision;

		var s = new Random(float.IsNaN(seed) ? Guid.NewGuid().GetHashCode() : (int)seed);
		var randInt = s.Next((int)rangeA, (int)rangeB + 1).Limit((int)rangeA, (int)rangeB);

		return randInt / (precision);
	}
	/// <summary>
	/// Returns true only <paramref name="percent"/>% / returns false (100 - <paramref name="percent"/>)% of the times.
	/// </summary>
	public static bool HasChance(this float percent)
	{
		percent = percent.Limit(0, 100);
		var n = Random(1f, 100f); // should not roll 0 so it doesn't return true with 0% (outside of roll)
		return n <= percent;
	}
	/// <summary>
	/// Converts a 360 degrees <paramref name="angle"/> into a normalized <see cref="Vector2"/> direction then returns the result.
	/// </summary>
	public static Vector2 ToDirection(this float angle)
	{
		//Angle to Radians : (Math.PI / 180) * angle
		//Radians to Vector2 : Vector2.x = cos(angle) ; Vector2.y = sin(angle)

		var rad = Mathf.Pi / 180 * angle;
		var dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

		return new Vector2(dir.x, dir.y);
	}
	/// <summary>
	/// Converts <paramref name="radians"/> to a 360 degrees angle and returns the result.
	/// </summary>
	public static float ToDegrees(this float radians)
	{
		return radians * (180f / Mathf.Pi);
	}
	/// <summary>
	/// Converts a 360 <paramref name="degrees"/> angle into radians and returns the result.
	/// </summary>
	public static float ToRadians(this float degrees)
	{
		return (Mathf.Pi / 180f) * degrees;
	}
	/// <summary>
	/// Converts a 360 degrees angle to a <see cref="Direction4"/> and returns it.
	/// </summary>
	public static Direction4 ToDirection4(this float angle)
	{
		var a = Mathf.Round((int)(angle.Wrap(360) / 90f));
		if(a >= Enum.GetNames(typeof(Direction4)).Length)
			a = 0;
		return (Direction4)a;

	}
	/// <summary>
	/// Converts a 360 degrees angle to a <see cref="Direction4"/> and returns it.
	/// </summary>
	public static Direction8 ToDirection8(this float angle)
	{
		var a = Mathf.Round((int)(angle.Wrap(360) / 45f));
		if(a >= Enum.GetNames(typeof(Direction8)).Length)
			a = 0;
		return (Direction8)a;
	}
	/// <summary>
	/// Converts a 360 degrees angle to a <see cref="DirectionH"/> and returns it.
	/// </summary>
	public static DirectionH ToDirectionH(this float angle)
	{
		var a = angle.Wrap(360);
		return a.IsBetween(90, 270) ? DirectionH.Left : DirectionH.Right;
	}
	/// <summary>
	/// Converts a 360 degrees angle to a <see cref="DirectionV"/> and returns it.
	/// </summary>
	public static DirectionV ToDirectionV(this float angle)
	{
		var a = angle.Wrap(360);
		return a.IsBetween(0, 180) ? DirectionV.Down : DirectionV.Up;
	}

	/// <summary>
	/// Returns whether this <paramref name="vector"/> is invalid.
	/// </summary>
	public static bool IsNaN(this Vector2 vector)
	{
		return float.IsNaN(vector.x) || float.IsNaN(vector.y);
	}
	/// <summary>
	/// Returns an invalid <see cref="Vector2"/>.
	/// </summary>
	public static Vector2 NaN(this Vector2 vector)
	{
		return new Vector2(float.NaN, float.NaN);
	}
	/// <summary>
	/// Converts a directional <see cref="Vector2"/> into a 360 degrees angle and returns the result.
	/// </summary>
	public static float ToAngle(this Vector2 direction)
	{
		//Vector2 to Radians: atan2(Vector2.y, Vector2.x)
		//Radians to Angle: radians * (180 / Math.PI)

		var rad = Mathf.Atan2(direction.y, direction.x);
		var result = rad * (180f / Mathf.Pi);
		return result;
	}
	/// <summary>
	/// Calculates the 360 degrees angle between two <see cref="Vector2"/> points and returns it.
	/// </summary>
	public static float Angle(this Vector2 point, Vector2 targetPoint)
	{
		return ToAngle(targetPoint - point).Wrap(360);
	}
	/// <summary>
	/// Snaps a <paramref name="point"/> to the closest grid cell according to <paramref name="gridSize"/> and returns the result.
	/// </summary>
	public static Vector2 ToGrid(this Vector2 point, Vector2 gridSize)
	{
		if(gridSize == default)
			return point;

		// this prevents -0 cells
		point.x -= point.x < 0 ? gridSize.x : 0;
		point.y -= point.y < 0 ? gridSize.y : 0;

		point.x -= point.x % gridSize.x;
		point.y -= point.y % gridSize.y;
		return point;
	}
	/// <summary>
	/// Calculates the direction between <paramref name="point"/> and <paramref name="targetPoint"/>. The result may be
	/// <paramref name="normalized"/> (see <see cref="Normalize"/> for info). Then it is returned.
	/// </summary>
	public static Vector2 DirectionBetweenPoints(this Vector2 point, Vector2 targetPoint, bool normalized = true)
	{
		return normalized ? (targetPoint - point).Normalized() : targetPoint - point;
	}
	/// <summary>
	/// Moves a <paramref name="point"/> in <paramref name="direction"/> with <paramref name="speed"/>. The result is then returned.
	/// </summary>
	public static Vector2 MoveInDirection(this Vector2 point, Vector2 direction, float speed, float dt = 1)
	{
		point.x += direction.x * speed * dt;
		point.y += direction.y * speed * dt;
		return new Vector2(point.x, point.y);
	}
	/// <summary>
	/// Moves a <paramref name="point"/> at a 360 degrees <paramref name="angle"/> with <paramref name="speed"/>. The result is then returned.
	/// </summary>
	public static Vector2 MoveAtAngle(this Vector2 point, float angle, float speed, float dt = 1)
	{
		var result = MoveInDirection(point, angle.Wrap(360).ToDirection().Normalized(), speed, dt);
		return result;
	}
	/// <summary>
	/// Moves a <paramref name="point"/> toward <paramref name="targetPoint"/> with <paramref name="speed"/>. The calculation ensures not to pass the
	/// <paramref name="targetPoint"/>. The result is then returned.
	/// </summary>
	public static Vector2 MoveToTarget(this Vector2 point, Vector2 targetPoint, float speed, float dt = 1)
	{
		var result = point.MoveAtAngle(point.Angle(targetPoint), speed, dt);

		speed *= dt;
		return result.DistanceTo(targetPoint) < speed * 1.1f ? targetPoint : result;
	}
	/// <summary>
	/// Calculates the <see cref="Vector2"/> point that is a certain <paramref name="percent"/> between <paramref name="point"/> and
	/// <paramref name="targetPoint"/> then returns the result. Also known as Lerping (linear interpolation).
	/// </summary>
	public static Vector2 PercentToTarget(this Vector2 point, Vector2 targetPoint, Vector2 percent)
	{
		point.x = percent.x.Map(0, 100, point.x, targetPoint.x);
		point.y = percent.y.Map(0, 100, point.y, targetPoint.y);
		return point;
	}
	/// <summary>
	/// Converts a directional <see cref="Vector2"/> to a <see cref="Direction4"/> and returns it.
	/// </summary>
	public static Direction4 ToDirection4(this Vector2 direction)
	{
		return direction.ToAngle().ToDirection4();
	}
	/// <summary>
	/// Converts a directional <see cref="Vector2"/> to a <see cref="Direction8"/> and returns it.
	/// </summary>
	public static Direction8 ToDirection8(this Vector2 direction)
	{
		return direction.ToAngle().ToDirection8();
	}
	/// <summary>
	/// Converts a directional <see cref="Vector2"/> to a <see cref="DirectionH"/> and returns it.
	/// </summary>
	public static DirectionH ToDirectionH(this Vector2 direction)
	{
		return direction.ToAngle().ToDirectionH();
	}
	/// <summary>
	/// Converts a directional <see cref="Vector2"/> to a <see cref="DirectionV"/> and returns it.
	/// </summary>
	public static DirectionV ToDirectionV(this Vector2 direction)
	{
		return direction.ToAngle().ToDirectionV();
	}

	/// <summary>
	/// Generates a random <see cref="int"/> number in the inclusive range [<paramref name="rangeA"/> - <paramref name="rangeB"/>] with an
	/// optional <paramref name="seed"/>. Then returns the result.
	/// </summary>
	public static int Random(this int rangeA, int rangeB, float seed = float.NaN)
	{
		return (int)Random(rangeA, rangeB, 0, seed);
	}
	/// <summary>
	/// Returns true only <paramref name="percent"/>% / returns false (100 - <paramref name="percent"/>)% of the times.
	/// </summary>
	public static bool HasChance(this int percent)
	{
		return HasChance((float)percent);
	}
	/// <summary>
	/// Restricts a <paramref name="number"/> in the inclusive range [<paramref name="rangeA"/> - <paramref name="rangeB"/>] with a certain type of
	/// <paramref name="limitation"/> and returns it. Also known as Clamping.<br></br><br></br>
	/// - Note when using <see cref="Limitation.Overflow"/>: <paramref name="rangeB"/> is not inclusive since <paramref name="rangeA"/> = <paramref name="rangeB"/>.
	/// <br></br>
	/// - Example for this: Range [0 - 10], (0 = 10). So <paramref name="number"/> = -1 would result in 9. Putting the range [0 - 11] would give the "real" inclusive
	/// [0 - 10] range.<br></br> Therefore <paramref name="number"/> = <paramref name="rangeB"/> would result in <paramref name="rangeA"/> but not vice versa.
	/// </summary>
	public static int Limit(this int number, int rangeA, int rangeB, Limitation limitation = Limitation.ClosestBound)
	{
		return (int)Limit((float)number, rangeA, rangeB, limitation);
	}
	/// <summary>
	/// Ensures a <paramref name="number"/> is <paramref name="signed"/> and returns the result.
	/// </summary>
	public static int Sign(this int number, bool signed)
		=> (int)Sign((float)number, signed);
	/// <summary>
	/// Returns whether <paramref name="number"/> is in range [<paramref name="rangeA"/> - <paramref name="rangeB"/>].
	/// The ranges may be <paramref name="inclusiveA"/> or <paramref name="inclusiveB"/>.
	/// </summary>
	public static bool IsBetween(this int number, int rangeA, int rangeB, bool inclusiveA = false, bool inclusiveB = false)
		=> IsBetween((float)number, rangeA, rangeB, inclusiveA, inclusiveB);
	/// <summary>
	/// Maps a <paramref name="number"/> from [<paramref name="A1"/> - <paramref name="B1"/>] to
	/// [<paramref name="B1"/> - <paramref name="B2"/>] and returns it. Similar to Lerping (linear interpolation).<br></br>
	/// - Example: 50 mapped from [0 - 100] and [0 - 1] results to 0.5<br></br>
	/// - Example: 25 mapped from [30 - 20] and [1 - 5] results to 3
	/// </summary>
	public static int Map(this int number, int a1, int a2, int b1, int b2)
		=> (int)Map((float)number, a1, a2, b1, b2);
		
	public static Node SpawnNodeOn(Node parentNode, string nodePath)
	{
		var nodeType = GD.Load<PackedScene>(nodePath);
		var newNode = nodeType.Instance();
		parentNode.AddChild(newNode);
		return newNode;
	}

	#region Backend
	private static readonly Dictionary<string, int> gateEntries = new Dictionary<string, int>();
	private static readonly Dictionary<string, bool> gates = new Dictionary<string, bool>();
	#endregion
}
