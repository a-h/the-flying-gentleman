using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace FlyingGentleman.Library.DatabaseServer
{
	/// <summary>
	/// Allows the installation and configuration of SQL Server.
	/// </summary>
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ILogEventCreator))]
    public interface IDatabaseServer
    {
        /// <summary>
        /// Creates the database server. Calls a static "CreateDatabase" method on the specified entities type 
        /// in the specified dll, passing as the only the parameter the specified database name.
        /// </summary>
        /// <param name="settings">The settings</param>
        [OperationContract]
        void DropAndCreateDatabase(CreateSettings settings);

        /// <summary>
        /// Executes arbitrary SQL against the database
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="commandText">The command text.</param>
        [OperationContract]
        void ExecuteSql(string serverName, string databaseName, string commandText);

        /// <summary>
        /// Creates a login with the specified name, and user for that login on the specified database, if they don't exist
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="loginName">Name of the login.</param>
        [OperationContract]
        void CreateLoginAndUserIfNotExists(string serverName, string databaseName, string loginName);

        /// <summary>
        /// Adds the data reader and writer roles to the specified user on the specified database
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="loginName">Name of the login.</param>
        [OperationContract]
        void AddDataReaderAndWriterRoles(string serverName, string databaseName, string loginName);

        /// <summary>
        /// Adds the database owner role to the specified user on the specified database
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="loginName">Name of the login.</param>
        [OperationContract]
        void AddDatabaseOwnerRole(string serverName, string databaseName, string loginName);

        /// <summary>
        /// Migrates the database to the latest version based on the migrations created in the entities project.
        /// </summary>
        /// <param name="settings">The migration settings.</param>
        [OperationContract]
        void MigrateDatabase(MigrateSettings settings);
    }
}
