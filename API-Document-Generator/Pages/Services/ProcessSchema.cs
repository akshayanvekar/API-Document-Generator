using System;
using System.Collections.Generic;
using System.Linq;
using API_Document_Generator.Model;
using Newtonsoft.Json.Linq;

namespace API_Document_Generator.Pages.Services
{
    public class ProcessSchema
    {
        public Root GetPath(JObject jsonObject)
        {

            Root root = new Root();
            Info info = new Info
            {
                Title = jsonObject["info"]["title"].ToString(),
                Description = jsonObject["info"]["description"].ToString(),
                Version = jsonObject["info"]["version"].ToString(),
                Host = jsonObject["host"].ToString(),
                BaseUrl = jsonObject["basePath"].ToString(),
            };
            root.Info = info;

            if (jsonObject.ContainsKey("paths"))
            {
                List<PathInfo> pathInfos = new List<PathInfo>();
                JObject paths = JObject.Parse(jsonObject["paths"].ToString());
                foreach (JProperty property in paths.Properties())
                {
                    PathInfo pathInfo = new PathInfo
                    {
                        PathName = property.Name
                    };

                    var propertypaths = JObject.Parse(property.First.ToString());

                    foreach (JProperty propertymethod in propertypaths.Properties())
                    {
                        pathInfo.MethodType = propertymethod.Name;

                        pathInfo.Summary = propertypaths[propertymethod.Name]["summary"].ToString();
                        pathInfo.Description = propertypaths[propertymethod.Name]["description"].ToString();
                        List<ResponseInfo> responseInfos = new List<ResponseInfo>();
                        var responseprop = JObject.Parse(propertymethod.Last.ToString());
                        foreach (JProperty propertyresponse in responseprop.Properties().Where(x => x.Name == "responses"))
                        {
                            var jObjectResponse = JObject.Parse(propertyresponse.Last.ToString());
                            foreach (JProperty itemresponse in jObjectResponse.Properties())
                            {
                                var JObjectdec = JObject.Parse(itemresponse.First.ToString());
                                ResponseInfo responseInfo = new ResponseInfo
                                {
                                    StatusCode = itemresponse.Name,
                                    Description = JObjectdec.GetValue("description").ToString()
                                };
                                responseInfos.Add(responseInfo);
                            }
                        }
                        pathInfo.responses = responseInfos;

                        List<Parameter> parameters = new List<Parameter>();
                        var parameterprop = JObject.Parse(propertymethod.Last.ToString());
                        foreach (JProperty propertyParameter in responseprop.Properties().Where(x => x.Name == "parameters"))
                        {
                            foreach (var prop in parameterprop["parameters"])
                            {
                                Parameter parameter = new Parameter
                                {
                                    Name = Convert.ToString(prop.SelectToken("name")),
                                    Type = Convert.ToString(prop.SelectToken("type")),
                                    IsRequired = Convert.ToString(prop.SelectToken("required")),
                                    Description = Convert.ToString(prop.SelectToken("description")),
                                };
                                parameters.Add(parameter);
                            }
                        }
                        pathInfo.parameters = parameters;

                        //Consumes
                        List<string> Consumes = new List<string>();
                        var parameterconsume = JObject.Parse(propertymethod.Last.ToString());
                        foreach (JProperty propertyParameter in responseprop.Properties().Where(x => x.Name == "consumes"))
                        {
                            foreach (var consume in parameterconsume["consumes"])
                            {
                                Consumes.Add(consume.ToString());
                            }
                        }
                        pathInfo.Consumes = Consumes;


                        //Produces 
                        List<string> Produces = new List<string>();
                        var parameterProduce = JObject.Parse(propertymethod.Last.ToString());
                        foreach (JProperty propertyParameter in responseprop.Properties().Where(x => x.Name == "produces"))
                        {
                            foreach (var produce in parameterProduce["produces"])
                            {
                                Produces.Add(produce.ToString());
                            }
                        }
                        pathInfo.Produces = Produces;
                    }
                    //Response                
                    pathInfos.Add(pathInfo);
                }
                root.PathInfos = pathInfos;
            }

            List<string> Schemas = new List<string>();
            if (jsonObject.ContainsKey("schemes"))
            {
                foreach (var scheme in jsonObject["schemes"])
                {
                    Schemas.Add(scheme.ToString());
                }
            }
            root.Schemas = Schemas;
            return root;
            // status = JsonConvert.SerializeObject(root, Formatting.Indented);
        }
    }

}