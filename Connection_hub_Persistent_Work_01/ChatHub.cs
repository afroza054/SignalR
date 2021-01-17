using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Connection_hub_Persistent_Work_01
{
    public class ChatHub : Hub
    {
        private static Dictionary<string, string> users = new Dictionary<string, string>();
        public static List<string> groups = new List<string> { "BBA 21", "MBA 34", "C# 43" };
        public override Task OnConnected()
        {
            Clients.All.message("SERVER", "new friend connected");
            return base.OnConnected();
        }
        public void CreateGroup(string groupName)
        {
            string username = users[Context.ConnectionId];
            if (!groups.Contains(groupName))
            {
                groups.Add(groupName);
                Clients.All.message("SERVER", $"new group created by {username}");
                Clients.All.updateGroups(groups);
            }
        }
        public void JoinToGroup(string groupName)
        {
            Groups.Add(Context.ConnectionId, groupName);
            string username = users[Context.ConnectionId];
            Clients.Client(Context.ConnectionId).message("SERVER", $"Joined group {groupName}");
            Clients.Client(Context.ConnectionId).joinedGroup(groupName);

        }
        public void LeaveFromGroup(string groupName)
        {
            Groups.Remove(Context.ConnectionId, groupName);
            string username = users[Context.ConnectionId];
            Clients.Client(Context.ConnectionId).message("SERVER", $"left group {groupName}");
            Clients.Client(Context.ConnectionId).leftGroup(groupName);
        }
        public void MessageToGroup(string groupName, string message)
        {
            var Date = DateTime.Now;
            string username = users[Context.ConnectionId];
            //Group Name And Date
            Clients.Group(groupName).message(username, message + $"<br/><b>From group {groupName}</b>" + $"<br/> {Date}");
        }
        public void AddMe(string username)
        {
            users.Add(Context.ConnectionId, username);

            Clients.All.updateList(users.Values.ToList());
            Clients.All.updateGroups(groups);

        }
        public void Send(string username, string message)
        {
            Clients.All.message(username, message);
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            string username = users[Context.ConnectionId];
            Clients.All.message("SERVER", $"{username} left");
            users.Remove(Context.ConnectionId);
            Clients.All.updateList(users.Values.ToList());
            return base.OnDisconnected(stopCalled);
        }
        public void uploadImage(string username, ImageData data)
        {


            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
            path = Path.Combine(path, data.Filename);
            data.Image = data.Image.Substring(data.Image.LastIndexOf(',') + 1);
            string converted = data.Image.Replace('-', '+');
            converted = converted.Replace('_', '/');
            byte[] buffer = Convert.FromBase64String(converted);
            FileStream fs = new FileStream($"{path}", FileMode.Create);
            fs.Write(buffer, 0, buffer.Length);
            fs.Close();
            Debug.WriteLine(data);
            Clients.All.message(username, $"<img src='/Images/{data.Filename}' class='rounded-circle' width='25' /> <a target='_blank' href='/Images/{data.Filename}'>download</a>");
        }

    }
    public class ImageData
    {

        public string Filename { get; set; }
        public string Image { get; set; }
    }
}