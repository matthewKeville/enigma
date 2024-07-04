using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using UI.View.Spectre;
using Services;
using UI.View.Spectre.Help;
using UI.Controller;
using UI.Command;
using UI.View.Spectre.Game;
using UI.View.Spectre.Status;
using Context;
using Entity;
using UI.View.Spectre.Browser;

Trace.Listeners.Add(new TextWriterTraceListener("./logs/enigma.log"));
Trace.AutoFlush = true;

HostApplicationBuilder builder = Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings());

builder.Services.AddSingleton<RootView, RootView>();
builder.Services.AddSingleton<HelpView, HelpView>();
builder.Services.AddSingleton<BrowserView, BrowserView>();
builder.Services.AddSingleton<BrowserController, BrowserController>();
builder.Services.AddSingleton<GameView, GameView>();
builder.Services.AddSingleton<StatusView, StatusView>();
builder.Services.AddSingleton<UI.View.Spectre.Status.ClockView, ClockView>();
builder.Services.AddSingleton<GridView, GridView>();
builder.Services.AddSingleton<CluesView, CluesView>();

builder.Services.AddSingleton<RootController, RootController>();
builder.Services.AddSingleton<GameController, GameController>();
builder.Services.AddSingleton<GridController, GridController>();
builder.Services.AddSingleton<CluesController, CluesController>();

builder.Services.AddSingleton<ContextAccessor>();
builder.Services.AddSingleton<SpectreRenderer>();
builder.Services.AddSingleton<KeyCommandInterpreter>();
builder.Services.AddSingleton<CommandDispatcher>();
IHost host = builder.Build();

CommandDispatcher commandDispatcher = host.Services.GetRequiredService<CommandDispatcher>();

/**
new Thread( () => {
    Thread.Sleep(3000);
    commandDispatcher.dispatchCommand(new CommandEventArgs(Command.DBG_PUZZLE_SWAP));
}).Start();
*/

KeyCommandInterpreter interpreter = host.Services.GetRequiredService<KeyCommandInterpreter>();
RootController controller = host.Services.GetRequiredService<RootController>();
SpectreRenderer render = host.Services.GetRequiredService<SpectreRenderer>();

host.Run();
