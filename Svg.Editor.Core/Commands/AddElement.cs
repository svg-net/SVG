using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Svg.Core.Commands
{
    public class AddElement : ICommand
    {
        private readonly List<object> _graphicsList;
        private readonly object _element;

        public AddElement(List<object> graphicsList, object element)
        {
            _graphicsList = graphicsList;
            _element = element;
        }

        public void Execute()
        {
            _graphicsList.Add(_element);
        }

        public void Undo()
        {
            _graphicsList.Remove(_element);
        }
    }
}
