
using GdUnit4;
using static GdUnit4.Assertions;
using System.Collections.Generic;

using Godot.Collections;

using static CustomMath.CustomMath;



[TestSuite]
public class TestCustomMathCs
{
	

	[TestCase(1,0,2, true)]
	[TestCase(0,1,2, false)]
	[TestCase(1,int.MinValue,int.MaxValue, true)]
	[TestCase(int.MinValue,1,int.MaxValue, false)]
	[TestCase(int.MaxValue,int.MinValue,1, false)]
	public void TestInRange(int testValue, int rangeStart, int rangeEnd, bool expect ) {
		AssertThat(IsInRange(testValue, rangeStart, rangeEnd)).Equals(expect);
	}
		HashSet<double> hash = new HashSet<double> {0.0, 2.0};

	[TestCase(1,0,2, new double[2] {1, 2})]
	[TestCase(0,1,2, new double[2] {0, 0})]
	[TestCase(0,double.MinValue,double.MaxValue, new double[2] {double.MinValue, double.MaxValue})]
	public void TestUnclamp(double testValue, double rangeStart, double rangeEnd, double[] expect)
	{
		double result = Unclamp(testValue, rangeStart, rangeEnd);
		bool isInExpect = false;
		
		foreach (double value in expect)
		    if (result == value) { isInExpect = true; break; }

		AssertBool(isInExpect).IsTrue();
	}


}

