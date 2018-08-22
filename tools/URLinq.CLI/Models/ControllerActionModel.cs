using System.Collections.Generic;

namespace URLinq.CLI.Models
{
    public class ControllerActionModel
    {
        public string ActionName { get; set; }
        public string Summary { get; set; }
        public List<ControllerActionParameter> Parameters { get; set; } = new List<ControllerActionParameter>();
    }
}