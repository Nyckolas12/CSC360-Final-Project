using System.Collections.Generic;

namespace ShapesCanvas.Commands
{
    /// <summary>
    /// Invoker in the Command pattern. Executes commands and remembers them
    /// on an undo stack so the last action can be reversed.
    /// </summary>
    public class CommandInvoker
    {
        private readonly Stack<IShapeCommand> _history = new();

        public void Run(IShapeCommand command)
        {
            command.Execute();
            _history.Push(command);
        }

        public bool CanUndo => _history.Count > 0;

        public void Undo()
        {
            if (_history.Count == 0) return;
            _history.Pop().Undo();
        }
    }
}
