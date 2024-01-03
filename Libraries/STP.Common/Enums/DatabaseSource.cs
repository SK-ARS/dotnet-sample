#region

using System;

#endregion

namespace STP.Common.Enums
{
    /// <summary>
    ///     Provides constants for all types of DBsources one could connect to
    ///     Add List of Databases
    /// </summary>
    [CLSCompliant(false)]
    public enum DatabaseSource
    {
        SQL = 1,
        ORACLE=2,
        OLEDB=3
    }
}