namespace dtui
{
    public class Discord
    {
        public static async Task<bool> Login(string? username, string? password, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);

            if (cancellationToken.IsCancellationRequested)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            return !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);
        }
    }
}
