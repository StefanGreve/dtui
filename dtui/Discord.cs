namespace dtui
{
    public class Discord
    {
        public static async Task<bool> Login(string username, string password, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // issue #3: implement authentication process here
                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
                return !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);
            }

            cancellationToken.ThrowIfCancellationRequested();
            return false;
        }
    }
}
