
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API_Document_Generator.Model;
using Blazor.FileReader;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json.Linq;
using System.Text;
using API_Document_Generator.Pages.Services;

namespace API_Document_Generator.Pages
{
    public partial class FileUpload
    {
        [Inject]
        protected IFileReaderService fileReader { get; set; }
        protected string fileStatus;
        protected ElementReference fileReference;
        protected string status { get; set; }
        protected Root root { get; set; }
        protected async Task HandleSelection()
        {
            var file = (await fileReader.CreateReference(fileReference).EnumerateFilesAsync()).FirstOrDefault();
            if (file != null)
            {
                var fileInfo = await file.ReadFileInfoAsync();
                if (Path.GetExtension(fileInfo.Name) != ".json")
                {
                    fileStatus = "Please select JSON file";
                    return;
                }
                var stream = await file.CreateMemoryStreamAsync();
                string jsonString = Encoding.UTF8.GetString(stream.ToArray());
                JObject jsonObject = JObject.Parse(jsonString);
                GetDocument(jsonObject);
            }
        }
        private void GetDocument(JObject jsonObject)
        {
            root = new ProcessSchema().GetPath(jsonObject);
        }
    }
}
