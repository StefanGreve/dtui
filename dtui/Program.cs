using System.Reactive.Concurrency;
using ReactiveUI;
using Terminal.Gui;
using System.Reflection;
using System.Resources;

namespace dtui
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0 && args.Contains("-usc"))
            {
                Application.UseSystemConsole = true;
            }

            Application.Init();

            var cm = new ConfigurationManager();
            var configuration = ConfigurationManager.Configuration;
            ConfigurationManager.ChangeCulture();

            var resourceManager = new ResourceManager($"dtui.i18n.string.{configuration.Language}", Assembly.GetExecutingAssembly());

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

            var loginView = new LoginView(new LoginViewModel(), ref configuration, ref resourceManager);

            Application.Run(loginView);
            Application.Shutdown();
        }
    }
}
