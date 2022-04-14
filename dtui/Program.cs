using System.Reactive.Concurrency;
using System.Reflection;
using System.Resources;

using McMaster.Extensions.CommandLineUtils;

using ReactiveUI;

using Terminal.Gui;

namespace dtui
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var cm = new ConfigurationManager();
            var configuration = ConfigurationManager.Configuration;
            ConfigurationManager.ChangeCulture();

            var assembly = Assembly.GetExecutingAssembly();
            var resourceManager = new ResourceManager($"dtui.i18n.string.{configuration.Language}", assembly);

            var app = new CommandLineApplication
            {
                Name = assembly.GetName().Name,
                Description = "Cross-Platform Discord Terminal UI written in .NET Core."
            };

            app.HelpOption(inherited: true);

            var version = app.Option("-v|--version", "Display program version", CommandOptionType.NoValue);

            app.Command("run", runCmd =>
            {
                runCmd.Description = "Enter the Discord TUI.";
                var usc = runCmd.Option("-usc|--use-system-console", "Enforce usage of the System.Console-based driver", CommandOptionType.NoValue);

                runCmd.OnExecute(() =>
                {
                    Application.UseSystemConsole = usc.HasValue();

                    Application.Init();
                    var toplevel = Application.Top;

                    Colors.Base = new ColorScheme()
                    {
                        Normal = Application.Driver.MakeAttribute(Color.Gray, Color.Black),
                        Disabled = Application.Driver.MakeAttribute(Color.DarkGray, Color.Black),
                        Focus = Application.Driver.MakeAttribute(Color.BrightMagenta, Color.Gray),
                        HotFocus = Application.Driver.MakeAttribute(Color.White, Color.Black),
                        HotNormal = Application.Driver.MakeAttribute(Color.Magenta, Color.Black)
                    };

                    RxApp.MainThreadScheduler = TerminalScheduler.Default;
                    RxApp.TaskpoolScheduler = TaskPoolScheduler.Default;

                    var loginView = new LoginView(new LoginViewModel(ref toplevel, ref configuration, ref resourceManager));

                    toplevel.Add(loginView);
                    Application.Run();
                    Application.Shutdown();
                });
            });

            app.OnExecute(() =>
            {
                if (version.HasValue())
                {
                    const string color = "\u001b[95m";
                    const string reset = "\u001b[0m";

                    Console.OutputEncoding = System.Text.Encoding.Unicode;

                    var info = new List<string>
                    {
                        string.Empty,
                        $"{color}  ⢠⣴⣾⣵⣶⣶⣾⣿⣦⡄   {reset}",
                        $"{color} ⢀⣾⣿⣿⢿⣿⣿⣿⣿⣿⣿⡄  {reset}{assembly.GetName().Name} (version {assembly.GetName().Version})",
                        $"{color} ⢸⣿⣿⣧⣀⣼⣿⣄⣠⣿⣿⣿  {reset}https://github.com/stefangreve/dtui",
                        $"{color} ⠘⠻⢷⡯⠛⠛⠛⠛⢫⣿⠟⠛  {reset}",
                        string.Empty
                    };

                    Console.WriteLine(string.Join(Environment.NewLine, info));
                }
                else
                {
                    app.ShowHelp();
                }
            });

            return app.Execute(args);
        }
    }
}
