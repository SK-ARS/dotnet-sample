#region

using STP.Common.Configuration.Provider;

#endregion

namespace STP.Common.Configuration.Interface
{
    public interface IDatabaseConfigProvider
    {
        int CommandTimeout { get; }
        int VarcharMax { get; }
        DatabaseGroupCollection DatabaseGroupCollectionItems { get; }
        string ConnectionString(string server, string subDataBase);
    }
}