using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using APNGLibrary;
using APNGPlayerLibrary;

namespace APNGPlayer
{
    public partial class PlayerForm : Form
    {
        private IStreamer m_streamer;

        private IPlugin[] m_plugins;

        public PlayerForm(string[] args)
        {
            InitializeComponent();
            
            if (args.Length == 1)
            {
                LoadFile(args[0]);
            }
        }

        private void LoadFile(string path)
        {
            m_streamer = new APNGStreamer(path);
            m_streamer.FrameUpdateEvent += Streamer_FrameUpdateEvent;
            m_streamer.ElapsedTimeUpdateEvent += Streamer_ElapsedTimeUpdateEvent;
            m_streamer.SpeedChangedEvent += Streamer_SpeedChangedEvent;
            m_streamer.TransportStateChangedEvent += Streamer_TransportStateChangedEvent;

            PluginLoader pluginLoader = new PluginLoader(m_streamer, Path.Combine(Directory.GetCurrentDirectory(), "Plugins"));
            AppDomain.CurrentDomain.AssemblyResolve += pluginLoader.OnAssemblyResolve;
            m_plugins = pluginLoader.GetPlugins();

            foreach (IPlugin plugin in m_plugins)
            {
                // TODO: clear plugins menu
                pluginsToolStripMenuItem.DropDownItems.Add(plugin.GetToolStripMenuItem());
            }
        }

        void Streamer_SpeedChangedEvent(object sender, SpeedChangedEventArgs e)
        {
            // update onscreen display of speed
            throw new NotImplementedException();
        }

        public void PlayToFrameCount(int frameCount)
        {
            m_streamer.PlayToFrameCount(frameCount);
        }

        public void PlayToElapsedTime(TimeSpan elapsedTime)
        {
            m_streamer.PlayToElapsedTime(elapsedTime);
        }

        private void Streamer_FrameUpdateEvent(object sender, FrameUpdateEventArgs e)
        {
            UpdateFrameCount(e.FrameCount);
        }

        private void UpdateFrameCount(int frameCount)
        {
            if (InvokeRequired)
            {
                Invoke((Action)delegate() { UpdateFrameCount(frameCount); });
            }
            else
            {
                labelCounter.Text = frameCount.ToString();
                pictureBox.Refresh();
            }
        }

        private void Streamer_TransportStateChangedEvent(object sender, TransportStateChangedEventArgs e)
        {
            UpdateTransportState(e.TransportState);
        }

        private void UpdateTransportState(TransportState transportState)
        {
            if (InvokeRequired)
            {
                Invoke((Action) delegate() { UpdateTransportState(transportState); });
            }
            else
            {
                switch (transportState)
                {
                    case TransportState.Pause:
                        buttonPlayToggle.Text = "Play";
                        break;
                    case TransportState.Play:
                        buttonPlayToggle.Text = "Pause";
                        break;
                }   
            }         
        }

        private void Streamer_ElapsedTimeUpdateEvent(object sender, ElapsedTimeUpdateEventArgs e)
        {
            UpdateElapsedTime(e.ElapsedTime);
        }

        private void UpdateElapsedTime(TimeSpan elapsedTime)
        {
            if (InvokeRequired)
            {
                Invoke((Action)delegate() { UpdateElapsedTime(elapsedTime); });
            }
            else
            {
                labelTime.Text = elapsedTime.ToString(@"hh\:mm\:ss\.ff");
            }
        }

        private void buttonPlayToggle_Click(object sender, EventArgs e)
        {
            switch (m_streamer.TransportState)
            {
                case TransportState.Stop:
                case TransportState.Pause:
                    m_streamer.Play();
                    break;
                case TransportState.Play:
                    m_streamer.Pause();
                    break;
            }
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Image img = m_streamer.Image;
                if (img != null)
                {
                    e.Graphics.DrawImage(img, 0, 0, pictureBox.Width, pictureBox.Height);
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            catch (Exception)
            {
            }
        }

        private void Player_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_streamer != null)
            {
                m_streamer.Pause();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog(this) == DialogResult.OK)
            {
                LoadFile(fileDialog.FileName);
            }
        }
    }
}
