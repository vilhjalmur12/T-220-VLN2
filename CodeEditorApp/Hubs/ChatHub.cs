using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace CodeEditorApp.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);
        }
        
        //public Task JoinRoom(string projectID)
        //{
        //    return Groups.Add(Context.ConnectionId, projectID);
        //}

        //public Task LeaveRoom(string projectID)
        //{
        //    return Groups.Remove(Context.ConnectionId, projectID);
        //}
    }
}