using System;
using System.Collections.Generic;
using System.Text;

namespace URLinq.CLI.Models
{
    public class ProjectModel
    {
        public string ProjectPath { get; set; }
        public string SolutionPath { get; set; }
        public List<ControllerModel> Controllers { get; set; } = new List<ControllerModel>(); 
 
    }
}
