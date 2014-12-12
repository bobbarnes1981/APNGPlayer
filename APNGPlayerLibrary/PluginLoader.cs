using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using APNGLibrary;

namespace APNGPlayerLibrary
{
    public class PluginLoader
    {
        /// <summary>
        /// Streamer
        /// </summary>
        private IStreamer m_streamer;

        /// <summary>
        /// Plugin path
        /// </summary>
        private string m_path;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="streamer"></param>
        /// <param name="path"></param>
        public PluginLoader(IStreamer streamer, string path)
        {
            m_streamer = streamer;
            m_path = path;
        }
        
        /// <summary>
        /// Resolve assemblies in plugin folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            string name = string.Format("{0}.dll", args.Name.Substring(0, args.Name.IndexOf(',')));
            return Assembly.LoadFrom(Path.Combine(m_path, name));
        }

        /// <summary>
        /// Load all plugins in plugin folder
        /// </summary>
        /// <returns></returns>
        public IPlugin[] GetPlugins()
        {
            // get directory
            DirectoryInfo directoryInfo = new DirectoryInfo(m_path);
            // build empty list for plugins
            List<IPlugin> plugins = new List<IPlugin>();
            // check all files in plgin folder
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                // if file is DLL
                if (file.Extension == ".dll")
                {
                    // load assembly
                    Assembly pluginAssembly = Assembly.LoadFile(file.FullName);
                    // get filename/pluginname
                    string name = string.Format("{0}.{0}", Path.GetFileNameWithoutExtension(file.FullName));
                    // try to get type
                    Type pluginType = pluginAssembly.GetType(name);
                    // if exists
                    if (pluginType != null)
                    {
                        // if implements IPlugin
                        if (typeof (IPlugin).IsAssignableFrom(pluginType))
                        {
                            // get default constructor
                            ConstructorInfo pluginConstructor = pluginType.GetConstructor(Type.EmptyTypes);
                            // construct
                            IPlugin plugin = (IPlugin) pluginConstructor.Invoke(new object[0]);
                            // set the streamer
                            plugin.SetStreamer(m_streamer);
                            // add to list
                            plugins.Add(plugin);
                        }
                    }
                }
            }
            // return the plugins
            return plugins.ToArray();
        }
    }
}
