using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FindJobs.App.helper
{
    public class GetUrl
    {
        public static string Get(string key)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var valueKey = specificAssembely(assembly, key);
                if (valueKey != "")
                    return valueKey;

            }
            return "";
        }
        private static string specificAssembely(Assembly assembly, string key)
        {
#if DEBUG
            var resName = assembly.GetManifestResourceNames()
                               ?.FirstOrDefault(r => r.EndsWith("appsettings.Debug.json", StringComparison.OrdinalIgnoreCase)) ?? "";
            var value = ReturnValue(key, resName, assembly);
            if (value != null && !string.IsNullOrEmpty(value))
            {
                return value?? "";
            }
            else
            {
                resName = assembly.GetManifestResourceNames()
                              ?.FirstOrDefault(r => r.EndsWith("appsettings.json", StringComparison.OrdinalIgnoreCase)) ?? "";
                return ReturnValue(key, resName, assembly)?? "";
            }
#else
            var resName = assembly.GetManifestResourceNames()
                                         ?.FirstOrDefault(r => r.EndsWith("appsettings.Release.json", StringComparison.OrdinalIgnoreCase)) ?? "";
            var value = ReturnValue(key, resName, assembly);
            if (!string.IsNullOrEmpty(value))
            {
                return value ?? "";
            }
            else
            {
                resName = assembly.GetManifestResourceNames()
                              ?.FirstOrDefault(r => r.EndsWith("appsettings.json", StringComparison.OrdinalIgnoreCase)) ?? "";
                return ReturnValue(key, resName, assembly);
            }
#endif
        }
        private static string? ReturnValue(string key, string resName, Assembly assembly)
        {
            if (!string.IsNullOrEmpty(resName))
            {
                var file = assembly.GetManifestResourceStream(resName);
                var sr = new StreamReader(file);
                var json = sr.ReadToEnd();
                var j = JsonConvert.DeserializeObject(json) as JObject;
                if (key.Contains(":"))
                {
                    var keys = key.Split(':');
                    var parentKey = keys[0];
                    if (!j.ContainsKey(parentKey)) return "";
                    string childKey = keys[1];
                    if (parentKey != null && childKey != null)
                        return j[parentKey]?[childKey]?.Value<string>();
                }
                if (!j.ContainsKey(key)) return "";
                return j[key]?.Value<string>();
            }
            return "";
        }
    }
}

