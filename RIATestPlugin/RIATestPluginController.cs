using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using APNGLibrary;
using RIATestLibrary;

namespace RIATestPlugin
{
    /// <summary>
    /// RIATest Plugin Controller
    /// </summary>
    public class RIATestPluginController
    {
        /// <summary>
        /// Streamer
        /// </summary>
        private IStreamer m_streamer;

        /// <summary>
        /// Report
        /// </summary>
        private Report m_report;

        public RIATestPluginController(IStreamer streamer)
        {
            m_streamer = streamer;
        }

        /// <summary>
        /// Load report from file
        /// </summary>
        /// <param name="path"></param>
        public void LoadReport(string path)
        {
            // load report
            XmlSerializer serializer = new XmlSerializer(typeof(Report));
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                m_report = (Report) serializer.Deserialize(fileStream);
            }
        }

        /// <summary>
        /// Generate report treenode
        /// </summary>
        /// <returns></returns>
        public TreeNode GenerateTreeNode()
        {
            return GenerateTreeNode(m_report);
        }

        private TreeNode GenerateTreeNode(Report report)
        {
            TreeNode reportNode = new TreeNode(string.Format("Report [{0}] {1} {2} {3}", report.DateTime, report.Application, report.Version, report.Project));

            TreeNode startNode = new TreeNode("Startup");
            foreach (RIATestLibrary.Message message in report.Startup.Messages)
            {
                startNode.Nodes.Add(GenerateTreeNode(message));
            }
            reportNode.Nodes.Add(startNode);

            TreeNode groupsNode = new TreeNode("Scripts");
            foreach (Group group in report.Groups)
            {
                groupsNode.Nodes.Add(GenerateTreeNode(group));
            }
            reportNode.Nodes.Add(groupsNode);

            return reportNode;
        }

        private TreeNode GenerateTreeNode(RIATestLibrary.Message message)
        {
            TreeNode messageNode = new TreeNode(string.Format("[{0}] {1} {2}", message.DateTime, message.Type, message.Text));
            messageNode.Tag = message;
            switch (message.Type)
            {
                case MessageType.Error:
                    messageNode.ForeColor = Color.Red;
                    break;
                case MessageType.Verification:
                    messageNode.ForeColor = Color.Green;
                    break;
                case MessageType.Info:
                    messageNode.ForeColor = Color.Gray;
                    break;
                case MessageType.Trace:
                    messageNode.ForeColor = Color.LightSeaGreen;
                    break;
            }
            return messageNode;
        }

        private TreeNode GenerateTreeNode(Group group)
        {
            TreeNode groupNode = new TreeNode(group.Label);
            if (group.Scripts != null)
            {
                foreach (Script script in group.Scripts)
                {
                    groupNode.Nodes.Add(GenerateTreeNode(script));
                }
            }
            if (group.SubGroups != null)
            {
                foreach (Group subGroup in group.SubGroups)
                {
                    groupNode.Nodes.Add(GenerateTreeNode(subGroup));
                }
            }
            return groupNode;
        }

        private TreeNode GenerateTreeNode(Script script)
        {
            TreeNode scriptNode = new TreeNode(string.Format("[{0}] {1}", script.DateTime, script.File));
            if (script.Messages != null)
            {
                foreach (RIATestLibrary.Message message in script.Messages)
                {
                    scriptNode.Nodes.Add(GenerateTreeNode(message));
                }
            }
            return scriptNode;
        }

        /// <summary>
        /// Get time to message timestamp from report start
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public TimeSpan GetTimeToMessage(RIATestLibrary.Message message)
        {
            return message.DateTime - m_report.DateTime;
        }

        /// <summary>
        /// Play to message timestamp
        /// </summary>
        /// <param name="message"></param>
        public void PlayToMessage(RIATestLibrary.Message message)
        {
            m_streamer.Pause();
            //m_streamer.Speed = 0;
            m_streamer.PlayToElapsedTime(GetTimeToMessage(message));
        }
    }
}
