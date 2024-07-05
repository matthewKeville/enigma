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
using UI.View.Spectre.Browser;
using Repository;
using UI.Controller.Browser;
using UI.Controller.Game;
using UI.Event;

Trace.Listeners.Add(new TextWriterTraceListener("./logs/enigma.log"));
Trace.AutoFlush = true;

HostApplicationBuilder builder = Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings());

builder.Services.AddSingleton<RootView, RootView>();
builder.Services.AddSingleton<HelpView, HelpView>();
builder.Services.AddSingleton<BrowserView, BrowserView>();
builder.Services.AddSingleton<BrowserController, BrowserController>();
builder.Services.AddSingleton<GameView, GameView>();
builder.Services.AddSingleton<StatusView, StatusView>();
builder.Services.AddSingleton<ClockView, ClockView>();
builder.Services.AddSingleton<GridView, GridView>();
builder.Services.AddSingleton<CluesView, CluesView>();

builder.Services.AddSingleton<RootController, RootController>();
builder.Services.AddSingleton<GameController, GameController>();
builder.Services.AddSingleton<GridController, GridController>();
builder.Services.AddSingleton<CluesController, CluesController>();

builder.Services.AddSingleton<CrosswordRepository, CrosswordRepository>();
builder.Services.AddSingleton<CrosswordService, CrosswordService>();

builder.Services.AddSingleton<ContextAccessor>();
builder.Services.AddSingleton<SpectreRenderer>();
builder.Services.AddSingleton<KeyCommandInterpreter>();
builder.Services.AddSingleton<CommandDispatcher>();
builder.Services.AddSingleton<EventDispatcher>();

IHost host = builder.Build();

KeyCommandInterpreter interpreter = host.Services.GetRequiredService<KeyCommandInterpreter>();
RootController controller = host.Services.GetRequiredService<RootController>();
SpectreRenderer render = host.Services.GetRequiredService<SpectreRenderer>();

host.Run();
