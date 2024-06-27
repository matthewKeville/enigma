using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using UI.View.Spectre;
using UI.ViewModel;
using UI.View.ViewModel;
using Services;
using UI.View.Spectre.Game;
using View.ViewModel;
using UI.View.Spectre.Help;
using UI.Controller;
using UI.Command;

Trace.Listeners.Add(new TextWriterTraceListener("./logs/enigma.log"));
Trace.AutoFlush = true;

//HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
HostApplicationBuilder builder = Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings());

builder.Services.AddSingleton<CluesViewModel, CluesViewModel>();
builder.Services.AddSingleton<StatusViewModel, StatusViewModel>();
builder.Services.AddSingleton<ClockViewModel, ClockViewModel>();
builder.Services.AddSingleton<GridViewModel, GridViewModel>();
builder.Services.AddSingleton<GameViewModel, UI.View.ViewModel.GameViewModel>();
builder.Services.AddSingleton<RootViewModel, RootViewModel>();

builder.Services.AddSingleton<RootView, RootView>();
builder.Services.AddSingleton<HelpView, HelpView>();
builder.Services.AddSingleton<GameView, GameView>();
builder.Services.AddSingleton<ClockView, ClockView>();
builder.Services.AddSingleton<GridView, GridView>();
builder.Services.AddSingleton<StatusView, StatusView>();
builder.Services.AddSingleton<CluesView, CluesView>();

builder.Services.AddSingleton<RootController, RootController>();
builder.Services.AddSingleton<GameController, GameController>();
builder.Services.AddSingleton<GridController, GridController>();

builder.Services.AddSingleton<ICrosswordProvider,NYDebugCrosswordProvider>();

builder.Services.AddSingleton<SpectreRenderer>();
builder.Services.AddSingleton<CommandInterpreter>();
IHost host = builder.Build();

CommandInterpreter interpreter = host.Services.GetRequiredService<CommandInterpreter>();
RootController controller = host.Services.GetRequiredService<RootController>();
SpectreRenderer render = host.Services.GetRequiredService<SpectreRenderer>();

host.Run();
