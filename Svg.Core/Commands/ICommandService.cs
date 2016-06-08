namespace Svg.Core.Commands
{
    public interface ICommandService
    {
        bool Execute(ICommand command);
        bool Undo();
    }
}