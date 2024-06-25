namespace DataAccessLibrary.Databases
{
    public interface ISqlDataAccess
    {
        List<T> LoadData<T, U>(string sqlStatement,
                               U paramters,
                               string connectionStringName,
                               bool isStroredProcedure = false);

        void SaveData<T>(string sqlStatement,
                         T paramters,
                         string connectionStringName,
                         bool isStroredProcedure = false);
    }
}