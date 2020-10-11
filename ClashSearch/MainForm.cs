using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.DocumentParts;
using System;

namespace ClashSearch
{
    public partial class MainForm : Form
    {
        private Document _mainDoc;
        private DocumentSavedViewpoints _viewPointsData;

        private List<SavedItem> _allChild;

        public MainForm(Document mainDoc)
        {
            InitializeComponent();
            _mainDoc = mainDoc;
            _viewPointsData = _mainDoc.SavedViewpoints;
            _allChild = new List<SavedItem>();

            listBox1.DisplayMember = "DisplayName";
        }

        private void AllChield(SavedItem s)
        {
            if (s.IsGroup)
            {
                GroupItem g = s as GroupItem;
                foreach(SavedItem i in g.Children)
                {
                    if(!i.IsGroup)
                    {
                        _allChild.Add(i);
                    }
                    AllChield(i);
                }
            } 
        }

        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {
            listBox1.Items.Clear(); 

            _allChild.Clear();

            if (this.textBox1.Text != "")
            {
                foreach (SavedItem si in _viewPointsData.Value)
                {
                    AllChield(si);
                }

                foreach (SavedItem si in _allChild.Where(x => x.DisplayName.Contains(this.textBox1.Text)))
                {
                    listBox1.Items.Add(si);
                    if (listBox1.Items.Count > 100) break;
                }
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedIndex != -1)
                {
                    int indx = listBox1.SelectedIndex;
                    _viewPointsData.CurrentSavedViewpoint = _allChild.First(x => x.Equals(listBox1.Items[indx]));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
