using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dtui
{
    public class Discord
    {
        public static async Task<bool> Login(string? username, string? password)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            bool valid = !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);
            Console.WriteLine($"u:{username}\tp:{password}\t(valid: {valid})");
            return valid;
        }
    }
}
