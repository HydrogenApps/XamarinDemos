using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using XplatChat.NativeApp.Shared.ViewModels;

namespace XplatChat.FormsApp
{
	public class App
	{
		public static Page GetMainPage()
		{
		    return new MainPage();
		}
	}

    public class MainPage : ContentPage
    {
        public MainPage()
        {

            Entry txtMessage;
            Button btnSend;
            Editor txtChat;


            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(5, 30, 5, 5),
                Spacing = 0,
                Children =
				                {
				                    new StackLayout
				                    {
				                        Orientation = StackOrientation.Horizontal,
                                       Spacing = 4,
                                       BackgroundColor = Color.White,
				                        Children =
				                        {
				                            (txtMessage = new Entry {Placeholder = "Enter message", HorizontalOptions = LayoutOptions.FillAndExpand, TextColor = Color.Black}),
				                            (btnSend = new Button {Text = "Send", BackgroundColor = Color.Lime, TextColor = Color.White, BorderRadius = 0, BorderWidth = 0})
				                        }
				                    },
				                    (txtChat = new Editor
				                    {
				                        Text = @"",
				                        IsEnabled = false,
				                        VerticalOptions = LayoutOptions.FillAndExpand,
                                        BackgroundColor = Color.White
				                    })
				                }
            };

            var viewModel = new MainViewModel();
            viewModel.Start(GetUserName());

            btnSend.Clicked += (sender, args) => viewModel.Send();            
            txtMessage.TextChanged += (sender, args) => viewModel.Message = txtMessage.Text;
            viewModel.MessageChanged += () => Device.BeginInvokeOnMainThread(() =>
            {
                if (txtMessage.Text != viewModel.Message)
                    txtMessage.Text = viewModel.Message;
            });
            viewModel.ChatChanged += () => Device.BeginInvokeOnMainThread(() => txtChat.Text = viewModel.Chat);
        }

        private string GetUserName()
        {
            switch (Device.OS)
            {
                case TargetPlatform.iOS:
                    return "Tim";
                case TargetPlatform.Android:
                    return "Larry";
                case TargetPlatform.WinPhone:
                    return "Satya";
            }
            return "";
        }
    }
}
