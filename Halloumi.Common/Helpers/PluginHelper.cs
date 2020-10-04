using System.Collections.Generic;
using System.IO;
using System.Reflection;


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
            var plugins = new List<T>();
            var folder = new DirectoryInfo(ApplicationHelper.GetExecutableFolder());

            var filemask = typeof(T).FullName.Replace(typeof(T).Name, "") + "*.dll";

            foreach (var file in folder.GetFiles(filemask))
            {
                var assembly = Assembly.LoadFrom(file.FullName);
                foreach (var type in assembly.GetTypes())
                {
                    if (!type.IsAbstract)
                    {
                        if (type.IsSubclassOf(typeof(T)) || typeof(T).IsAssignableFrom(type))
                        {
                            try
                            {
                                var plugin = (T)type.InvokeMember(null, BindingFlags.CreateInstance, null, null, null);
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
