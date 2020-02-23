using System;

namespace MiniTwit.Utils
{
    public static class Misc
    {
        public static bool IsDevelopment()
        {
            return string.Equals(
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                "development", 
                StringComparison.InvariantCultureIgnoreCase
            );
        }
    }
}