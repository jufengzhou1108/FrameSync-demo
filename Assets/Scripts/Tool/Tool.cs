public static class Tool
{
    public static int FloatToInt(float f)
    {
        return (int)(f * 1000);
    }

    public static int ClampInt(int value,int left,int right)
    {
        if (left > value)
        {
            value = left;
        }

        if (right < value)
        {
            value = right;
        }

        return value;
    }
}