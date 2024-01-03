#region

using System;

#endregion

namespace STP.Common.Configuration.Interface
{
    public interface IBuild
    {
        string BuildVersion { get; }
        Version Version { get; }
        string ImageRuntimeVersion { get; }
        bool IsRelease { get; }
        Version TypedVersion();
    }
}