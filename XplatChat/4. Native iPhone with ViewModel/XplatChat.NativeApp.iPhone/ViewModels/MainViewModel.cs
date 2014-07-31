

using System;
using System.IO;
using System.Text;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Transports;
using XplatChat.Contract.Pcl;

namespace XplatChat.NativeApp.iPhone.ViewModels
{
    public class MainViewModel
    {
        private HubConnection _hubConnection;
        private IHubProxy _chatHubProxy;

        private string _Message;
        public event Action MessageChanged;

        public string Message
        {
            get { return _Message; }
            set
            {
                if (value != _Message)
                {
                    _Message = value;
                    if (MessageChanged != null)
                        MessageChanged();
                }
            }
        }


        private string _Chat;
        public event Action ChatChanged;

        public string Chat
        {
            get { return _Chat; }
            set
            {
                if (value != _Chat)
                {
                    _Chat = value;
                    if (ChatChanged != null)
                        ChatChanged();
                }
            }
        }

        private string _User;
        public event Action UserChanged;

        public string User
        {
            get { return _User; }
            set
            {
                if (value != _User)
                {
                    _User = value;
                    if (UserChanged != null)
                        UserChanged();
                }
            }
        }

        public async void Start(string userName)
        {
            User = userName;
            _hubConnection = new HubConnection("http://10.37.129.4/XplatChat.Service.Endpoint"); //CHECK IP ADDRESS!
            _chatHubProxy = _hubConnection.CreateHubProxy("ChatHub");

            _chatHubProxy.On<XplatEvent>("NewXplatEvent", xplatEvent =>
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
                Chat = string.Format("{0}\n{1}", chat, Chat);
            });
            await _hubConnection.Start(new LongPollingTransport()); //Long polling seemed to be more reliable on WP
            await _chatHubProxy.Invoke("Join", new JoinRequest { Name = User });
        }

        public async void Send()
        {
            await _chatHubProxy.Invoke("SendMessage", new SendMessageRequest { Name = User, Message = Message });
            Message = "";
        }
    }
}