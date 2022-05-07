namespace GamepadCmdMapper
{
    /// <summary>
    /// Entry point of the service
    /// 
    /// Usage : 
    /// GamepadCmdMapper.exe "[parameter file path]"
    /// 
    /// Parameter file path : 
    /// 1 parameter / line, a parameter is represented as follows :
    /// [Pattern 1];[Duration 1];[Command 1]
    /// [Pattern 2];[Duration 2];[Command 2]
    /// ...
    /// 
    /// With :
    ///  - Pattern : button or combination of buttons pressed at the same time required for matching the pattern
    ///              buttons must be separated by comma
    ///  - Duration : Pressed buttons duration (in milliseconds) required for matching the pattern
    ///  - Command : Command to execute for this pattern
    ///  
    /// Examples :
    /// A;1000;taskkill /F /IM notepad.exe ==> Press "A" for 1000 ms will execute the command "taskkill /F /IM notepad.exe"
    /// Start,Back;3000;taskkill /F /IM calc.exe ==> Press "Start" + "Back" for 3000 ms will execute the command "taskkill /F /IM calc.exe"
    /// 
    /// </summary>
    public class GamepadCmdWorker : BackgroundService
    {
        private const int DELAY_MS = 100;

        private readonly GamepadCmdArgs _gamepadCmdArgs;
        private readonly GamepadCmdService _gamepadCmdService;
        private readonly ILogger<GamepadCmdWorker> _logger;

        public GamepadCmdWorker(GamepadCmdArgs gamepadCmdArgs, GamepadCmdService gamepadCmdService, ILogger<GamepadCmdWorker> logger)
        {
            string initMsg;

            _gamepadCmdArgs = gamepadCmdArgs;
            _gamepadCmdService = gamepadCmdService;
            _logger = logger;

            if (!_gamepadCmdService.Init(_gamepadCmdArgs.FilePath, out initMsg))
            {
                _logger.LogError(initMsg);
            }

            _logger.LogInformation(initMsg);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(DELAY_MS, stoppingToken);

                string gamepadState = _gamepadCmdService.GetState(DELAY_MS);
                if (!string.IsNullOrEmpty(gamepadState))
                {
                    _logger.LogInformation(gamepadState);
                }
            }
        }
    }
}