using System.Collections.Generic;

namespace API_Document_Generator.Model
{
    public class Root
    {
        public Info Info { get; set; }
        public List<PathInfo> PathInfos { get; set; }
        public List<string> Schemas { get; set; }
    }
    public class Info
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }

    }
    public class PathInfo
    {
        private string _methodtype;
        public string PathName { get; set; }
        public string MethodType
        {
            get
            {
                return _methodtype.ToUpper();
            }
            set
            {
                _methodtype = value;
            }
        }
        public string Summary { get; set; }
        public string Description { get; set; }
        public List<Parameter> parameters { get; set; }
        public List<ResponseInfo> responses { get; set; }

        public List<string> Consumes { get; set; }
        public List<string> Produces { get; set; }
    }
    public class ResponseInfo
    {
        public string StatusCode { get; set; }
        public string Description { get; set; }
    }
    public class Parameter
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string IsRequired { get; set; }
    }
}