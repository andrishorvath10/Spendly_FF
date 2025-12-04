using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spendly_FF.Services
{
    public interface IConnectivityService
    {
        NetworkAccess NetworkAccess { get; }
        event EventHandler<ConnectivityChangedEventArgs> ConnectivityChanged;
    }
}
