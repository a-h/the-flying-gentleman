using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyingGentleman.Library.DatabaseServer;
using FlyingGentleman.Library;
using FlyingGentleman.RemoteServerLibrary.CommandLine;
using System.IO;
using System.Net;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Wmi;
using System.ServiceProcess;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading;

namespace FlyingGentleman.RemoteServerLibrary.DatabaseServer
{
	/// <summary>
	/// Allows the installation and configuration of SQL Server.
	/// </summary>
	[Serializable]
	public class DatabaseServer : LogEventCreatorBase, IDatabaseServer
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DatabaseServer"/> class.
		/// </summary>
		public DatabaseServer()
		{
		}


		/// <summary>
        /// Creates the database server. Calls a static "DropAndCreateDatabase" method on the specified entities type 
        /// in the specified dll, passing as the only the parameter the specified database name.
        /// </summary>
        /// <param name="settings">The settings</param>
        public void DropAndCreateDatabase(CreateSettings settings)
        {
            LogEventHelper.CreateEvent(this, "DropAndCreateDatabase", string.Format("Creating database {0} on server {1}, from entities {2}, loaded from {3}", settings.DatabaseName, settings.ServerName, settings.EntitiesTypeName, settings.EntitiesDllLocation));

            ExecuteEntityMethod(settings);
        }

        /// <summary>
        /// Creates the database server. Calls a static "DropAndCreateDatabase" method on the specified entities type 
        /// in the specified dll, passing as the only the parameter the specified database name.
        /// </summary>
        /// <param name="settings">The settings</param>
        public void MigrateDatabase(MigrateSettings settings)
        {
            LogEventHelper.CreateEvent(this, "MigrateDatabase", string.Format("Migrating database {0} on server {1}, from entities {2}, loaded from {3}", settings.DatabaseName, settings.ServerName, settings.EntitiesTypeName, settings.EntitiesDllLocation));

            ExecuteEntityMethod(settings);
        }

        private void ExecuteEntityMethod(ModifySettings settings)
        {

            //create an appdomain to load the entities dll and all its dependencies. we can then safely unload
            //this once we've finished, so no worries about trying to load different versions of the 
            var entitiesDomain = AppDomain.CreateDomain("EntitiesDomain", null, AppDomain.CurrentDomain.SetupInformation, new System.Security.PermissionSet(System.Security.Permissions.PermissionState.Unrestricted));
            
            try
            {
                entitiesDomain.SetData("CreateSettings", new[] { 
                    settings.EntitiesDllLocation, 
                    settings.EntitiesTypeName, 
                    settings.ServerName, 
                    settings.DatabaseName,
                    settings.MethodName
                });

                entitiesDomain.DoCallBack(() =>
                {
                    var createSettings = AppDomain.CurrentDomain.GetData("CreateSettings") as string[];
                    var assembly = Assembly.LoadFrom(createSettings[0]);

                    //the next call, to assembly.GetType will cause the referenced EntityFramework assembly to be resolved
                    AppDomain.CurrentDomain.AssemblyLoad += new AssemblyVerifier(assembly).VerifyEntitiesVersion;

                    var entitiesType = assembly.GetType(createSettings[1], true);
                    var method = entitiesType.GetMethod(createSettings[4], BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy, null, new[] { typeof(string), typeof(string) }, null);
                    if (method == null)
                    {
                        throw new Exception(string.Format("No public static {0}(string, string) method found on type {1}",
                            createSettings[4], entitiesType.AssemblyQualifiedName));
                    }
                    try
                    {
                        method.Invoke(null, new object[] { createSettings[2], createSettings[3] });
                    }
                    catch (TargetInvocationException tie)
                    {
                        throw tie.InnerException;
                    }
                });


                LogEventHelper.CreateEvent(this, settings.MethodName, "Done making database changes");
            }
            finally
            {
                //make sure we unload the appdomain if we have an exception
                AppDomain.Unload(entitiesDomain);
            }
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
            
            using (SqlConnection conn = new SqlConnection(string.Format("Data Source={0};Initial Catalog={1};Integrated Security=SSPI;", serverName, databaseName)))
            using (SqlCommand command = new SqlCommand(commandText, conn))
            {
                conn.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Creates a login with the specified name, and user for that login on the specified database, if they don't exist
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="loginName">Name of a windows login.</param>
        public void CreateLoginAndUserIfNotExists(string serverName, string databaseName, string loginName)
        {
            LogEventHelper.CreateEvent(this, "CreateLoginAndUserIfNotExists", string.Format("Creating a login on server {0} for database {1} and login {2}", serverName, databaseName, loginName));
            var script = new StringBuilder();
            script.AppendFormat("IF  NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'{0}')", loginName).AppendLine();
            script.AppendFormat("BEGIN").AppendLine();
            script.AppendFormat("    CREATE LOGIN [{0}] FROM WINDOWS ", loginName).AppendLine();
            script.AppendFormat("    WITH DEFAULT_DATABASE=[{0}], ", databaseName).AppendLine();
            script.AppendFormat("    DEFAULT_LANGUAGE=[us_english]").AppendLine();
            script.AppendFormat("END").AppendLine();
            script.AppendFormat("").AppendLine();
            script.AppendFormat("USE [{0}]", databaseName).AppendLine();
            script.AppendFormat("IF  NOT EXISTS (SELECT * from sys.database_principals WHERE sid = SUSER_SID(N'{0}'))", loginName).AppendLine();
            script.AppendFormat("BEGIN").AppendLine();
            script.AppendFormat("    CREATE USER [{0}] FOR LOGIN [{0}]", loginName).AppendLine();
            script.AppendFormat("END").AppendLine();
            var commandText = script.ToString();

            using (SqlConnection conn = new SqlConnection(string.Format("Data Source={0};Initial Catalog={1};Integrated Security=SSPI;", serverName, databaseName)))
            using (SqlCommand command = new SqlCommand(commandText, conn))
            {
                conn.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Adds the data reader and writer roles to the specified user on the specified database
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="loginName">Name of a windows login.</param>
        public void AddDataReaderAndWriterRoles(string serverName, string databaseName, string loginName)
        {
            LogEventHelper.CreateEvent(this, "AddDataReaderAndWriterRoles", string.Format("Adding datareader and datawriter roles on server {0} for database {1} and login {2}", serverName, databaseName, loginName));

            using (SqlConnection conn = new SqlConnection(string.Format("Data Source={0};Initial Catalog={1};Integrated Security=SSPI;", serverName, databaseName)))
            {
                conn.Open();
                var script = string.Format("SELECT TOP 1 name from sys.database_principals WHERE sid = SUSER_SID(N'{0}')", loginName);
                string userName;

                using (SqlCommand command = new SqlCommand(script, conn))
                using (var reader = command.ExecuteReader())
                {   
                    if (reader.Read())
                    {
                        userName = reader.GetString(0);
                    }
                    else
                    { 
                        throw new Exception(string.Format("No user found in database for windows login {0}", loginName));
                    }
                }

                if (userName == "dbo")
                {
                    LogEventHelper.CreateEvent(this, "AddDataReaderAndWriterRoles", string.Format("Database user for loginName {0} is dbo, not adding to roles.", loginName));
                }
                else
                {
                    LogEventHelper.CreateEvent(this, "AddDataReaderAndWriterRoles", string.Format("Database user for loginName {0} is {1}", loginName, userName));
                    using (SqlCommand command = new SqlCommand("sp_addrolemember", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter()
                        {
                            ParameterName = "rolename",
                            Value = "db_datareader"
                        });
                        command.Parameters.Add(new SqlParameter()
                        {
                            ParameterName = "membername",
                            Value = userName
                        });

                        command.ExecuteNonQuery();
                    }
                    using (SqlCommand command = new SqlCommand("sp_addrolemember", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter()
                        {
                            ParameterName = "rolename",
                            Value = "db_datawriter"
                        });
                        command.Parameters.Add(new SqlParameter()
                        {
                            ParameterName = "membername",
                            Value = userName
                        });
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        /// Adds the database owner role to the specified user on the specified database
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="loginName">Name of a windows login.</param>
        public void AddDatabaseOwnerRole(string serverName, string databaseName, string loginName)
        {
            LogEventHelper.CreateEvent(this, "AddDatabaseOwnerRole", string.Format("Adding dbowner roles on server {0} for database {1} and login {2}", serverName, databaseName, loginName));

            using (SqlConnection conn = new SqlConnection(string.Format("Data Source={0};Initial Catalog={1};Integrated Security=SSPI;", serverName, databaseName)))
            {
                conn.Open();
                var script = string.Format("SELECT TOP 1 name from sys.database_principals WHERE sid = SUSER_SID(N'{0}')", loginName);
                string userName;

                using (SqlCommand command = new SqlCommand(script, conn))
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        userName = reader.GetString(0);
    }
                    else
                    {
                        throw new Exception(string.Format("No user found in database for windows login {0}", loginName));
                    }
                }

                if (userName == "dbo")
                {
                    LogEventHelper.CreateEvent(this, "AddDatabaseOwnerRole", string.Format("Database user for loginName {0} is dbo, not adding to roles.", loginName));
                }
                else
                {
                    LogEventHelper.CreateEvent(this, "AddDatabaseOwnerRole", string.Format("Database user for loginName {0} is {1}", loginName, userName));
                    using (SqlCommand command = new SqlCommand("sp_addrolemember", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter()
                        {
                            ParameterName = "rolename",
                            Value = "db_owner"
                        });
                        command.Parameters.Add(new SqlParameter()
                        {
                            ParameterName = "membername",
                            Value = userName
                        });

                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }

    internal class AssemblyVerifier
    {
        public Assembly Assembly { get; set; }

        public AssemblyVerifier(Assembly assembly)
        {
            this.Assembly = assembly;
        }

        public void VerifyEntitiesVersion(object sender, AssemblyLoadEventArgs args)
        {
            //Get the details of the assembly the is being loaded into the appdomain
            var name = args.LoadedAssembly.GetName();
            if (name.Name == "EntityFramework")
            {
                var referencedVersion = Assembly
                    .GetReferencedAssemblies()
                    .Single(a => a.Name == "EntityFramework").Version;

                if (name.Version != referencedVersion)
                {
                    throw new Exception(string.Format("Version of EF referenced by assembly {0} does not equal version resolved locally {1}", name.Version, referencedVersion));
                }
            }
        }
    }

}
