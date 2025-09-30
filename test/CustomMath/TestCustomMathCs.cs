
using GdUnit4;
using Godot;

using static GdUnit4.Assertions;


[TestSuite]
public class TestCustomMathCs
{
	private GodotObject CustomMath = (GodotObject)GD.Load<GDScript>("res://components/math.gd").New();

	[TestCase]
	public void InRange() {
		bool inRange;
		inRange = (bool)CustomMath.Call("is_in_range", [1, 0, 2] );
		AssertBool(inRange).IsTrue();
	}

}

