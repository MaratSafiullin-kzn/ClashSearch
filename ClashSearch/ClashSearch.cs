using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.DocumentParts;
using Autodesk.Navisworks.Api.Plugins;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

using ComponentManager = Autodesk.Windows.ComponentManager;
using IWin32Window = System.Windows.Forms.IWin32Window;

namespace ClashSearch
{
    /// <summary>
    /// Главная точка входа для приложения.
    /// </summary>
    [PluginAttribute("ClashSearch",  //Plugin name
                     "Марат Сафиуллин, АО Казанский Гипронииавиапром",           //for character Developer ID or GUID
        ToolTip = "Поиск", //The tooltip for the item in the ribbon
        DisplayName = "Поиск")] //Display name for the Plugin in the Ribbon

    class ClashViewSearch : AddInPlugin
    {
        public MainForm mf = null;
        private Document _mainDoc = null;

        public override int Execute(params string[] parameters)
        {
            try
            {
                Autodesk.Navisworks.Api.Application.ActiveDocument.FileNameChanged += Application_FileNameChanged;

                if (mf == null || mf.IsDisposed)
                {
                    _mainDoc = Autodesk.Navisworks.Api.Application.MainDocument;

                    IWin32Window navis_window = new JtWindowHandle(ComponentManager.ApplicationWindow);

                    mf = new MainForm(_mainDoc);
                    mf.Show(navis_window);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return 0;
        }

        private void Application_FileNameChanged(object sender, EventArgs e)
        {
            if (mf != null)
            {
                mf.Dispose();
                mf.Close();
            }
        }
    }
}
