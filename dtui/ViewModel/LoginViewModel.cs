using System.Reactive;
using System.Runtime.Serialization;
using System.Resources;

using NStack;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using Terminal.Gui;

namespace dtui
{
    [DataContract]
    public class LoginViewModel : ReactiveObject
    {
        readonly ObservableAsPropertyHelper<bool> _isValid;

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
        public ReactiveCommand<Unit, Unit> Exit { get; }

        private async Task LoginAsync(CancellationToken ct)
        {
            // TODO: use cancellation token
            IsAuthenticated = await Discord.Login(Username.ToString(), Password.ToString());

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
            Exit = ReactiveCommand.Create(() => Application.RequestStop());
        }
    }
}
