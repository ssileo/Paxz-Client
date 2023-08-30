using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaxzClient.commands
{
    public class SimpleCommand
    {
        public List<string> handlers;
        public string description;

        public virtual void execute(string cmd, string[] args) { }
    }
}
