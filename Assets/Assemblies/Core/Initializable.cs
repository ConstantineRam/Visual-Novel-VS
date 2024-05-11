using System;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable once IdentifierTypo
public interface Initializable
{
    bool IsInitialized { get; }
    
    Task Initialize(object data, CancellationToken ct);
}
