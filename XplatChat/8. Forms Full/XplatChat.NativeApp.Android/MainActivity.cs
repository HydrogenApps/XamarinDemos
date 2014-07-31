using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using XplatChat.NativeApp.Shared.ViewModels;

namespace XplatChat.NativeApp.Android
{
    [Activity(Label = "XplatChat.NativeApp.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private MainViewModel _viewModel = new MainViewModel();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var btnSend = FindViewById<Button>(Resource.Id.btnSend);
            var txtMessage = FindViewById<EditText>(Resource.Id.txtMessage);
            var txtChat = FindViewById<TextView>(Resource.Id.txtChat);

            btnSend.Click += (sender, args) => _viewModel.Send();            
            txtMessage.TextChanged += (sender, args) => _viewModel.Message = txtMessage.Text;
            _viewModel.MessageChanged += () => RunOnUiThread(() =>
            {
                if (txtMessage.Text != _viewModel.Message)
                    txtMessage.Text = _viewModel.Message;
            });
            _viewModel.ChatChanged += () => RunOnUiThread(() => txtChat.Text = _viewModel.Chat);

            _viewModel.Start("Larry");
        }
    }
}

