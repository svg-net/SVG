using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Svg.Core.Commands
{
    public class CommandService : ICommandService
    {
        private readonly Stack<ICommand> _commands; 

        public CommandService()
        {
            _commands = new Stack<ICommand>();
        }

        public bool Execute(ICommand command)
        {
            try
            {
                _commands.Push(command);
                command.Execute();
                return true;
            }
            catch (Exception exception)
            {
                // ExceptionPolicy? :)
                return false;
            }
        }

        public bool Undo()
        {
            try
            {
                var command = _commands.Pop();
                command.Undo();
                return true;
            }
            catch (Exception exception)
            {
                // ExceptionPolicy? :)
                return false;
            }
        }
    }
}
