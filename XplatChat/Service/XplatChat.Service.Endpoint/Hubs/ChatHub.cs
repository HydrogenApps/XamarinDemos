using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using WebGrease.Css.Extensions;
using XplatChat.Contract.Pcl;

namespace XplatChat.Service.Endpoint.Hubs
{
    public class ChatHub : Hub
    {
        private static Dictionary<string,XplatUser> _xplatUsers = new Dictionary<string, XplatUser>();
        private static Dictionary<string, string> _connectionMapping = new Dictionary<string, string>();
        private static ConcurrentQueue<XplatEvent> _recentEvents = new ConcurrentQueue<XplatEvent>();

        private static object _lock = new object();

        public void Join(JoinRequest request)
        {
            var user = new XplatUser
            {
                WhenJoined = DateTime.Now,
                Name = request.Name
            };

            lock (_lock)
            {
                _xplatUsers[user.Name] = user;
                _connectionMapping[Context.ConnectionId] = user.Name;                
            }

            PublishNewXplatEvent(new XplatEvent
            {
                EventType = XplatEventTypeEnum.UserJoined,
                WhenOccurred = DateTime.Now,
                Name = request.Name
            });
        }

        public void SendMessage(SendMessageRequest request)
        {
            XplatUser user = null;
            lock (_lock)
            {
                if(_xplatUsers.ContainsKey(request.Name))
                    user = _xplatUsers[request.Name];
            }

            if (user != null)
            {

                PublishNewXplatEvent(new XplatEvent
                {
                    EventType = XplatEventTypeEnum.Message,
                    WhenOccurred = DateTime.Now,
                    Name = request.Name,
                    Message = request.Message
                });
            }
        }

        public override Task OnConnected()
        {
            _recentEvents.ToArray().ForEach(xplatEvent => Clients.Caller.NewXplatEvent(xplatEvent));
            Clients.Caller.NewXplatEvent(new XplatEvent { EventType = XplatEventTypeEnum.Message, Name = "System", Message = "Welcome to Xplat Chat!", WhenOccurred = DateTime.Now });
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            DoOnDisconnected();
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnDisconnected()
        {
            DoOnDisconnected();
            return base.OnDisconnected();
        }
        public void DoOnDisconnected()
        {

            lock (_lock)
            {
                if (_connectionMapping.ContainsKey(Context.ConnectionId))
                {
                    string userName = _connectionMapping[Context.ConnectionId];
                    _connectionMapping.Remove(Context.ConnectionId);

                    if (_xplatUsers.ContainsKey(userName))
                    {
                        _xplatUsers.Remove(userName);
                        PublishNewXplatEvent(new XplatEvent
                        {
                            EventType = XplatEventTypeEnum.UserLeft,
                            WhenOccurred = DateTime.Now,
                            Name = userName
                        });
                    }
                }
            }
        }

        private void PublishNewXplatEvent(XplatEvent xplatEvent)
        {
            _recentEvents.Enqueue(xplatEvent);
            Clients.All.NewXplatEvent(xplatEvent);

            while (_recentEvents.Count > 10)
            {
                XplatEvent dequeuedEvent;
                _recentEvents.TryDequeue(out dequeuedEvent);
            }
        }
    }
}