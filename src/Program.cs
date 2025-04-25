using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Services.PluginService;
using Services.CommandServices;

HostApplicationBuilder builder = Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings());
Startup.AddServices(builder.Services);
Startup.AddConfigs(builder.Configuration);
Startup.InitializeLogger();
IHost host = builder.Build();

DatabaseContext dbContext = host.Services.GetRequiredService<DatabaseContext>();
Startup.UpdateOrCreateDB(dbContext);
host.Start();

if ( args.Count() == 0 ) {
  Console.WriteLine("invalid arguments");
  Console.WriteLine($"see help for available commands");
  return;
}

String command = args[0];
switch ( command ) {
  case "list":
    host.Services.GetRequiredService<ListCommandService>().List(args);
    break;
  case "sync":
    host.Services.GetRequiredService<PluginService>().Sync();
    break;
  /**
  case "install":
  */
  case "help":
    HelpCommandService.Help();
    break;
  case "start":
    host.Services.GetRequiredService<StartCommandService>().Start(args);
    break;
  default:
    Console.WriteLine($"unknown command {command}");
    Console.WriteLine($"see help for available commands");
    break;

}
