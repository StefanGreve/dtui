using System.Reactive.Disposables;
using System.Reactive.Linq;
using NStack;
using ReactiveUI;
using Terminal.Gui;
using ReactiveMarbles.ObservableEvents;
using System.Resources;


namespace dtui
{
    public class ChatView : Window, IViewFor<ChatViewModel>
    {
        readonly CompositeDisposable _disposable = new();

        public ChatViewModel ViewModel { get; set; }

        public Configuration Configuration { get; set; }

        public ResourceManager ResourceManager { get; set; }

        public Label GetTitleLabel()
        {
            Label titleLabel = new("This is a test title label") { X = 1, Y = 1 };
            Add(titleLabel);
            return titleLabel;
        }

        public ChatView(ChatViewModel viewModel) : base("dtui")
        {
            ViewModel = viewModel;
            Configuration = ChatViewModel.Configuration;
            ResourceManager = ChatViewModel.ResourceManager;

            Label titleLabel = GetTitleLabel();
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ChatViewModel)value;
        }

        protected override void Dispose(bool disposing)
        {
            _disposable.Dispose();
            base.Dispose(disposing);
        }
    }
}
