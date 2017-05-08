using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace CodeEditorApp.Hubs
{
    public class MyHub1 : Hub
    {
        public void JoinDocument(int documentID)
        {
            Groups.Add(Context.ConnectionId, Convert.ToString(documentID));
        }
        public void OnChange(object changeData, int documentID)
        {
            Clients.Group(Convert.ToString(documentID), Context.ConnectionId).OnChange(changeData);
            //Clients.All.OnChange(changeData);
        }
        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);
        }
    }
}