using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using DotNek.Common.Helpers;
using DotNek.Common.Models;
using FindJobs.Domain.Dtos;
using Microsoft.Extensions.Configuration;

namespace FindJobs.Web.Global
{
    public class GlobalResources : IGlobalResources
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IConfiguration configuration;

        public GlobalResources(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.configuration = configuration;
        }
        private string GetMinifiedFileContent(string filePath)
        {
            using (System.IO.StreamReader Reader =
                      new System.IO.StreamReader(filePath))
            {
                return new System.Text.StringBuilder(Reader.ReadToEnd()).ToString().Replace("\r", " ")
                    .Replace("\n", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace(": ", ":")
                    .Replace("{ ", "{").Replace(" }", "}") + " ";

            }
        }
        public Dictionary<string, string> GetInlineStyles()
        {
            var inlineStyles = new Dictionary<string, string>();
            inlineStyles.Add("Layout", "");

            string[] files = System.IO.Directory.GetFiles(System.IO.Path.Combine(webHostEnvironment.WebRootPath, "css\\layout-first"));
            foreach (var file in files)
            {
                inlineStyles["Layout"] += GetMinifiedFileContent(file);
            }
            return inlineStyles;
        }

        
    }
}