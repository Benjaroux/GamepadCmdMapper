using GamepadCmdMapper;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "GamepadCmdMapper";
    })
    .ConfigureServices(services =>
    {
        services.AddSingleton(new GamepadCmdArgs() { Args = args });
        services.AddSingleton<GamepadCmdService>();
        services.AddHostedService<GamepadCmdWorker>();
    })
    .Build();

await host.RunAsync();