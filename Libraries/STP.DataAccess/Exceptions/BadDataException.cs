namespace STP.DataAccess.Exceptions
{
    public class BadDataException : SafeProcedureDataException
    {
        public BadDataException(string message) : base(message)
        {
        }

        public BadDataException(string message, string dbInstanceName)
            : base(message)
        {
            DBInstanceName = dbInstanceName;
        }

        public BadDataException(string message, string dbInstanceName, string procedureName)
            : base(message)
        {
            DBInstanceName = dbInstanceName;
            this.procedureName = procedureName;
        }
    }
}