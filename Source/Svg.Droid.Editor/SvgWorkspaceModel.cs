using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Svg.Core.Commands;
using Svg.Droid.Editor.Interfaces;
using Svg.Droid.Editor.Services;
using Svg.Droid.Editor.Tools;

namespace Svg.Droid.Editor
{
    public class SvgWorkspaceModel
    {
        public List<SelectableAndroidBitmap> Elements { get; } = new List<SelectableAndroidBitmap>();
        private CommandService CommandService { get; } = new CommandService();

        public ISelectionService SelectionService { get; } = new SelectionService();
        public Action<bool> SelectedItemChanged;

        private IToolService ToolService { get; } = new ToolService();
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
            SelectionService.SelectedItem = relevantSvg;
            SelectedItemChanged?.Invoke(SelectionService.SelectedItem != null);
        }

        public SelectableAndroidBitmap UpdateSelectedItem(float x, float y)
        {
            SelectionService.SelectedItem = SelectionService.SelectNewItem(Elements, (int) x, (int) y);
            SelectedItemChanged?.Invoke(SelectionService.SelectedItem != null);

            return SelectionService.SelectedItem;
        }

        public void UnselectAll()
        {
            SelectionService.SelectedItem = null;
            SelectedItemChanged?.Invoke(false);
        }

        public bool IsInRangeOfSelected(float x, float y)
        {
            if (SelectionService.SelectedItem == null)
                return false;

            return SelectionService.IsInRangeOfSelected(SelectionService.SelectedItem, (int) x, (int) y);
        }
    }
}