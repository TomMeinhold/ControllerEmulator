namespace ControllerEmulatorAPI.Extensions
{
    public static class MathExtensions
    {
        public static float Amplify(this int x, float y)
        {
            if (y == 0)
            {
                return x;
            }

            if (x > 1)
            {
                return x + y;
            }

            if (x < 1)
            {
                return x + (y * -1);
            }

            return x;
        }

        public static int Normalize(this int x)
        {
            if (x > 32000)
            {
                return 32000;
            }
            else if (x < 0)
            {
                return 0;
            }
            else
            {
                return x;
            }
        }
    }
}