using System;
using MvvmCross.Core.ViewModels;

namespace Svg.Droid.SampleEditor.Core.ViewModels
{
    public class EditorViewModel 
        : MvxViewModel
    {
        public event Action GridVisibilityChanged;
        public event Action DeleteSelectedElement;

        public IMvxCommand ToggleGridCommand
        {
            get { return new MvxCommand(ToggleGrid, CanToggleGrid);}
        }

        private void ToggleGrid()
        {
            GridVisibilityChanged?.Invoke();
        }

        private bool CanToggleGrid()
        {
            return true;
        }


        public IMvxCommand DeleteSelectionCommand
        {
            get { return new MvxCommand(DeleteSelection, CanDeleteSelection); }
        }

        private void DeleteSelection()
        {

        }

        private bool CanDeleteSelection()
        {
            return true;
        }
        

        public IMvxCommand AddTextCommand
        {
            get { return new MvxCommand(AddText, CanAddText); }
        }

        private void AddText()
        {
        }

        private bool CanAddText()
        {
            // if current selection is text, this should be "edit text"!!
            return true;
        }
    }
}
