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

namespace API_Document_Generator.Pages
{
    public partial class Resource
    {
        //https://petstore.swagger.io/v2/swagger.json
        protected string status { get; set; }
        protected string url { get; set; }
        protected Root root { get; set; }
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
        private void GetDocument(JObject jsonObject)
        {
            root = new ProcessSchema().GetPath(jsonObject);
        }

        protected void GetDocument(Root root)
        {
            Document document = new Document();

            //Header and Footer
            Section section = document.Sections[0];
            HeaderFooter header = section.HeadersFooters.Header;
            Paragraph HParagraph = header.AddParagraph();
            TextRange HText = HParagraph.AppendText(root.Info.Title);

            //Set Header Text Format
            HText.CharacterFormat.FontName = "Algerian";
            HText.CharacterFormat.FontSize = 15;
            HText.CharacterFormat.TextColor = Color.RoyalBlue;

            //Set Header Paragraph Format
            HParagraph.Format.HorizontalAlignment = HorizontalAlignment.Left;
            HParagraph.Format.Borders.Bottom.BorderType = BorderStyle.ThickThinMediumGap;
            HParagraph.Format.Borders.Bottom.Space = 0.05f;
            HParagraph.Format.Borders.Bottom.Color = Color.DarkGray;


            HeaderFooter footer = section.HeadersFooters.Footer;



            //document.SaveToFile("OperateWord.docx", FileFormat.Docx);

        }
    }
}