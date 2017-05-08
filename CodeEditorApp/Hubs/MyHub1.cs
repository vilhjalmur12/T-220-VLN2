using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace CodeEditorApp.Hubs
{
    public class MyHub1 : Hub
    {
        public void OnChange(object changeData)
        {
            Clients.All.OnChange(changeData);
        }
    }
}