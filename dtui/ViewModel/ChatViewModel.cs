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
    public class ChatViewModel : ReactiveObject
    {
        [IgnoreDataMember]
        public static Toplevel Toplevel { get; set; } = default!;

        [IgnoreDataMember]
        public static Configuration Configuration = default!;

        [IgnoreDataMember]
        public static ResourceManager ResourceManager = default!;

        public ChatViewModel(ref Toplevel toplevel, ref Configuration configuration, ref ResourceManager resourceManager)
        {
            Toplevel = toplevel;
            Configuration = configuration;
            ResourceManager = resourceManager;
        }
    }
}
