
using GdUnit4;
using static GdUnit4.Assertions;

// using Godot;

using static CustomMath.CustomMath;



[TestSuite]
public class TestCustomMathCs
{
	// private GodotObject CustomMath = (GodotObject)GD.Load<GDScript>("res://components/math.gd").New();
	

	[TestCase]
	public void InRange() {
		bool inRange;
		inRange = IsInRange(1, 0, 2);
		AssertBool(inRange).IsTrue();
	}

}

