namespace Intcrementor
{
    public static class Validators
    {
        public static int ValidateRange(int value, int min, int max)
        {
            if (value < min) { return min; }
            if (value > max) { return max; }
            return value;
        }
    }
}
