using System;
using System.Collections.Generic;
using System.Text;

namespace URLinq.CLI.Models
{
    public class ControllerActionParameter
    {
        public string ParameterName { get; set; }
        public string ParameterTypeName { get; set; }
        public string ParameterTypeNamespace { get; set; }
        public string ParameterComment { get; set; }
        public bool Required { get; set; }
    }
}
