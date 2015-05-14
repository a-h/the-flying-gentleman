using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyingGentleman.Library.DatabaseServer;
using FlyingGentleman.Library;

namespace FlyingGentleman.Mocks.DatabaseServer
{
	public class DatabaseServer : MockBase, IDatabaseServer
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DatabaseServer"/> class.
		/// </summary>
		/// <param name="a">The action to use to log the event.</param>
		public DatabaseServer(Action<LogEvent> a)
		{
			this.Action = a;

		}

		/// <summary>
		/// Installs the database server.
		/// </summary>
		/// <param name="settings"></param>
		public void Install(InstallSettings settings)
		{
			LogEventHelper.CreateEvent(this.Action, this, "Install", "Installing...");
		}

		/// <summary>
		/// Enables access to the server over TCP/IP.
		/// </summary>
		/// <param name="instanceName">The name of the instance.  The use of NULL or string.Empty will result in the default MSSQLSERVER instance name being used.</param>
		public void EnableTcpIp(string instanceName)
		{
			LogEventHelper.CreateEvent(this.Action, this, "EnableTcpIp", "Enabling TCP/IP on instance: " + instanceName ?? "Default");
		}

        /// <summary>
        /// Creates the database server. Calls a static "CreateDatabase" method on the specified entities type
        /// in the specified dll, passing as the only the parameter the specified database name.
        /// </summary>
        /// <param name="settings">The settings</param>
        public void DropAndCreateDatabase(CreateSettings settings)
        {
            LogEventHelper.CreateEvent(this, "CreateDatabase", string.Format("Creating database {0}, from entities {1}, loaded from {2}", settings.DatabaseName, settings.EntitiesTypeName, settings.EntitiesDllLocation));
        }

        /// <summary>
        /// Executes arbitrary SQL against the database
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="commandText">The command text.</param>
        public void ExecuteSql(string serverName, string databaseName, string commandText)
        {
            LogEventHelper.CreateEvent(this, "ExecuteSql", string.Format("Executing SQL on server {0} against database {1}\n{2}{3}", serverName, databaseName, commandText.Substring(0, Math.Min(commandText.Length, 100)), (commandText.Length > 100) ? "..." : ""));
        }
        
        /// <summary>
        /// Creates a login with the specified name, and user for that login on the specified database, if they don't exist
        /// </summary>
        /// <param name="loginName">Name of a windows login.</param>
        /// <param name="databaseName">Name of the database.</param>
        public void CreateLoginAndUserIfNotExists(string serverName, string databaseName, string loginName)
        {
            LogEventHelper.CreateEvent(this, "CreateLoginAndUserIfNotExists", string.Format("Creating a login on server {0} for database {1} and login {2}", serverName, databaseName, loginName));
        }


        /// <summary>
        /// Adds the data reader and writer roles to the specified user on the specified database
        /// </summary>
        /// <param name="loginName">Name of a windows login.</param>
        /// <param name="databaseName">Name of the database.</param>
        public void AddDataReaderAndWriterRoles(string serverName, string databaseName, string loginName)
        {
            LogEventHelper.CreateEvent(this, "AddDataReaderAndWriterRoles", string.Format("Adding datareader and datawriter roles on server {0} for database {1} and login {2}", serverName, databaseName, loginName));
        }

        /// <summary>
        /// Adds the database owner role to the specified user on the specified database
        /// </summary>
        /// <param name="loginName">Name of a windows login.</param>
        /// <param name="databaseName">Name of the database.</param>
        public void AddDatabaseOwnerRole(string serverName, string databaseName, string loginName)
        {
            LogEventHelper.CreateEvent(this, "AddDatabaseOwnerRole", string.Format("Adding database owner role on server {0} for database {1} and login {2}", serverName, databaseName, loginName));
        }


        #region IDatabaseServer Members


        public void MigrateDatabase(MigrateSettings settings)
        {
            LogEventHelper.CreateEvent(this, "MigrateDatabase", string.Format("Migrating database {0} on server {1}, from entities {2}, loaded from {3}", settings.DatabaseName, settings.ServerName, settings.EntitiesTypeName, settings.EntitiesDllLocation));
        }

        #endregion
    }
}
