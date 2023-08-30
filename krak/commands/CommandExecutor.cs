using System;
using System.Collections.Generic;
using PaxzClient.commands;

namespace PaxzClient.commands
{
    public static class CommandExecutor
    {

        public static readonly string prefix = "/";

        public static List<ToggleCommand> toggleCommands = new List<ToggleCommand>();
        public static List<SimpleCommand> simpleCommands = new List<SimpleCommand>();

        public static void LoadCommands()
        {
            // LoadSimpleCommands();
            
            
        }

        private static void LoadSimpleCommands()
        {
            //MakeSimple(new TestCommand(), new List<string>() { "test" }, "ForTests");
            
        }
        private static void MakeSimple(SimpleCommand cmd, List<string> handlers, string description = "")
        {
            cmd.handlers = handlers;
            cmd.description = description;
            simpleCommands.Add(cmd);
        }

        public static void MakeToggle(bool state, List<string> handlers, string description = "")
        {
            ToggleCommand cmd = new ToggleCommand();
            cmd.handlers = handlers;
            cmd.state = state;
            cmd.description = description;
            toggleCommands.Add(cmd);
        }

        public static bool Execute(string cmd)
        {
            string[] args = cmd.Split(' ');

            string cmd_full = cmd;

            cmd = args[0].ToLower();

            if (cmd.StartsWith(prefix))
            {
                var commands = toggleCommands;

                foreach (var command in commands)
                {
                    foreach (var handler in command.handlers)
                    {
                        if (prefix + handler == cmd)
                        {
                            command.state = !command.state;
                            //Paxz.SendMessage((command.state.Value ? TextFormat.LIME : TextFormat.RED) + $"{handler}: " + (command.state.Value ? "True" : "False"));
                            //Paxz.Logger.LogMessage("CommandExecutor -> Executed: " + cmd);
                            return true;
                        }
                    }
                }

                var command2 = simpleCommands;
                foreach (var command in command2)
                {
                    foreach (var handler in command.handlers)
                    {
                        if (prefix + handler == cmd)
                        {
                            command.execute(cmd_full, args);
                            //Paxz.Logger.LogMessage("CommandExecutor -> Executed: " + cmd);
                            return true;
                        }
                    }
                }


               // Paxz.SendMessage($"{TextFormat.RED}Unknown command. Try {prefix}help for a list of commands");

               // Paxz.Logger.LogError("CommandExecutor -> Unknown command: " + cmd);
                return true;
            }
            return false;
        }
    }
}
