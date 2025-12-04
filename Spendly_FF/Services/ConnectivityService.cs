using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spendly_FF.Services
{
    public class ConnectivityService : IConnectivityService
    {
        public ConnectivityService()
        {
            // Feliratkozás a MAUI beépített eseményére
            Connectivity.Current.ConnectivityChanged += (sender, e) =>
                ConnectivityChanged?.Invoke(this, e);
        }

        public NetworkAccess NetworkAccess => Connectivity.Current.NetworkAccess;

        public event EventHandler<ConnectivityChangedEventArgs> ConnectivityChanged;
    }
}
