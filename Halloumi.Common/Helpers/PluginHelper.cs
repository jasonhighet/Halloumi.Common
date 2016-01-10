using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;


namespace Halloumi.Common.Helpers
{
    public static class PluginHelper<T>
    {
        #region Public Methods

        /// <summary>
        /// Finds and loads the plugins of the particular type from the execution folder 
        /// Expects to find plugins in the executable folder, named in the format
        /// [T].[PluginName].dll
        /// </summary>
        /// <returns>A list of loaded plugins</returns>
        public static List<T> LoadPlugins()
        {
            List<T> plugins = new List<T>();
            DirectoryInfo folder = new DirectoryInfo(ApplicationHelper.GetExecutableFolder());

            string filemask = typeof(T).FullName.Replace(typeof(T).Name, "") + "*.dll";

            foreach (FileInfo file in folder.GetFiles(filemask))
            {
                Assembly assembly = Assembly.LoadFrom(file.FullName);
                foreach (Type type in assembly.GetTypes())
                {
                    if (!type.IsAbstract)
                    {
                        if (type.IsSubclassOf(typeof(T)) || typeof(T).IsAssignableFrom(type))
                        {
                            try
                            {
                                T plugin = (T)type.InvokeMember(null, BindingFlags.CreateInstance, null, null, null);
                                plugins.Add(plugin);
                            }
                            catch
                            { }
                        }
                    }
                }
            }

            return plugins;
        }

        #endregion
    }
}
