using System.Reactive;
using System.Runtime.Serialization;
using System.Resources;

using NStack;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using Terminal.Gui;
using System.Reactive.Linq;

namespace dtui
{
    [DataContract]
    public class LoginViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<bool> _isValid;

        private CancellationTokenSource _cancellationTokenSource = new();

        [Reactive, DataMember]
        public ustring Username { get; set; } = ustring.Empty;

        [Reactive, DataMember]
        public ustring Password { get; set; } = ustring.Empty;

        [IgnoreDataMember]
        public bool IsValid => _isValid.Value;

        [IgnoreDataMember]
        public bool IsAuthenticated { get; set; } = false;

        [IgnoreDataMember]
        public static Toplevel Toplevel = default!;

        [IgnoreDataMember]
        public static Configuration Configuration = default!;

        [IgnoreDataMember]
        public static ResourceManager ResourceManager = default!;

        [IgnoreDataMember]
        public ReactiveCommand<Unit, Unit> Login { get; }

        [IgnoreDataMember]
        public ReactiveCommand<Unit, Unit> Cancel { get; }

        [IgnoreDataMember]
        public ReactiveCommand<Unit, Unit> Exit { get; }

        private async Task LoginAsync()
        {
            var cancellationToken = _cancellationTokenSource.Token;

            try
            {
                IsAuthenticated = await Discord.Login(Username.ToString()!, Password.ToString()!, cancellationToken);

                if (IsAuthenticated)
                {
                    Toplevel.RemoveAll();
                    Toplevel.Add(new ChatView(new ChatViewModel(ref Toplevel, ref Configuration, ref ResourceManager)));
                }
                else
                {
                    MessageBox.ErrorQuery(ResourceManager.GetString("LoginError"), ResourceManager.GetString("LoginErrorMessage"), ResourceManager.GetString("OkButton"));
                }
            }
            catch (OperationCanceledException)
            {
                MessageBox.ErrorQuery(ResourceManager.GetString("LoginCancelled"), ResourceManager.GetString("LoginCancelledMessage"), ResourceManager.GetString("OkButton"));
                _cancellationTokenSource.Dispose();

            }
            finally
            {
                _cancellationTokenSource = new CancellationTokenSource();
            }
        }

        public LoginViewModel(ref Toplevel toplevel, ref Configuration configuration, ref ResourceManager resourceManager)
        {
            Toplevel = toplevel;
            Configuration = configuration;
            ResourceManager = resourceManager;

            IObservable<bool>? canLogin = this.WhenAnyValue(
                    x => x.Username,
                    x => x.Password,
                    (username, password) => !ustring.IsNullOrEmpty(username) && !ustring.IsNullOrEmpty(password)
            );

            _isValid = canLogin.ToProperty(this, x => x.IsValid, scheduler: RxApp.MainThreadScheduler);

            Login = ReactiveCommand.CreateFromTask(LoginAsync, canLogin);
            Login.ThrownExceptions.Subscribe(async _ => await LoginAsync());

            Cancel = ReactiveCommand.Create(() => _cancellationTokenSource.Cancel());

            Exit = ReactiveCommand.Create(() => Application.RequestStop());
        }
    }
}
