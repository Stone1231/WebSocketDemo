using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace WebSocketLibrary1
{
    public class MyWebsocket : WebSocketBehavior
    {
        private string _name;
        private static int _number = 0;
        private string _prefix;

        public MyWebsocket()
            : this(null)
        {
        }

        public MyWebsocket(string prefix)
        {
            _prefix = !prefix.IsNullOrEmpty() ? prefix : "anon#";
        }

        private string getName()
        {
            var name = Context.QueryString["name"];
            return !name.IsNullOrEmpty()
                   ? name
                   : (_prefix + getNumber());
        }

        private static int getNumber()
        {
            return Interlocked.Increment(ref _number);
        }

        protected override void OnOpen()
        {
            _name = getName();
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            //Sessions.Broadcast(String.Format("{0}: {1}", _name, e.Data));
            Sessions.SendTo(this.ID, e.Data);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Sessions.Broadcast(String.Format("{0} got logged off...", _name));
        }
    }
}
