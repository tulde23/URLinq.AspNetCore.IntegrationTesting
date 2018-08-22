using System;
using System.Collections.Generic;
using System.Text;

namespace URLinq.CLI.Models
{
    public class ControllerModel
    {
        public string Namespace { get; set; }
        public string ControllerName { get; set; }
        public List<ControllerActionModel> Actions { get; set; } = new List<ControllerActionModel>();
    }
}
