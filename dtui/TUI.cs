using Terminal.Gui;
using NStack;

namespace dtui
{
    //internal interface IDTUI
    //{
    //    public static Toplevel? TopLevel = default;

    //    public void Close();

    //    public void SetStyles();

    //    public void DrawView();
    //}

    //internal sealed class LoginWindow : Window, IDTUI
    //{
    //    public bool Success { get; set; } = false;

    //    public static Toplevel? TopLevel;

    //    public LoginWindow(ref Toplevel toplevel) : base(title: "dtui#login")
    //    {
    //        TopLevel = toplevel;
    //        SetStyles();
    //        DrawView();
    //    }

    //    public void Close()
    //    {
    //        Application.RequestStop();
    //    }

    //    public void SetStyles()
    //    {
    //        X = 0;
    //        Y = 0;
    //        Width = Dim.Fill();
    //        Height = Dim.Fill();
    //    }

    //    public void DrawView()
    //    {
    //        Label header = new("login to discord") { X = 2, Y = 1 };

    //        Label usernameLabel = new("username: ") { X = Pos.Left(header), Y = Pos.Top(header) + 2 };
    //        TextField username = new() { X = usernameLabel.Text.Length + 2, Y = Pos.Top(usernameLabel), Width = 40 };

    //        Label passwordLabel = new("password: ") { X = Pos.Left(usernameLabel), Y = Pos.Top(username) + 2 };
    //        TextField password = new() { X = passwordLabel.Text.Length + 2, Y = Pos.Top(passwordLabel), Width = 40, Secret = true };

    //        Button ok = new("ok") { X = Pos.Right(password) - 6, Y = Pos.Top(password) + 2 };
    //        Button cancel = new("cancel") { X = Pos.Left(ok) - 12, Y = Pos.Top(ok) };

    //        this.Add(header, usernameLabel, username, passwordLabel, password, ok, cancel);

    //        #region register events

    //        ok.Clicked += () => OnOkClicked(username.Text, password.Text);

    //        cancel.Clicked += Close;

    //        #endregion

    //    }

    //    private void OnOkClicked(ustring username, ustring password)
    //    {
    //        Success = Discord.IsAuthenticated(username.ToString(), password.ToString());
            
    //        // try to sign in here, else abort operation gracefully and set valid
    //        Console.WriteLine($"signing in . . . (username={username}, password={password})");

    //        if (!Success)
    //        {
    //            MessageBox.ErrorQuery("login error", "failed to establish a connection to discord", "ok");
    //        }
    //        else
    //        {
    //            TopLevel?.RemoveAll();
    //            TopLevel?.Add(new DiscordChatWindow(ref TopLevel));
    //        }
    //    }
    //}

    //internal sealed class DiscordChatWindow : Window, IDTUI
    //{
    //    public static Toplevel? TopLevel;

    //    public DiscordChatWindow(ref Toplevel toplevel) : base("dtui#chat")
    //    {
    //        TopLevel = toplevel;
    //        SetStyles();
    //        DrawView();
    //    }

    //    public void Close() => throw new NotImplementedException();

    //    public void DrawView()
    //    {
    //        Label test = new ("Hello, World!") { X = 2, Y = 1 };
    //        this.Add(test);
    //    }

    //    public void SetStyles()
    //    {
    //        X = 0;
    //        Y = 0;
    //        Width = Dim.Fill();
    //        Height = Dim.Fill();
    //    }
    //}
}
