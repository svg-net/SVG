using System.Collections.Generic;
using Svg.Droid.Editor.Tools;

namespace Svg.Droid.Editor.Services
{
    public class ToolService
    {
        public IEnumerable<ITool> Tools { get; } = new List<ITool>()
        {
            //-------------------------------------------
            // ADD YOUR TOOLS HERE
            //-------------------------------------------
            // Warning: The order of the Tools is relevant
            //          Tools on top influence the tools below
            //-------------------------------------------

            SharedMasterTool.Instance, // Used for general tool infos and inter tool communication

            //new SnappingTool(),
            new ZoomTool(),
            new GridTool(),
            new SelectionTool(),
            new MoveSvgTool(),
            new PanTool(),
            //-------------------------------------------
        };
    }
}