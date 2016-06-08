using Android.App;
using Android.OS;
using Android.Views;
using MvvmCross.Droid.Views;
using Svg.Droid.Editor;
using Svg.Droid.SampleEditor.Core.ViewModels;

namespace Svg.Droid.SampleEditor.Views
{
    [Activity(Label = "Edit SVG")]
    public class EditorView : MvxActivity
    {
        private SvgWorkspace _padView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.EditorVIew);
            _padView = FindViewById<SvgWorkspace>(Resource.Id.pad);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.editor, menu);

            //m_MenuItem_EditQuote = menu.FindItem(Resource.Id.menu_EditQuote);
            //m_MenuItem_EditQuote.SetVisible(ViewModel.CanEdit);

            return true;
        }

        protected override void OnResume()
        {
            ViewModel.GridVisibilityChanged += ViewModelGridVisibilityChanged;
            ViewModel.DeleteSelectedElement += ViewModelOnDeleteSelectedElement;

            base.OnResume();
        }
        
        private void ViewModelOnDeleteSelectedElement()
        {
            _padView.RemoveSelectedItem();
        }

        protected override void OnPause()
        {
            ViewModel.GridVisibilityChanged -= ViewModelGridVisibilityChanged;
            ViewModel.DeleteSelectedElement -= ViewModelOnDeleteSelectedElement;

            base.OnPause();
        }

        public new EditorViewModel ViewModel
        {
            get { return (EditorViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.delete:
                    ViewModel.DeleteSelectionCommand.Execute(null);
                    return true;
                case Resource.Id.add_text:
                    ViewModel.AddTextCommand.Execute(null);
                    return true;
                case Resource.Id.show_grid:
                    ViewModel.ToggleGridCommand.Execute(null);
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void ViewModelGridVisibilityChanged()
        {
            _padView.ToggleGridVisibility();
        }
    }
}
