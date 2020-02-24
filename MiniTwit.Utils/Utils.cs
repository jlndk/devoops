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

        public static bool RunsInDocker()
        {
            return string.Equals(
                Environment.GetEnvironmentVariable("MINITWIT_ENVIRONMENT"),
                "docker",
                StringComparison.InvariantCultureIgnoreCase
            );
        }
    }
}