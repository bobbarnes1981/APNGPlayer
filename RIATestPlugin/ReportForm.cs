using System;
using System.Windows.Forms;

namespace RIATestPlugin
{
    public partial class ReportForm : Form
    {
        private RIATestPluginController m_controller;

        public ReportForm(RIATestPluginController controller)
        {
            InitializeComponent();
            m_controller = controller;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog(this) == DialogResult.OK)
            {
                m_controller.LoadReport(fileDialog.FileName);
                treeViewReport.Nodes.Clear();
                treeViewReport.Nodes.Add(m_controller.GenerateTreeNode());
            }
        }

        private void treeViewReport_DoubleClick(object sender, EventArgs e)
        {
            TreeNode selectedNode = treeViewReport.SelectedNode;
            if (selectedNode != null)
            {
                if (selectedNode.Tag != null)
                {
                    RIATestLibrary.Message message = (RIATestLibrary.Message)selectedNode.Tag;

                    TimeSpan elapsedTime = m_controller.GetTimeToMessage(message);
                    if (MessageBox.Show(this, string.Format("Skip to {0}?", elapsedTime), "",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        m_controller.PlayToMessage(message);
                    }
                }
            }
        }
    }
}
