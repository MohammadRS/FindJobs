using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Globalization;
using System.Resources;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections;

namespace FindJobs.Resources
{
    [ContentProperty(nameof(Source))]
    public class AppResources : IMarkupExtension
    {
        public string Source { get; set; }

        public static ImageSource GetImage(string name)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var assemblyName = System.IO.Path.GetFileNameWithoutExtension(assembly.Location);
            var defaultNamespace = assemblyName;
            string resourcePath = defaultNamespace + ".Images." + name;
            return ImageSource.FromResource(resourcePath);
        }

        public static string GetImageSvg(string name)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var assemblyName = System.IO.Path.GetFileNameWithoutExtension(assembly.Location);
            var defaultNamespace = assemblyName;
            return "resource://" + defaultNamespace + ".Images." + name + "?assembly=" + defaultNamespace;
        }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }
            return Source.ToLower().EndsWith(".svg") ? GetImageSvg(Source) : GetImage(Source);
        }
        public ViewDataDictionary GetViewData(string culture, string resourceName)
        {
            var cultureFileName = "";
            if (culture.Length > 2)
                cultureFileName = culture.Substring(0, 2);

            var assembly = GetType().Namespace;
            var resourceAssembly = System.Reflection.Assembly.Load(new System.Reflection.AssemblyName(assembly));
            ResourceManager MyResourceClass =
            new ResourceManager(assembly + ".Resx." + cultureFileName + "." + resourceName, resourceAssembly);
            ResourceSet resourceSet =
                MyResourceClass.GetResourceSet(new CultureInfo(culture), true, true);
            var viewDataDictionary = new ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
             new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary());
            foreach (DictionaryEntry entry in resourceSet)
            {
                string resourceKey = entry.Key.ToString();
                object resource = entry.Value;
                viewDataDictionary.Add(resourceKey, resource);
            }
            return viewDataDictionary;
        }
        
    }
}

