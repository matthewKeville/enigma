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

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

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

//builder.Services.AddSingleton<ICrosswordProvider,RotatingCrosswordProvider>();
builder.Services.AddSingleton<ICrosswordProvider,NYDebugCrosswordProvider>();

builder.Services.AddSingleton<CommandInterpreter, CommandInterpreter>();

builder.Services.AddSingleton<SpectreRenderer,SpectreRenderer>();
using IHost host = builder.Build();

CommandInterpreter commandInterpreter = host.Services.GetRequiredService<CommandInterpreter>();
RootController rootController = host.Services.GetRequiredService<RootController>();
SpectreRenderer renderer = host.Services.GetRequiredService<SpectreRenderer>();
renderer.init();

Trace.WriteLine("asdfasdff");
