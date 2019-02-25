using Microsoft.Win32;
using SearchWindowsRegistry.BL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SearchWindowsRegistry
{
    public partial class Form1 : Form
    {
        public RegistrySearcher RegistrySearcher { get; set; }

        public Form1()
        {
            InitializeComponent();

            RegistrySearcher = new RegistrySearcher();
            RegistrySearcher.FoundRegistry += RegistrySearcher_FoundRegistry;
            RegistrySearcher.Finished += RegistrySearcher_Finished;
        }

        private void RegistrySearcher_FoundRegistry(object sender, FoundRegistryItem e)
        {
            this.AddKeyToList(e);
        }

        private void RegistrySearcher_Finished(object sender, EventArgs e)
        {
            this.ResetButtons();
        }

        private void ResetButtons()
        {
            //if calling this method from a thread
            if (this.InvokeRequired)
            {
                //invoke this method from the thread the control is in
                this.Invoke(new Action(ResetButtons));
                return;
            }

            btnSearch.Enabled = true;
            btnCancel.Visible = false;
        }

        public void AddKeyToList(FoundRegistryItem regItem)
        {
            //if calling this method from a thread
            if (this.InvokeRequired)
            {
                //invoke this method from the thread the control is in
                this.Invoke(new Action<FoundRegistryItem>(AddKeyToList), regItem);
                return;
            }

            lstResults.Items.Add(regItem);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lstResults.Items.Clear();

            btnSearch.Enabled = false;
            btnCancel.Visible = true;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            RegistrySearcher.StartSearch(txtSearchTerm.Text, rbKey.Checked, rbValue.Checked);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            RegistrySearcher.Cancel();
            btnCancel.Visible = false;
        }
    }
}
