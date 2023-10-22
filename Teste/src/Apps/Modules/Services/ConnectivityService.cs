using Core.Modules.Services;

namespace Apps.Modules.Services
{
    public class ConnectivityService : IConnectivityService
    {
        public bool IsConnected() => Connectivity.NetworkAccess == NetworkAccess.Internet;
    }
}
