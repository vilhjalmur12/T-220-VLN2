using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace CodeEditorApp.Hubs
{
    //Hub that handles the update on the editor.
    public class MyHub1 : Hub
    {
        //Group clients depending on documentID
        public void JoinDocument(int documentID)
        {
            Groups.Add(Context.ConnectionId, Convert.ToString(documentID));
        }
        //Every time there's a change in editor.
        public void OnChange(object changeData, string documentID)
        {
            Clients.Group(documentID, Context.ConnectionId).OnChange(changeData);
            //Clients.All.OnChange(changeData);
        }
        //Send message with info about who made the last changes         
        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);
        }
        public void comment(string name, string message)
        {
            Clients.All.addNewMessageToPage(name, message);
        }

        public void LeaveDocument(int documentID)
        {
            Groups.Remove(Context.ConnectionId, Convert.ToString(documentID));
        }

        public void SwapDocuments(int OldID, int NewID)
        {
            Groups.Remove(Context.ConnectionId, Convert.ToString(OldID));
            Groups.Add(Context.ConnectionId, Convert.ToString(NewID));
        }

    }
}