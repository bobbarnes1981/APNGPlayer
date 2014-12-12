using System;
using System.Windows.Forms;
using APNGLibrary;
using APNGPlayerLibrary;

namespace RIATestPlugin
{
    /// <summary>
    /// RIATest Plugin for APNG Player
    /// </summary>
    public class RIATestPlugin : IPlugin, IDisposable
    {
        /// <summary>
        /// IStreamer object
        /// </summary>
        private IStreamer m_streamer;

        /// <summary>
        /// Report form
        /// </summary>
        private ReportForm m_form;

        /// <summary>
        /// Get toolstrip menu item for plugin
        /// </summary>
        /// <returns></returns>
        public ToolStripMenuItem GetToolStripMenuItem()
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem("RIATestReportPlugin");
            menuItem.Click += MenuItemOnClick;
            return menuItem;
        }

        /// <summary>
        /// Set IStreamer method
        /// </summary>
        /// <param name="streamer"></param>
        public void SetStreamer(IStreamer streamer)
        {
            m_streamer = streamer;
        }

        /// <summary>
        /// Menu item click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void MenuItemOnClick(object sender, EventArgs eventArgs)
        {
            if (m_form == null)
            {
                m_form = new ReportForm(new RIATestPluginController(m_streamer));
            }
            m_form.Show();
        }

        public void Dispose()
        {
            if (m_form != null)
            {
                m_form.Dispose();
            }
        }
    }
}
