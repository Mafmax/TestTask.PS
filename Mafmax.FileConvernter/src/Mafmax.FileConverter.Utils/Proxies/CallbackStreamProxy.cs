using AsyncCallback = System.Func<System.Threading.Tasks.Task>;
using SyncCallback = System.Action;
namespace Mafmax.FileConverter.Utils.Proxies;

public class CallbackStreamProxy : DelegatingStream
{
    private bool _closed = false;
    private AsyncCallback _asyncCallback = () => Task.CompletedTask;
    private SyncCallback _syncCallback = () => { };

    /// <inheritdoc />
    public CallbackStreamProxy(Stream stream) : base(stream) { }

    public CallbackStreamProxy SetAsyncCallback(AsyncCallback callback)
    {
        _asyncCallback = callback;

        return this;
    }

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
