namespace Julian.Extention
{
    public static class ExtentionMethods
    {
        public static float RemapAlpha(this float value, float from, float to)
        {
            return Remap(value, 0f, 1f, from, to);
        }

        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static int ClampContinue(this int value, int min, int max)
        {
            if (value < min)
                return max - 1;
            else if (value >= max)
                return min;

            return value;
        }
    }
}

