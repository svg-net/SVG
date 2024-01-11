using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SvgW3CTestRunner
{
    public partial class ListSearchDialog : Form
    {
        private int _seletedTabIndex;
        private ListBox[] _listItems;

        public ListSearchDialog()
        {
            InitializeComponent();

            this.Load += OnFormDialogLoad;
        }

        public int SeletedTabIndex { get => _seletedTabIndex; set => _seletedTabIndex = value; }
        public ListBox[] ListItems { get => _listItems; set => _listItems = value; }

        private void OnFormDialogLoad(object sender, EventArgs e)
        {
            if (_listItems != null && _listItems.Length == 4)
            {
                comboBoxSelectTab.Items.Add("Pass W3C");
                comboBoxSelectTab.Items.Add("Fail W3C");
                comboBoxSelectTab.Items.Add("Pass Other");
                comboBoxSelectTab.Items.Add("Fail Other");

                comboBoxSelectTab.SelectedIndex = _seletedTabIndex;
                buttonSearch.Enabled = true;
                textBoxSearch.Enabled = true;
            }
        }

        private void OnSearch(object sender, EventArgs e)
        {
            if (_listItems == null || _listItems.Length != 4)
            {
                return;
            }

            var isFound = false;
            _seletedTabIndex = comboBoxSelectTab.SelectedIndex;
            var searchText = textBoxSearch.Text.Trim();
            if (searchText.Length == 0)
            {
                labelStatus.Text = "Text required: Enter the file name to search.";
                return;
            }
            if (!searchText.EndsWith(".svg"))
            {
                searchText += ".svg";
            }
            labelStatus.Text = $"Searching: {searchText}";

            var selectedItems = _listItems[_seletedTabIndex].Items;
            for (int index = 0; index < selectedItems.Count; index++)
            {
                var selectedItem = selectedItems[index];
                Console.WriteLine(selectedItem.ToString());
                if (searchText.Equals(selectedItem.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    _listItems[_seletedTabIndex].SelectedIndex = index;
                    isFound = true;
                    break;
                } 
            }

            if (isFound)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                labelStatus.Text = $"File name not found: {searchText}";
            }
        }
    }
}
