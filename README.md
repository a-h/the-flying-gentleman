# the-flying-gentleman

A deployment framework for writing deployments in C#.

Define:
* Roles (e.g. Database, Website, WindowsService).
* Installation steps against target servers.
** Run command lines.
** Install custom cultures.
** Run SQL Server Scripts and Entity Framework Migrations.
** Mirror directories, delete files and transform App.Config files.
** Setup RabbitMQ users.
** Setup IIS, including setting up application pools, start and stop application pools.
** Install and manage Windows services.
