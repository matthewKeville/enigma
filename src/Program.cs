using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Services;

using UI.Commands;
using UI.Controller;
using UI.Controller.Browser;
using UI.Controller.Game;

using UI.Event;

using UI.View.Spectre;
using UI.View.Spectre.Help;
using UI.View.Spectre.Game;
using UI.View.Spectre.Status;
using UI.View.Spectre.Browser;
using Services.CrosswordInstaller;
using Services.CrosswordInstaller.NYT;
using UI.Controller.Help;
using UI.Controller.Game.Status;
using UI.View.Spectre.Game.Complete;
using UI.Controller.Complete;

Trace.Listeners.Add(new TextWriterTraceListener("./logs/enigma.log"));
Trace.AutoFlush = true;

HostApplicationBuilder builder = Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings());

builder.Services.AddSingleton<DatabaseContext, DatabaseContext>();

builder.Services.AddSingleton<RootView, RootView>();
builder.Services.AddSingleton<HelpView, HelpView>();

builder.Services.AddSingleton<BrowserView, BrowserView>();
builder.Services.AddSingleton<InstallerView, InstallerView>();
builder.Services.AddSingleton<PickerView, PickerView>();
builder.Services.AddSingleton<GameView, GameView>();
builder.Services.AddSingleton<StatusView, StatusView>();
builder.Services.AddSingleton<ClockView, ClockView>();
builder.Services.AddSingleton<GridView, GridView>();
builder.Services.AddSingleton<CluesView, CluesView>();
builder.Services.AddSingleton<CompleteView, CompleteView>();

builder.Services.AddSingleton<RootController, RootController>();
builder.Services.AddSingleton<HelpController, HelpController>();
builder.Services.AddSingleton<BrowserController, BrowserController>();
builder.Services.AddSingleton<PickerController, PickerController>();
builder.Services.AddSingleton<InstallerController, InstallerController>();

builder.Services.AddSingleton<GameController, GameController>();
builder.Services.AddSingleton<GridController, GridController>();
builder.Services.AddSingleton<CluesController, CluesController>();
builder.Services.AddSingleton<CompleteController, CompleteController>();

builder.Services.AddSingleton<StatusController, StatusController>();
builder.Services.AddSingleton<ClockController, ClockController>();

builder.Services.AddSingleton<CrosswordService, CrosswordService>();
builder.Services.AddSingleton<NYTCrosswordInstaller, NYTCrosswordInstaller>();
builder.Services.AddSingleton<NYTCrosswordParser, NYTCrosswordParser>();
builder.Services.AddSingleton<CrosswordInstallerService, CrosswordInstallerService>();

builder.Services.AddSingleton<SpectreRenderer>();
builder.Services.AddSingleton<KeyReader>();
builder.Services.AddSingleton<KeyDispatcher>();
builder.Services.AddSingleton<EventDispatcher>();

IHost host = builder.Build();

KeyReader keyReader = host.Services.GetRequiredService<KeyReader>();
RootController controller = host.Services.GetRequiredService<RootController>();
SpectreRenderer render = host.Services.GetRequiredService<SpectreRenderer>();

host.Run();
