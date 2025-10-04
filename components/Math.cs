namespace CustomMath;

class CustomMath
{
    static public bool IsInRange(double value, double min, double max, bool minStrict = false, bool maxStrict = false)
    {
        bool isAboveMin;
        bool isBelowMin;

        if (minStrict) isAboveMin = value > min;
        else isAboveMin = value >= min;

        if (maxStrict) isBelowMin = value < max;
        else isBelowMin = value <= max;

        return isAboveMin && isBelowMin;
    }

    static public double Unclamp(double value, double min, double max)
    {
        if (!IsInRange(value, min, max)) return value;
        double diffMedian = (max - min) / 2 + min;
        if (value == diffMedian)
        {
            System.Random rand = new System.Random();
            value -= (rand.NextDouble() - 0.5);
        }
        if (value < diffMedian) return min;
        return max;
    }
}
