namespace Project.UI
{
    public static class ModHelper
    {
        public static string ValueToString(int value)
        {
            if (value == 0)
                return "0";
            
            return value > 0 ? $"+{value}" : value.ToString();
        }
    }
}