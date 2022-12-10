using AsyncCallback = System.Func<System.Threading.Tasks.Task>;
using SyncCallback = System.Action;
namespace Mafmax.FileConverter.Utils.Proxies;

/// <summary>
/// Proxy for stream that can set callback after stream disposes.
/// </summary>
public class CallbackStreamProxy : DelegatingStream
{
    private bool _closed;
    private AsyncCallback _asyncCallback = () => Task.CompletedTask;
    private SyncCallback _syncCallback = () => { };

    /// <inheritdoc />
    public CallbackStreamProxy(Stream stream) : base(stream) { }

    /// <summary>
    /// Sets async callback that invokes after <see cref="DisposeAsync"/> method.
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    public CallbackStreamProxy SetAsyncCallback(AsyncCallback callback)
    {
        _asyncCallback = callback;

        return this;
    }

    /// <summary>
    /// Sets callback that invokes after <see cref="Dispose"/> method.
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    public CallbackStreamProxy SetSyncCallback(SyncCallback callback)
    {
        _syncCallback = callback;

        return this;
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        if (!_closed)
        {
            await _asyncCallback().ConfigureAwait(false);
            _closed = true;
        }
        await base.DisposeAsync();
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (!_closed)
        {
            _syncCallback();
            _closed = true;
        }
        base.Dispose(disposing);
    }
}
