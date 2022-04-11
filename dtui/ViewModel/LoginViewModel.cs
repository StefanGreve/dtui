using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.Serialization;

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
        public ReactiveCommand<Unit, Unit> Login { get; }

        [IgnoreDataMember]
        public ReactiveCommand<Unit, Unit> Exit { get; }

        private async Task LoginAsync(CancellationToken ct)
        {
            // TODO: use cancellation token
            IsAuthenticated = await Discord.Login(Username.ToString(), Password.ToString());
        }

        public LoginViewModel()
        {
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
