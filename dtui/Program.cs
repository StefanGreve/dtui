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
            ConfigurationManager.Init();
            ConfigurationManager.ChangeCulture();

            var assembly = Assembly.GetExecutingAssembly();
            var configuration = ConfigurationManager.Configuration;

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
                var language = runCmd.Option("-l|--language", "Set TUI language code", CommandOptionType.SingleValue);

                runCmd.OnExecute(() =>
                {
                    Application.UseSystemConsole = usc.HasValue() ? true : configuration.UseSystemConsole;

                    Application.Init();
                    var toplevel = Application.Top;
                    var colorscheme = configuration.ColorScheme;
                    var resourceManager = new ResourceManager($"dtui.i18n.string.{(language.HasValue() ? language.Value() : configuration.Language)}", assembly);

                    Colors.Base = new Terminal.Gui.ColorScheme()
                    {
                        Disabled = Application.Driver.MakeAttribute(colorscheme.Disabled.ForeColor, colorscheme.Disabled.BackColor),
                        Normal = Application.Driver.MakeAttribute(colorscheme.Normal.ForeColor, colorscheme.Normal.BackColor),
                        HotNormal = Application.Driver.MakeAttribute(colorscheme.HotNormal.ForeColor, colorscheme.HotNormal.BackColor),
                        Focus = Application.Driver.MakeAttribute(colorscheme.Focus.ForeColor, colorscheme.Focus.BackColor),
                        HotFocus = Application.Driver.MakeAttribute(colorscheme.HotFocus.ForeColor, colorscheme.HotFocus.BackColor)
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
