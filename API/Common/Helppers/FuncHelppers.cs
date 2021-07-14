using System;

namespace API.Services
{
    public static class FuncHelppers
    {
        public static string GenerateGUID() => Guid.NewGuid().ToString().ToUpper();
    }
}
