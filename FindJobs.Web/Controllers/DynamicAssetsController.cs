using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;
using System.Web;

namespace FindJobs.Web.Controllers
{
    public class DynamicAssetsController : Controller
    {
        private string GetPath(string ext, string folderName, string fileName)
        {
            fileName = fileName.Replace("/", string.Empty).Replace("\\", string.Empty).Replace(".", string.Empty) +
                       "." + ext;
            folderName = folderName.Replace("/", string.Empty).Replace("\\", string.Empty).Replace(".", string.Empty);
            string folderPath;
             if (folderName == "internal")
                folderPath = "wwwroot/" + ext + "/internal/";
            else
                folderPath = "Areas/" + folderName + "/Resources/";

            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), folderPath) + fileName;
        }

        [Route("/dcss/{folderName}/{fileName}/{parameter?}")]
        public IActionResult CSS(string folderName, string fileName, string parameter = null)
        {
            var fullPath = GetPath("css", folderName, fileName);
            string file = string.Empty;
            file = System.IO.File.ReadAllText(fullPath);

            if (parameter != null)
            {
                var parameters = parameter.Split('~');
                for (var i = 0; i < parameters.Length; i++)
                {
                    file = file.Replace("$" + i + "$", HttpUtility.UrlDecode(parameters[i]));
                }
            }

            return Content(file, "text/css");
        }

        [Route("/djs/{folderName}/{fileName}/{parameter?}")]
        public IActionResult JS(string folderName, string fileName, string parameter)
        {
            var fullPath = GetPath("js", folderName, fileName);
            string file = string.Empty;
            file = System.IO.File.ReadAllText(fullPath);

            if (parameter != null)
            {
                var parameters = parameter.Split('~');
                for (var i = 0; i < parameters.Length; i++)
                {
                    file = file.Replace("$" + i + "$", HttpUtility.UrlDecode(parameters[i]));
                }
            }

            return Content(file, "text/javascript");
        }

        [Route("/d{ext}/{folderName}/{fileName}")]
        public IActionResult Image(string folderName, string fileName,string ext)
        {
            ext = ext.Replace("/", string.Empty).Replace("\\", string.Empty).Replace(".", string.Empty);
            fileName = fileName.Replace("/", string.Empty).Replace("\\", string.Empty).Replace(".", string.Empty);
            folderName = folderName.Replace("/", string.Empty).Replace("\\", string.Empty).Replace(".", string.Empty);
            var ms = FindJobs.Resources.WebResources.GetImage(folderName+"."+fileName+"."+ext);
            return new FileStreamResult(ms, "image/"+ext.Replace("jpg","jpeg").Replace("svg","svg+xml"));
        }
    }
}