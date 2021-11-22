// *****************************************************************************
// 
//  © Component Factory Pty Ltd 2012. All rights reserved.
//	The software and associated documentation supplied hereunder are the 
//  proprietary information of Component Factory Pty Ltd, PO Box 1504, 
//  Glen Waverley, Vic 3150, Australia and are supplied subject to licence terms.
// 
//  Version 4.6.0.0 	www.ComponentFactory.com
// *****************************************************************************

using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ComponentFactory.Krypton.Toolkit;
using Microsoft.Win32;
using System.IO;

namespace KryptonFormExamples
{
    public partial class Form1 : KryptonForm
    {
        public Form1()
        {
            InitializeComponent();
            Home.BringToFront();
            try
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);
                string friendlyName = AppDomain.CurrentDomain.FriendlyName;
                if (registryKey.GetValue(friendlyName) == null)
                {
                    registryKey.SetValue(friendlyName, 11001, RegistryValueKind.DWord);
                }
                this.webBrowser1.Url = new Uri(string.Format("file:///{0}/bin/Monaco.html", Application.StartupPath));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Monaco Editor! Error: " + ex.Message + "\nThis error has been copied. Please send to Zesty.", "", MessageBoxButtons.OK);
                Clipboard.SetText(ex.Message);
                Application.ExitThread();
            }
            this.scriptBoxLoad();
        }

        private void scriptBoxLoad()
        {
            this.listBox1.Items.Clear();
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.StartupPath + "/scripts");
            FileInfo[] files = directoryInfo.GetFiles("*.txt");
            FileInfo[] files2 = directoryInfo.GetFiles("*.lua");
            foreach (FileInfo fileInfo in files)
            {
                this.listBox1.Items.Add(fileInfo.Name);
            }
            foreach (FileInfo fileInfo2 in files2)
            {
                this.listBox1.Items.Add(fileInfo2.Name);
            }
        }

        private void kryptonCheckSetPalettes_CheckedButtonChanged(object sender, EventArgs e)
        {
            // Recalc the non client size to reflect new border style
            RecalcNonClient();

            switch (kryptonCheckSetPalettes.CheckedIndex)
            {
                case 0:
                    kryptonManager.GlobalPaletteMode = PaletteModeManager.ProfessionalSystem;
                    break;
                case 1:
                    kryptonManager.GlobalPaletteMode = PaletteModeManager.Office2007Black;
                    break;
                case 2:
                    kryptonManager.GlobalPaletteMode = PaletteModeManager.Office2007Silver;
                    break;
                case 3:
                    kryptonManager.GlobalPaletteMode = PaletteModeManager.Office2007Blue;
                    break;
                case 4:
                    kryptonManager.GlobalPaletteMode = PaletteModeManager.ProfessionalOffice2003;
                    break;
                case 5:
                    kryptonManager.GlobalPaletteMode = PaletteModeManager.SparkleBlue;
                    break;
                case 6:
                    kryptonManager.GlobalPaletteMode = PaletteModeManager.SparkleOrange;
                    break;
                case 7:
                    kryptonManager.GlobalPaletteMode = PaletteModeManager.Office2010Black;
                    break;
                case 8:
                    kryptonManager.GlobalPaletteMode = PaletteModeManager.Office2010Silver;
                    break;
                case 9:
                    kryptonManager.GlobalPaletteMode = PaletteModeManager.Office2010Blue;
                    break;
            }
        }

        private void kryptonCheckSetStyles_CheckedButtonChanged(object sender, EventArgs e)
        {
            switch (kryptonCheckSetStyles.CheckedIndex)
            {
                case 0:
                    FormBorderStyle = FormBorderStyle.Sizable;
                    break;
                case 1:
                    FormBorderStyle = FormBorderStyle.FixedToolWindow;
                    break;
                case 2:
                    FormBorderStyle = FormBorderStyle.SizableToolWindow;
                    break;
                case 3:
                    FormBorderStyle = FormBorderStyle.FixedDialog;
                    break;
                case 4:
                    FormBorderStyle = FormBorderStyle.Fixed3D;
                    break;
                case 5:
                    FormBorderStyle = FormBorderStyle.FixedSingle;
                    break;
                case 6:
                    FormBorderStyle = FormBorderStyle.None;
                    break;
            }

            // Recalc the non client size to reflect new border style
            RecalcNonClient();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set correct initial checked button
            if (KryptonManager.CurrentGlobalPalette == KryptonManager.PaletteOffice2007Black)
                kryptonCheckSetPalettes.CheckedIndex = 1;

            //// Setup the property grid to edit this form
            //propertyGrid.SelectedObject = new KryptonFormProxy(this);
        }

        private void kryptonOffice2007Blue_Click(object sender, EventArgs e)
        {

        }

        private void sixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Setting.BringToFront();
        }

        private void kryptonHeaderGroup1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void kryptonHeaderGroup1_Panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void kryptonCheckButton5_Click(object sender, EventArgs e)
        {
            Home.BringToFront();
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            //Execute ur shit
            HtmlDocument document = this.webBrowser1.Document;
            string scriptName = "GetText";
            object[] array = new string[0];
            object[] args = array;
            string script = document.InvokeScript(scriptName, args).ToString();
            new WeAreDevs_API.ExploitAPI().SendLuaScript(script);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            this.webBrowser1.Document.InvokeScript("SetText", new object[]
{
                ""
});
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Lua File (*.lua)|*.lua|Text File (*.txt)|*.txt";
            saveFileDialog.Title = "Save Script";
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            try
            {
                HtmlDocument document = this.webBrowser1.Document;
                string scriptName = "GetText";
                object[] array = new string[0];
                object[] args = array;
                string value = document.InvokeScript(scriptName, args).ToString();
                StreamWriter streamWriter = new StreamWriter(File.Create(saveFileDialog.FileName));
                streamWriter.Write(value);
                streamWriter.Dispose();
            }
            catch
            {
                MessageBox.Show("An unexpected error occured!", "Oof");
            }
        }

        private OpenFileDialog open = new OpenFileDialog
        {
            Filter = "Lua Script (*.txt)|*.txt|Lua Script (*.lua)|*.lua",
            FilterIndex = 1,
            Title = "Open Script"
        };

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            if (this.open.ShowDialog() == DialogResult.OK)
            {
                string text = File.ReadAllText(this.open.FileName);
                HtmlDocument document = this.webBrowser1.Document;
                string scriptName = "SetText";
                object[] array = new string[]
                {
                    text
                };
                object[] args = array;
                document.InvokeScript(scriptName, args);
            }
        }

        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            HtmlDocument document = this.webBrowser1.Document;
            string scriptName = "GetText";
            object[] array = new string[0];
            object[] args = array;
            string script = document.InvokeScript(scriptName, args).ToString();
            Clipboard.SetText(script);
            this.webBrowser1.Document.InvokeScript("SetText", new object[]
{
                ""
});
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            HtmlDocument document = this.webBrowser1.Document;
            string scriptName = "GetText";
            object[] array = new string[0];
            object[] args = array;
            string script = document.InvokeScript(scriptName, args).ToString();
            Clipboard.SetText(script);
        }

        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            string text = Clipboard.GetText();
            HtmlDocument document = this.webBrowser1.Document;
            string scriptName = "SetText";
            object[] array = new string[]
            {
                    text
            };
            object[] args = array;
            document.InvokeScript(scriptName, args);
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            object selectedItem = this.listBox1.SelectedItem;
            if (selectedItem != null)
            {
                if (File.Exists(Application.StartupPath + "\\scripts\\" + selectedItem.ToString()))
                {
                    string text = File.ReadAllText(Application.StartupPath + "\\scripts\\" + selectedItem.ToString());
                    HtmlDocument document = this.webBrowser1.Document;
                    string scriptName = "SetText";
                    object[] array = new string[]
                    {
                        text
                    };
                    object[] args = array;
                    document.InvokeScript(scriptName, args);
                    return;
                }
                this.listBox1.Items.Remove(this.listBox1.SelectedItem);
            }
        }
    }

    public class KryptonFormProxy
    {
        private KryptonForm _form;

        public KryptonFormProxy(KryptonForm form)
        {
            _form = form;
        }

        [Category("Appearance")]
        [Description("The text associated with the control.")]
        [DefaultValue("")]
        public string Text
        {
            get { return _form.Text; }
            set { _form.Text = value; }
        }

        [Category("Appearance")]
        [Description("The extra text associated with the control.")]
        [DefaultValue("")]
        public string TextExtra
        {
            get { return _form.TextExtra; }
            set { _form.TextExtra = value; }
        }

        [Category("Appearance")]
        [Description("The icon associated with the control.")]
        [DefaultValue("")]
        public Icon Icon
        {
            get { return _form.Icon; }
            set { _form.Icon = value; }
        }

        [Category("Visuals")]
        [Description("Should custom chrome be allowed for this KryptonForm instance.")]
        [DefaultValue(true)]
        public bool AllowFormChrome
        {
            get { return _form.AllowFormChrome; }
            set { _form.AllowFormChrome = value; }
        }

        [Category("Visuals")]
        [Description("Should the form status strip be considered for merging into chrome.")]
        [DefaultValue(true)]
        public bool AllowStatusStripMerge
        {
            get { return _form.AllowStatusStripMerge; }
            set { _form.AllowStatusStripMerge = value; }
        }

        [Category("Visuals")]
        [Description("Header style for a main form.")]
        [DefaultValue(typeof(HeaderStyle), "Form")]
        public HeaderStyle HeaderStyle
        {
            get { return _form.HeaderStyle; }
            set { _form.HeaderStyle = value; }
        }

        [Category("Visuals")]
        [Description("Chrome group border style.")]
        [DefaultValue(typeof(PaletteBorderStyle), "FormMain")]
        public PaletteBorderStyle GroupBorderStyle
        {
            get { return _form.GroupBorderStyle; }
            set { _form.GroupBorderStyle = value; }
        }

        [Category("Visuals")]
        [Description("Chrome group background style.")]
        [DefaultValue(typeof(PaletteBackStyle), "FormMain")]
        public PaletteBackStyle GroupBackStyle
        {
            get { return _form.GroupBackStyle; }
            set { _form.GroupBackStyle = value; }
        }

        [Category("Visuals")]
        [Description("Overrides for defining common form appearance that other states can override.")]
        public PaletteFormRedirect StateCommon
        {
            get { return _form.StateCommon; }
        }

        [Category("Visuals")]
        [Description("Overrides for defining inactive form appearance.")]
        public PaletteForm StateInactive
        {
            get { return _form.StateInactive; }
        }

        [Category("Visuals")]
        [Description("Overrides for defining active form appearance.")]
        public PaletteForm StateActive
        {
            get { return _form.StateActive; }
        }

        [Category("Visuals")]
        [Description("Collection of button specifications.")]
        public KryptonForm.FormButtonSpecCollection ButtonSpecs
        {
            get { return _form.ButtonSpecs; }
        }

        [Category("Window Style")]
        [DefaultValue(true)]
        public bool ControlBox
        {
            get { return _form.ControlBox; }
            set { _form.ControlBox = value; }
        }

        [Category("Window Style")]
        [DefaultValue(true)]
        public bool MaximizeBox
        {
            get { return _form.MaximizeBox; }
            set { _form.MaximizeBox = value; }
        }

        [Category("Window Style")]
        [DefaultValue(true)]
        public bool MinimizeBox
        {
            get { return _form.MinimizeBox; }
            set { _form.MinimizeBox = value; }
        }

        [Category("Window Style")]
        [DefaultValue(true)]
        public bool ShowIcon
        {
            get { return _form.ShowIcon; }
            set { _form.ShowIcon = value; }
        }
    }
}
