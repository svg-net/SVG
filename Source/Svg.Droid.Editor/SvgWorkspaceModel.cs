using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Svg.Core.Commands;
using Svg.Droid.Editor.Services;
using Svg.Droid.Editor.Tools;

namespace Svg.Droid.Editor
{
    public class SvgWorkspaceModel
    {
        public List<SelectableAndroidBitmap> Elements { get; } = new List<SelectableAndroidBitmap>();
        private CommandService CommandService { get; } = new CommandService();
        private SelectionService SelectionService { get; } = new SelectionService();
        private ToolService ToolService { get; } = new ToolService();

        public static SelectableAndroidBitmap SelectedItem { get; set; }
        public Action<bool> SelectedItemChanged;

        public IEnumerable<ITool> Tools => ToolService.Tools;

        public SvgWorkspaceModel()
        {
            foreach (var tool in Tools)
                tool.Reset();
        }

        public void AddSvg(SvgDocument svg)
        {

        }
        
        public void AddCommand(Action action)
        {
            
        }

        public void Select(SelectableAndroidBitmap relevantSvg)
        {
            SelectedItem = relevantSvg;
            SelectedItemChanged?.Invoke(SelectedItem != null);
        }

        public SelectableAndroidBitmap UpdateSelectedItem(float x, float y)
        {
            SelectedItem = SelectionService.SelectNewItem(Elements, (int) x, (int) y);
            SelectedItemChanged?.Invoke(SelectedItem != null);

            return SelectedItem;
        }

        public void UnselectAll()
        {
            SelectedItem = null;
            SelectedItemChanged?.Invoke(false);
        }

        public bool IsInRangeOfSelected(float x, float y)
        {
            if (SelectedItem == null)
                return false;

            return SelectionService.IsInRangeOfSelected(SelectedItem, (int) x, (int) y);
        }
    }
}