namespace Toxic
{
    public class Is
    {
        public static Threashold GreaterThanOrEqualTo(int limit)
        {
            return value => value.ToDouble() >= limit;
        }
    }
}