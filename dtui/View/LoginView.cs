using System.Reactive.Disposables;
using System.Reactive.Linq;
using NStack;
using ReactiveUI;
using Terminal.Gui;
using ReactiveMarbles.ObservableEvents;
using System.Resources;

namespace dtui
{
    public class LoginView : Window, IViewFor<LoginViewModel>
    {
        readonly CompositeDisposable _disposable = new();

        public LoginViewModel ViewModel { get; set; }

        public Configuration Configuration { get; set; }

        public ResourceManager ResourceManager { get; set; }

        Label GetTitleLabel()
        {
            Label titleLabel = new(ResourceManager.GetString("LoginTitleLabel")) { X = 1, Y = 1 };
            Add(titleLabel);
            return titleLabel;
        }

        Label GetUsernameLabel(View previous)
        {
            Label usernameLabel = new(ResourceManager.GetString("UsernameLabel")) { X = Pos.Left(previous), Y = Pos.Top(previous) + 2 };
            Add(usernameLabel);
            return usernameLabel;
        }

        TextField GetUsernameInput(View previous)
        {
            TextField usernameInput = new() { X = previous.Text.Length + 2, Y = Pos.Top(previous), Width = 40 };

            ViewModel
                .WhenAnyValue(x => x.Username)
                .BindTo(usernameInput, x => x.Text)
                .DisposeWith(_disposable);

            usernameInput
                .Events()
                .TextChanged
                .Select(old => usernameInput.Text)
                .DistinctUntilChanged()
                .BindTo(ViewModel, x => x.Username)
                .DisposeWith(_disposable);

            Add(usernameInput);
            return usernameInput;
        }

        Label GetPasswordLabel(View previous)
        {
            Label passwordLabel = new(ResourceManager.GetString("PasswordLabel")) { X = Pos.Left(previous), Y = Pos.Top(previous) + 2 };
            Add(passwordLabel);
            return passwordLabel;
        }

        TextField GetPasswordInput(View previous)
        {
            TextField passwordInput = new() { X = previous.Text.Length + 2, Y = Pos.Top(previous), Width = 40, Secret = true };

            ViewModel
                .WhenAnyValue(x => x.Password)
                .BindTo(passwordInput, x => x.Text)
                .DisposeWith(_disposable);

            passwordInput
                .Events()
                .TextChanged
                .Select(old => passwordInput.Text)
                .DistinctUntilChanged()
                .BindTo(ViewModel, x => x.Password)
                .DisposeWith(_disposable);

            Add(passwordInput);
            return passwordInput;
        }

        Button GetLoginButton(View previous)
        {
            Button loginButton = new("login") { X = Pos.Right(previous), Y = Pos.Top(previous) + 2 };
            loginButton.X -= loginButton.Text.Length + 4;

            loginButton
                .Events()
                .Clicked
                .InvokeCommand(ViewModel, x => x.Login)
                .DisposeWith(_disposable);

            Add(loginButton);
            return loginButton;
        }

        Button GetExitButton(View previus)
        {
            Button exitButton = new("exit") { X = Pos.Left(previus), Y = Pos.Top(previus) };
            exitButton.X -= exitButton.Text.Length + 4 + 2;

            exitButton
                .Events()
                .Clicked
                .InvokeCommand(ViewModel, x => x.Exit)
                .DisposeWith(_disposable);

            Add(exitButton);
            return exitButton;
        }

        Label GetProgressLabel(View initial, View previous)
        {
            var idle = ustring.Make("press 'login' to log in");
            var progress = ustring.Make("logging in . . .".PadRight(idle.Length));
            var progressLabel = new Label(idle) { X = Pos.Left(initial), Y = Pos.Top(previous), Width = 40, Enabled = false };

            ViewModel
                .WhenAnyObservable(x => x.Login.IsExecuting)
                .Select(executing => executing ? progress : idle)
                .ObserveOn(RxApp.MainThreadScheduler)
                .BindTo(progressLabel, x => x.Text)
                .DisposeWith(_disposable);


            Add(progressLabel);
            return progressLabel;
        }

        public LoginView(LoginViewModel viewModel, ref Configuration configuration, ref ResourceManager resourceManager) : base("dtui")
        {
            ViewModel = viewModel;
            Configuration = configuration;
            ResourceManager = resourceManager;
            Label titleLabel = GetTitleLabel();
            Label usernameLabel = GetUsernameLabel(titleLabel);
            TextField usernameInput = GetUsernameInput(usernameLabel);
            Label passwordLabel = GetPasswordLabel(usernameLabel);
            TextField passwordInput = GetPasswordInput(passwordLabel);
            Button loginButton = GetLoginButton(passwordInput);
            Button exitButton = GetExitButton(loginButton);

            GetProgressLabel(titleLabel, loginButton);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (LoginViewModel)value;
        }

        protected override void Dispose(bool disposing)
        {
            _disposable.Dispose();
            base.Dispose(disposing);
        }
    }
}
