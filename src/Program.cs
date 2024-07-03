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

Trace.Listeners.Add(new TextWriterTraceListener("./logs/enigma.log"));
Trace.AutoFlush = true;

HostApplicationBuilder builder = Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings());

builder.Services.AddSingleton<RootView, RootView>();
builder.Services.AddSingleton<HelpView, HelpView>();

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
builder.Services.AddSingleton<CommandInterpreter>();
IHost host = builder.Build();

ContextAccessor ctxAccessor = host.Services.GetRequiredService<ContextAccessor>();
NYDebugCrosswordGenerator generator= new NYDebugCrosswordGenerator();
ctxAccessor.setContext(new ApplicationContext(generator.sample1()));

new Thread( () => {
    Thread.Sleep(3000);
    ctxAccessor.setContext(new ApplicationContext(generator.sample2()));
    Trace.WriteLine("setting new puzzle context");
}).Start();

CommandInterpreter interpreter = host.Services.GetRequiredService<CommandInterpreter>();
RootController controller = host.Services.GetRequiredService<RootController>();
SpectreRenderer render = host.Services.GetRequiredService<SpectreRenderer>();

host.Run();
