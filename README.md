# concurrent-cancelation

## Week 6 - Concurrency: Add cancellation to a simulated download

**Time**: 30-45 minutes
**Dependencies**: .NET SDK; no external packages.

This week's main concept is cooperative cancellation. Use a simulated download so you can focus on cancellation without networking setup. This stands alone from earlier exercises.

### Coding task

Build a console app with this helper:

```cs
static async Task DownloadChunkAsync(CancellationToken token)
{
    // Simulate waiting for I/O; cancellation can interrupt the wait.
    await Task.Delay(200, token);
}
```

### Minimal working checkpoint -- first 10-15 minutes

Call the helper five times in a loop, awaiting each call and passing `CancellationToken.None`. After each completed call, print `Downloaded chunk N`. Print `Download complete` after all give chunks.

Keep the calls sequential: the goal is controlling one operation's lifetime.

### Add cancellation -- remaining 20-30 minutes

Create a cancellation source:

```cs
using var cancellation = new CancellationTokenSource();
cancellation.CancelAfter(450); // Request cancellation after roughly 450 ms.
// Pass cancellation.Token to each DownloadChunkAsync call.
```

Wrap the download loop in `try`/`catch`. Hanlde `OperationCanceledException` by printing `Download canceled`, and ensure `Download complete` appears only when every chunk finishes.

Run once with automatic cancellation enabled, then again with the `CancelAfter` call commented out. Keep inputs fixed at five chunks and 200 milliseconds per chunk. No command-line parsing or additional error handling is required.

### Acceptance criteria

- Without cancellation, all five chunks messages appear, followed by `Download complete`.
- With cancellation, the operation reports cancellation without an unhandled exception or a completion message.
- Chunk messages appear only after their simulated waits finish; don't require an exact number before cancellation because scheduing can vary.

### Underlying concept

A cancellation token communicates a request; it does not forcibly terminate code. Operations must observe that token. Here, `Task.Delay` observes it and signals cancellation through an exception when awaited. Passing the token through helper methods keeps that cooperation intact.

*Optional stretch question*: If you removed the token argument from `Task.Delay` but still created the cancellation source, what behavior would change--any why?
