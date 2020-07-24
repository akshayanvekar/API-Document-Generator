using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using API_Document_Generator.Model;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System.Drawing;
using API_Document_Generator.Pages.Services;
using System.IO;
using Microsoft.JSInterop;

namespace API_Document_Generator.Pages
{
    public partial class Resource
    {
        //https://petstore.swagger.io/v2/swagger.json
        protected string status { get; set; }
        protected string url { get; set; }
        protected Root root { get; set; }
        [Inject]
        protected IJSRuntime jSRuntime { get; set; }
        protected async Task GetJson()
        {
            try
            {
                if (String.IsNullOrEmpty(url))
                {
                    status = "Url cannot be empty";
                    return;
                }
                using HttpClient client = new HttpClient();
                var response = await client.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(jsonString);
                    GetDocument(jsonObject);
                }
                else
                {
                    status = "Could not access entered URL. Please try again with valid one.";
                }
            }
            catch (Exception ex)
            {
                string exception = $"{ex.Message} {ex.StackTrace} {ex.InnerException}";
                status = "Could not access entered URL. Please try again with valid one.";
            }
        }
        protected async Task DownloadDocument()
        {
            status = $"Calling Download";
            if (root != null)
            {
                ProcessDocument processDocument = new ProcessDocument();
                MemoryStream stream = processDocument.GenerateDocument(root);
                status = $"Loaded Stream {stream.Length}";
                await FileUtil.SaveAs(jSRuntime, "Document.Docx", stream.ToArray());
            }
            else
            {
                status = $"Root is null ";
            }
        }
        private void GetDocument(JObject jsonObject)
        {
            root = new ProcessSchema().GetPath(jsonObject);
        }

    }
}