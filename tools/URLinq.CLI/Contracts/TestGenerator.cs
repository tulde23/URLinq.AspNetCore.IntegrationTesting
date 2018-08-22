using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLinq.CLI.Models;

namespace URLinq.CLI.Contracts
{
    internal class TestGenerator : IIntegrationTestGenerator
    {
        private string MethodTemplate = @"
       public Task <#MethodName#>Test(){
            var route = URLinq.RouteHelper.BuildRequestMessage<<#ControllerName#>,object>(controller=>controller.<#ActionName#>(<#ActionParameters#>));
	   }
";

        private ControllerModel _mode;
        private StringBuilder builder = new StringBuilder(URLinq.CLI.Assets.Code.IntegrationTest);

        public Task Generate(ControllerModel controllerModel)
        {
            _mode = controllerModel;
            builder.Replace("<#ControllerName#>", controllerModel.ControllerName);
            var methods = new StringBuilder();
            foreach (var item in controllerModel.Actions)
            {
                var template = new StringBuilder(MethodTemplate.Replace("<#MethodName#>", item.ActionName));
                template.Replace("<#ControllerName#>", controllerModel.ControllerName)
                            .Replace("<#ActionName#>", item.ActionName)
                            .Replace("<#ActionParameters#>", string.Join(",", item.Parameters.Select(x => x.ParameterName)));
                methods.AppendLine(template.ToString());
            }
            builder.Replace("<#TestMethods#>", methods.ToString());
            return Task.CompletedTask;
        }

        public Task WriteTo(string path)
        {
            File.WriteAllText(path, builder.ToString());
            return Task.CompletedTask;
        }
    }
}