using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNet.SignalR.Client;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using XplatChat.Contract.Pcl;
using XplatChat.NativeApp.Shared.ViewModels;

namespace XplatChat.NativeApp.iPhone.Views
{
  
    public class MainViewController : UIViewController
    {
        private MainViewModel _viewModel = new MainViewModel();

        public override void ViewDidLoad()
        {

            base.ViewDidLoad();

            SetupView();

            btnSend.TouchUpInside += (sender, args) => _viewModel.Send();
            txtMessage.AllEditingEvents += (sender, args) => _viewModel.Message = txtMessage.Text;            
            _viewModel.MessageChanged += () => UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                if (txtMessage.Text != _viewModel.Message)
                    txtMessage.Text = _viewModel.Message;
            });
            _viewModel.ChatChanged += () => UIApplication.SharedApplication.InvokeOnMainThread(() => txtChat.Text = _viewModel.Chat);

            _viewModel.Start("Tim");
        }

        private UITextField txtMessage;
        private UITextView txtChat;
        private UIButton btnSend;


        private void SetupView()
        {

            View.BackgroundColor = UIColor.White;





            txtMessage = new UITextField { TranslatesAutoresizingMaskIntoConstraints = false };

            txtChat = new UITextView { TranslatesAutoresizingMaskIntoConstraints = false, TextAlignment = UITextAlignment.Left, Editable = false };

            btnSend = new UIButton(UIButtonType.System) { TranslatesAutoresizingMaskIntoConstraints = false };

            btnSend.SetTitle("Send", UIControlState.Normal);



            Add(txtMessage);

            Add(txtChat);

            Add(btnSend);



            var nsDictionary = NSDictionary.FromObjectsAndKeys(new object[] { txtMessage, txtChat, btnSend }, new[] { "txtMessage", "lblChat", "btnSend" });



            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|-[txtMessage(==20)]", NSLayoutFormatOptions.DirectionLeadingToTrailing, null, nsDictionary));

            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-0-[txtMessage]-[btnSend(==100)]-0-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, null, nsDictionary));



            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|-[btnSend(==20)]", NSLayoutFormatOptions.DirectionLeadingToTrailing, null, nsDictionary));



            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:[txtMessage]-[lblChat]-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, null, nsDictionary));

            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-0-[lblChat]-0-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, null, nsDictionary));





            txtMessage.Placeholder = "Enter message";

        }


    }


}