namespace concurrent_cancelation;

class Program
{
    static async Task Main(string[] args)
    {
        using CancellationTokenSource cancellation = new();
        cancellation.CancelAfter(450);

        try
        {
            for (var i = 1; i <= 5; i++)
            {
                await DownloadChunkAsync(cancellation.Token);
                Console.WriteLine($"Downloaded chunk {i}");
            }

            Console.WriteLine("Download complete");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Download canceled");
        }
    }

    static async Task DownloadChunkAsync(CancellationToken token)
    {
        // Simulate waiting for I/O; cancellation can interrupt the wait.
        await Task.Delay(200, token);
    }
}
