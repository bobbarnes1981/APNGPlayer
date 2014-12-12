using System.Windows.Forms;
using APNGLibrary;

namespace APNGPlayerLibrary
{
    /// <summary>
    /// IPlugin interface
    /// </summary>
    public interface IPlugin
    {
        void SetStreamer(IStreamer streamer);
        ToolStripMenuItem GetToolStripMenuItem();
    }
}
