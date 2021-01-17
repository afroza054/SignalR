using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace Only_Persistent_Work_02
{
    public class ChatConnection : PersistentConnection
    {
        public static Dictionary<string, string> users = new Dictionary<string, string>();
        protected override Task OnConnected(IRequest request, string connectionId)
        {

            return Connection.Broadcast(new MessageData { MessageType = "message", From = "SERVER", Data = "New Guest Connected." });
        }
        protected override Task OnDisconnected(IRequest request, string connectionId, bool stopCalled)
        {
            users.Remove(connectionId);
            var toSend = new MessageData { MessageType = "updateUsers", From = "SERVER", Data = "", List = users.Values.ToArray() };
            Connection.Broadcast(toSend);
            return base.OnDisconnected(request, connectionId, stopCalled);
        }
        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            MessageData m = JsonConvert.DeserializeObject<MessageData>(data);
            MessageData toSend;
            switch (m.MessageType.ToLower())
            {
                case "addme":
                    users.Add(connectionId, m.From);
                    toSend = new MessageData { MessageType = "updateUsers", From = "SERVER", Data = "", List = users.Values.ToArray() };
                    break;
                case "message":
                    toSend = new MessageData { MessageType = "message", From = users[connectionId], Data = m.Data };
                    break;
                default:
                    toSend = new MessageData();
                    break;
            }
            return Connection.Broadcast(toSend);
        }

    }
    public class MessageData
    {
        public string MessageType { get; set; }
        public string From { get; set; }
        public string Data { get; set; }
        public string[] List { get; set; }
    }
}