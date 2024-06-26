using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using UI.View.Spectre;
using UI.ViewModel;
using UI.View.ViewModel;
using Spectre.Console;
using Services;

Trace.Listeners.Add(new TextWriterTraceListener("./logs/enigma.log"));
Trace.AutoFlush = true;
Trace.WriteLine("starting");

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<MainView, MainView>();
builder.Services.AddSingleton<ClockView, ClockView>();
builder.Services.AddSingleton<GridView, GridView>();
builder.Services.AddSingleton<StatusView, StatusView>();
builder.Services.AddSingleton<CluesView, CluesView>();

builder.Services.AddSingleton<CluesViewModel, CluesViewModel>();
builder.Services.AddSingleton<StatusViewModel, StatusViewModel>();
builder.Services.AddSingleton<ClockViewModel, ClockViewModel>();
builder.Services.AddSingleton<GridViewModel, GridViewModel>();

//builder.Services.AddSingleton<ICrosswordProvider,NYDebugCrosswordProvider>();
//builder.Services.AddSingleton<ICrosswordProvider,DebugCrosswordProvider>();
builder.Services.AddSingleton<ICrosswordProvider,RotatingCrosswordProvider>();

builder.Services.AddSingleton<SpectreRenderer,SpectreRenderer>();
using IHost host = builder.Build();

SpectreRenderer renderer = host.Services.GetRequiredService<SpectreRenderer>();
renderer.init();
Trace.WriteLine("asdfasdff");
