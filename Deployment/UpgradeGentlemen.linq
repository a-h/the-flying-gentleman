<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\WPF\UIAutomationTypes.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\UIAutomationClient.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.ServiceProcess.dll</Reference>
  <Namespace>System.Windows.Automation</Namespace>
  <Namespace>System.ServiceProcess</Namespace>
</Query>

void Main()
{
	var leagueOfGentlemen = new[] 
    { 
        "server1",
	};
	
	
	var sourceFolder = @"\\buildserver\the-flying-gentleman\bin\release\";
	var sourceLibraryFilePath = sourceFolder + @"\FlyingGentleman.Library.dll";
	var sourceLibraryVersion = Assembly.ReflectionOnlyLoadFrom(sourceLibraryFilePath).GetName().Version;
	
	foreach (var gentlemanHost in leagueOfGentlemen)
	{
		int retryCount = 0;
		while (retryCount++ < 3)
		{
			try
			{
				using (var gentleman = new ServiceController("Flying Gentleman Agent", gentlemanHost))
				{	
					DoDeployment(gentlemanHost, gentleman, sourceFolder, sourceLibraryVersion);
				}
				
				break;
			}
			catch(Exception ex)
			{
				ex.Message.Dump();
				if (retryCount < 3)
				{
					"Failed, retrying...".Dump();
				}
				else
				{
					"Deployment failed!".Dump();
				}
			}
		}
	};
}

public void DoDeployment(string gentlemanHost, ServiceController gentleman, string sourceFolder, Version sourceLibraryVersion)
{
	Console.Write("Gentleman {0} status: {1}. ", gentlemanHost, gentleman.Status);
	if (gentleman.Status == ServiceControllerStatus.Running)
	{
		Console.Write("Stopping... ");
		gentleman.Stop();
		Console.Write("Waiting... ");
		gentleman.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 1, 0));
		Console.Write("Stopped. ");
	}
	Console.WriteLine();
	
	var destinationFolder = string.Format(@"\\{0}\C$\Services\FlyingGentleman\Agent", gentlemanHost);
	
	Console.Write("Copying files:\n\tSource: {0}\n\tDestination: {1}\n\t", sourceFolder, destinationFolder);
	foreach (var file in Directory.EnumerateFiles(sourceFolder))
	{
		File.Copy(file, file.Replace(sourceFolder, destinationFolder), true);
		Console.Write(".");
	}
	
	var copiedLibraryFilePath = string.Format(@"\\{0}\C$\Services\FlyingGentleman\Agent\FlyingGentleman.Library.dll", gentlemanHost);
	var copiedLibraryVersion = GetAssemblyVersion(copiedLibraryFilePath);
	("comparing copied library version " + copiedLibraryVersion + " with source version " + sourceLibraryVersion).Dump();
	if (copiedLibraryVersion != sourceLibraryVersion)
	{
		throw new Exception("Destination file does not match expected version after deployment!");
	}
	
	Console.WriteLine("\n\tRenaming config.");
	File.Copy(destinationFolder + @"\Service.config", 
			  destinationFolder + @"\FlyingGentleman.Agent.exe.config", true);
	
	
	Console.Write("Starting... ");
	gentleman.Start();
	Console.Write("Waiting... ");
	gentleman.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 1, 0));
	Console.WriteLine("Done!");
}


public Version GetAssemblyVersion(string path)
{
	var libraryDomain = Util.CreateAppDomain("LibraryDomain");
	try
	{
		libraryDomain.SetData("gentlemanLibraryPath", path);
		
		path.Dump();
		
		libraryDomain.DoCallBack(() => {
			var libraryPath = AppDomain.CurrentDomain.GetData("gentlemanLibraryPath") as string;
			var copiedLibraryVersion = Assembly.ReflectionOnlyLoadFrom(libraryPath).GetName().Version;
			AppDomain.CurrentDomain.SetData("gentlemanLibraryVersion", copiedLibraryVersion);
		});
		
		var version = libraryDomain.GetData("gentlemanLibraryVersion") as Version;
		return version;
	}
	finally
	{
		AppDomain.Unload(libraryDomain);
	}
}