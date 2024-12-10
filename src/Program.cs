using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.CrosswordInstaller;
using Services.CrosswordInstaller.NYT;
using Event;
using Terminal.Gui;
using UI.Game;
using UI;
using UI.Game.Clues;

HostApplicationBuilder builder = Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings());
builder.Services.AddSingleton<DatabaseContext, DatabaseContext>();
builder.Services.AddSingleton<CrosswordService, CrosswordService>();
builder.Services.AddSingleton<NYTCrosswordInstaller, NYTCrosswordInstaller>();
builder.Services.AddSingleton<NYTCrosswordParser, NYTCrosswordParser>();
builder.Services.AddSingleton<CrosswordInstallerService, CrosswordInstallerService>();

builder.Services.AddSingleton<EventBus, EventBus>();
builder.Services.AddSingleton<RootView, RootView>();
builder.Services.AddSingleton<BrowserView, BrowserView>();
builder.Services.AddSingleton<PuzzleInstallerView, PuzzleInstallerView>();
builder.Services.AddSingleton<PuzzlePickerView, PuzzlePickerView>();
builder.Services.AddSingleton<GameView, GameView>();
builder.Services.AddSingleton<GridView, GridView>();

builder.Services.AddSingleton<CluesView, CluesView>();
builder.Services.AddSingleton<CluesSingleView, CluesSingleView>();
builder.Services.AddSingleton<CluesSplitView, CluesSplitView>();

IHost host = builder.Build();
host.Start();

Trace.Listeners.Add(new TextWriterTraceListener("./logs/enigma.log"));
Trace.AutoFlush = true;

Application.Init();
Application.KeyBindings.Clear();
Application.KeyDown += (sender,key) => {
  //Default was escape
  if (key.Equals(Key.C.WithCtrl)) {
    Application.RequestStop();
  }
};

Application.Run(host.Services.GetService<RootView>());
Application.Shutdown ();


