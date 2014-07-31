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

namespace XplatChat.NativeApp.iPhone.Views
{

    public class MainViewController : UIViewController
    {

        public override void ViewDidLoad()
        {

            base.ViewDidLoad();

            SetupView();

            var hubConnection = new HubConnection("http://10.37.129.4/XplatChat.Service.Endpoint"); //CHECK IP ADDRESS!
            var chatHubProxy = hubConnection.CreateHubProxy("ChatHub");

            chatHubProxy.On<XplatEvent>("NewXplatEvent", xplatEvent =>
            {
                UIApplication.SharedApplication.InvokeOnMainThread(() =>
                {
                    string chat = "";
                    switch (xplatEvent.EventType)
                    {
                        case XplatEventTypeEnum.UserLeft:
                            chat = string.Format("{0:hh:mm:ss} User {1} left", xplatEvent.WhenOccurred, xplatEvent.Name);
                            break;
                        case XplatEventTypeEnum.UserJoined:
                            chat = string.Format("{0:hh:mm:ss} User {1} joined", xplatEvent.WhenOccurred, xplatEvent.Name);
                            break;
                        case XplatEventTypeEnum.Message:
                            chat = string.Format("{0:hh:mm:ss} {1}: {2}", xplatEvent.WhenOccurred, xplatEvent.Name, xplatEvent.Message);
                            break;
                    }
                    txtChat.Text = string.Format("{0}\n{1}", chat, txtChat.Text);
                });
            });

            btnSend.TouchUpInside += (sender, args) =>
            {
                chatHubProxy.Invoke("SendMessage", new SendMessageRequest { Name = "Tim", Message = txtMessage.Text });
                txtMessage.Text = "";
            };

            hubConnection.Start().ContinueWith((result) => chatHubProxy.Invoke("Join", new JoinRequest { Name = "Tim" }));

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