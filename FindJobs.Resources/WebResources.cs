using System;
using System.IO;
using System.Reflection;


namespace FindJobs.Resources
{

    public class WebResources
    {
        public static Stream GetImage(string name)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var assemblyName = System.IO.Path.GetFileNameWithoutExtension(assembly.Location);
            var defaultNamespace = assemblyName;
            string resourcePath = defaultNamespace + ".Images." + name;
            Stream stream = assembly.GetManifestResourceStream(resourcePath);
            return stream;
        }
    }
}
