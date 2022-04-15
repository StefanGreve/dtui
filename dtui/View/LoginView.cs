using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Resources;

using NStack;

using ReactiveMarbles.ObservableEvents;

using ReactiveUI;

using Terminal.Gui;

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
            TextField usernameInput = new() { X = previous.Text.Length + (Language.IsAsian(Configuration.Language) ? -2 : 2), Y = Pos.Top(previous), Width = 40 };

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
            TextField passwordInput = new() { X = previous.Text.Length + (Language.IsAsian(Configuration.Language) ? -2 : 2), Y = Pos.Top(previous), Width = 40, Secret = true };

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
            Button loginButton = new(ResourceManager.GetString("LoginButton")) { Y = Pos.Top(previous) + 2 };
            loginButton.X = Pos.Right(previous) - loginButton.Text.Length - (Language.IsAsian(Configuration.Language) ? 0 : 4);

            loginButton
                .Events()
                .Clicked
                .InvokeCommand(ViewModel, x => x.Login)
                .DisposeWith(_disposable);

            Add(loginButton);
            return loginButton;
        }

        Button GetCancelButton(View previous)
        {
            Button cancelButton = new(ResourceManager.GetString("CancelButton")) { X = Pos.Left(previous), Y = Pos.Top(previous) };
            cancelButton.X -= cancelButton.Text.Length + (Language.IsAsian(Configuration.Language) ? 0 : 4) + 2;

            cancelButton
                .Events()
                .Clicked
                .InvokeCommand(ViewModel, x => x.Cancel)
                .DisposeWith(_disposable);

            Add(cancelButton);
            return cancelButton;
        }

        Button GetExitButton(View previus)
        {
            Button exitButton = new(ResourceManager.GetString("ExitButton")) { X = Pos.Left(previus), Y = Pos.Top(previus) };
            exitButton.X -= exitButton.Text.Length + (Language.IsAsian(Configuration.Language) ? 3 : 4) + 2;

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
            var idle = ustring.Make(ResourceManager.GetString("LoginIdleLabel"));
            var progress = ustring.Make(ResourceManager.GetString("LoginProgressLabel")!.PadRight(idle.Length));
            var progressLabel = new Label(idle) { X = Pos.Left(initial), Y = Pos.Top(previous) + 2, Width = 40, Enabled = false };

            ViewModel
                .WhenAnyObservable(x => x.Login.IsExecuting)
                .Select(executing => executing ? progress : idle)
                .ObserveOn(RxApp.MainThreadScheduler)
                .BindTo(progressLabel, x => x.Text)
                .DisposeWith(_disposable);

            Add(progressLabel);
            return progressLabel;
        }

        public LoginView(LoginViewModel viewModel) : base("dtui")
        {
            ViewModel = viewModel;
            Configuration = LoginViewModel.Configuration;
            ResourceManager = LoginViewModel.ResourceManager;

            Label titleLabel = GetTitleLabel();
            Label usernameLabel = GetUsernameLabel(titleLabel);
            TextField usernameInput = GetUsernameInput(usernameLabel);
            Label passwordLabel = GetPasswordLabel(usernameLabel);
            TextField passwordInput = GetPasswordInput(passwordLabel);
            Button loginButton = GetLoginButton(passwordInput);
            Label progressLabel = GetProgressLabel(titleLabel, loginButton);
            Button cancelButton = GetCancelButton(loginButton);
            Button exitButton = GetExitButton(cancelButton);
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
