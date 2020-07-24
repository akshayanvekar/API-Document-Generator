using System.IO;
using System;
using System.Drawing;
using API_Document_Generator.Model;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;

namespace API_Document_Generator.Pages.Services
{
    public class ProcessDocument
    {
        public MemoryStream GenerateDocument(Root root)
        {
            Document document = new Document();

            Section newsection = document.AddSection();
            Paragraph Information = newsection.AddParagraph();
            TextRange InformationTR = Information.AppendText("API Information");
            InformationTR.CharacterFormat.FontName = "Calibri";
            InformationTR.CharacterFormat.FontSize = 16;
            InformationTR.CharacterFormat.TextColor = Color.Black;
            InformationTR.CharacterFormat.Bold = true;
            InformationTR.CharacterFormat.AllCaps = true;
            InformationTR.CharacterFormat.UnderlineStyle = UnderlineStyle.Thick;
            Information.Format.LineSpacing = 20;



            Paragraph Title = newsection.AddParagraph();
            TextRange TitleTR = Title.AppendText($"Title: {root.Info.Title}");
            TitleTR.CharacterFormat.FontName = "Calibri";
            TitleTR.CharacterFormat.FontSize = 12;
            TitleTR.CharacterFormat.TextColor = Color.Black;
            Paragraph Version = newsection.AddParagraph();
            TextRange VersionTR = Version.AppendText($"Version: {root.Info.Version}");
            VersionTR.CharacterFormat.FontName = "Calibri";
            VersionTR.CharacterFormat.FontSize = 12;
            VersionTR.CharacterFormat.TextColor = Color.Black;
            Paragraph Host = newsection.AddParagraph();
            TextRange HostTR = Host.AppendText($"Host: {root.Info.Host}");
            HostTR.CharacterFormat.FontName = "Calibri";
            HostTR.CharacterFormat.FontSize = 12;
            HostTR.CharacterFormat.TextColor = Color.Black;
            Paragraph BaseUrl = newsection.AddParagraph();
            TextRange BaseUrlTR = BaseUrl.AppendText($"BaseUrl: {root.Info.BaseUrl}");
            BaseUrlTR.CharacterFormat.FontName = "Calibri";
            BaseUrlTR.CharacterFormat.FontSize = 12;
            BaseUrlTR.CharacterFormat.TextColor = Color.Black;
            Paragraph Description = newsection.AddParagraph();
            TextRange DescTR = Description.AppendText($"Description: {root.Info.Description}");
            DescTR.CharacterFormat.FontName = "Calibri";
            DescTR.CharacterFormat.FontSize = 12;
            DescTR.CharacterFormat.TextColor = Color.Black;


            if (root.PathInfos != null && root.PathInfos.Count > 0)
            {
                Section PathInformationSection = document.AddSection();
                Paragraph EndpointParaGraph = PathInformationSection.AddParagraph();
                TextRange EndPointTR = EndpointParaGraph.AppendText("End Points");
                EndPointTR.CharacterFormat.FontName = "Calibri";
                EndPointTR.CharacterFormat.FontSize = 16;
                EndPointTR.CharacterFormat.TextColor = Color.Black;
                EndPointTR.CharacterFormat.Bold = true;
                EndPointTR.CharacterFormat.AllCaps = true;
                EndPointTR.CharacterFormat.UnderlineStyle = UnderlineStyle.Thick;
                EndpointParaGraph.Format.LineSpacing = 20;

                foreach (var pathInfo in root.PathInfos)
                {
                    //Section PathInformationSection = document.AddSection();                    
                    Paragraph methodParaGraph = PathInformationSection.AddParagraph();
                    TextRange MethodTR = methodParaGraph.AppendText($"Method: {pathInfo.MethodType}");
                    MethodTR.CharacterFormat.FontName = "Calibri";
                    MethodTR.CharacterFormat.FontSize = 12;
                    MethodTR.CharacterFormat.TextColor = Color.Black;

                    Paragraph pathNameParGraph = PathInformationSection.AddParagraph();
                    TextRange pathTR = pathNameParGraph.AppendText($"Resource: {pathInfo.PathName}");
                    pathTR.CharacterFormat.FontName = "Calibri";
                    pathTR.CharacterFormat.FontSize = 12;
                    pathTR.CharacterFormat.TextColor = Color.Black;

                    Paragraph consumesParaGraph = PathInformationSection.AddParagraph();
                    TextRange consumeTR = consumesParaGraph.AppendText($"Accepts Header: {string.Join(",", pathInfo.Consumes)}");
                    consumeTR.CharacterFormat.FontName = "Calibri";
                    consumeTR.CharacterFormat.FontSize = 12;
                    consumeTR.CharacterFormat.TextColor = Color.Black;

                    Paragraph producesParaGraph = PathInformationSection.AddParagraph();
                    TextRange produceTR = producesParaGraph.AppendText($"Accepts Response: {string.Join(",", pathInfo.Produces)}");
                    produceTR.CharacterFormat.FontName = "Calibri";
                    produceTR.CharacterFormat.FontSize = 12;
                    produceTR.CharacterFormat.TextColor = Color.Black;

                    Paragraph summaryParaGraph = PathInformationSection.AddParagraph();
                    TextRange summaryTR = summaryParaGraph.AppendText($"Summary: {pathInfo.Summary}");
                    summaryTR.CharacterFormat.FontName = "Calibri";
                    summaryTR.CharacterFormat.FontSize = 12;
                    summaryTR.CharacterFormat.TextColor = Color.Black;



                    if (pathInfo.parameters != null && pathInfo.parameters.Count > 0)
                    {
                        //Section tableSection = document.AddSection();
                        Table table = PathInformationSection.AddTable(true);
                        String[] Header = { "Parameter", "Type", "Is Required", "Description" };

                        String[][] data = new String[pathInfo.parameters.Count][];
                        for (int index = 0; index < pathInfo.parameters.Count; index++)
                        {
                            data[index] = new String[] {
                                pathInfo.parameters[index].Name,
                                pathInfo.parameters[index].Type,
                                pathInfo.parameters[index].IsRequired,
                                pathInfo.parameters[index].Description
                            };
                        }


                        //Add Cells
                        table.ResetCells(data.Length + 1, Header.Length);

                        //Header Row
                        TableRow FRow = table.Rows[0];
                        FRow.IsHeader = true;
                        //Row Height
                        FRow.Height = 23;
                        //Header Format
                        FRow.RowFormat.BackColor = Color.White;
                        for (int i = 0; i < Header.Length; i++)
                        {
                            //Cell Alignment
                            Paragraph p = FRow.Cells[i].AddParagraph();
                            FRow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                            p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                            //Data Format
                            TextRange TR = p.AppendText(Header[i]);
                            TR.CharacterFormat.FontName = "Calibri";
                            TR.CharacterFormat.FontSize = 12;
                            TR.CharacterFormat.TextColor = Color.Black;
                            TR.CharacterFormat.Bold = true;
                        }

                        //Data Row
                        for (int r = 0; r < data.Length; r++)
                        {
                            TableRow DataRow = table.Rows[r + 1];

                            //Row Height
                            DataRow.Height = 15;

                            //C Represents Column.
                            for (int c = 0; c < data[r].Length; c++)
                            {
                                //Cell Alignment
                                DataRow.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                                //Fill Data in Rows
                                Paragraph p2 = DataRow.Cells[c].AddParagraph();
                                TextRange TR2 = p2.AppendText(data[r][c]);
                                //Format Cells
                                p2.Format.HorizontalAlignment = HorizontalAlignment.Center;
                                TR2.CharacterFormat.FontName = "Calibri";
                                TR2.CharacterFormat.FontSize = 12;
                                TR2.CharacterFormat.TextColor = Color.Black;
                            }
                        }

                    }
                    Paragraph SpaceParaGraph = PathInformationSection.AddParagraph();
                    SpaceParaGraph.Format.LineSpacing = 20;
                }

            }
            // document.SaveToFile($"{root.Info.Title}.docx", FileFormat.Docx);
            using (MemoryStream stream = new MemoryStream())
            {
                document.SaveToStream(stream, FileFormat.Docx);
                return stream;
            }
        }

    }
}