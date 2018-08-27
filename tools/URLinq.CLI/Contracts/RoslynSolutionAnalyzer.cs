using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Buildalyzer;
using Buildalyzer.Workspaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using URLinq.CLI.Models;

namespace URLinq.CLI.Contracts
{
    internal class RoslynSolutionAnalyzer : ISolutionAnalyzer
    {
        private readonly ILogger<RoslynSolutionAnalyzer> logger;

        public RoslynSolutionAnalyzer(ILogger<RoslynSolutionAnalyzer> logger)
        {
            this.logger = logger;
        }

        public async Task<IEnumerable<ProjectModel>> Analyze(string solutionPath)
        {
            var projects = new List<ProjectModel>();
            var solution = new FileInfo(solutionPath);
            logger.LogDebug($"Inspecting Solution:{solutionPath}");
            var directory = solution.Directory;
            //find first project
            var firstProject = directory.GetFiles("*.csproj", SearchOption.AllDirectories).FirstOrDefault();
            AnalyzerManager manager = new AnalyzerManager(solutionPath, new AnalyzerManagerOptions
            {
            });

        
            var t = manager.Projects.Values.Select(project => GetProjects(project));
            //var tasks = manager.Projects.Values.Select(project =>
            //{
            //    var workspace = project.GetWorkspace();
            //    var csharpProject = workspace.CurrentSolution.Projects.First();
            //    return GetControllers(csharpProject);
            //});
            var results = await Task.WhenAll(t);

            //foreach (var project in manager.Projects.Values)
            //{
            //    var workspace = project.GetWorkspace();
            //    var csharpProject = workspace.CurrentSolution.Projects.First();
            //    var controllers = GetControllers(csharpProject);
            //    if (controllers.Any())
            //    {
            //        var data = JsonConvert.SerializeObject(controllers, Formatting.Indented);
            //        File.WriteAllText("Controllers.json", data);
            //        projects.Add(new ProjectModel
            //        {
            //            Controllers = controllers.ToList(),
            //            ProjectPath = project.ProjectFile.Path,
            //            SolutionPath = solution.FullName
            //        });
            //        break;
            //    }
            //}
            results.All(x =>
            {
                x.SolutionPath = solution.FullName;
                return true;
            });
            return results.Where(x => x.Controllers.Any());
        }

        private async Task<ProjectModel> GetProjects(ProjectAnalyzer project)
        {
            var p = new ProjectModel();
            var workspace = project.GetWorkspace();
            var csharpProject = workspace.CurrentSolution.Projects.First();
            var controllers = await GetControllers(csharpProject);
            logger.LogInformation($"Inspecting Project:{project.ProjectFile.Path} for Controllers");
            p.Controllers = controllers.ToList();
            p.ProjectPath = project.ProjectFile.Path;
            p.SolutionPath = workspace.CurrentSolution.FilePath;
            return p;
        }

        private async Task<IEnumerable<ControllerModel>> GetControllers(Project project)
        {
            Compilation compilation = await project.GetCompilationAsync();

            var controllers = new List<ControllerModel>();
            foreach (var code in project.Documents.Where(x => x.Name.EndsWith("Controller.cs", StringComparison.OrdinalIgnoreCase)))
            {
                var controllerModel = new ControllerModel();
                logger.LogInformation($"{project.Name} has controller:{code.Name}");

                SyntaxTree syntaxTree = null;
                code.TryGetSyntaxTree(out syntaxTree);
                var semanticModel = compilation.GetSemanticModel(syntaxTree);
                var root = (CompilationUnitSyntax)syntaxTree.GetRoot();
                var ns = GetNamespace(root);

                ClassDeclarationSyntax classDeclaration = root.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
                controllerModel.ControllerName = classDeclaration.Identifier.ValueText;
                controllerModel.Namespace = ns;
                controllerModel.Actions = new List<ControllerActionModel>(GetMethods(classDeclaration, semanticModel, controllerModel, compilation));
                if (controllerModel.Actions.Any())
                {
                    controllers.Add(controllerModel);
                }
            }
            return controllers;
        }

        /// <summary>
        /// Gets the containing class namespace.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <returns></returns>
        public string GetNamespace(CompilationUnitSyntax root)
        {
            var ns = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>().SingleOrDefault();
            if (ns != null)
            {
                return ns.Name.ToString();
            }
            return null;
        }

        /// <summary>
        /// Gets the containing class namespace.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <returns></returns>
        public IEnumerable<ControllerActionModel> GetMethods(ClassDeclarationSyntax classDeclarationSyntax, SemanticModel semanticModel, ControllerModel controllerModel, Compilation compilation)
        {
            var actions = new List<ControllerActionModel>();
            foreach (var method in classDeclarationSyntax.DescendantNodes().OfType<MethodDeclarationSyntax>())
            {
                var trivias = method.GetLeadingTrivia();
                var xmlCommentTrivia = trivias.FirstOrDefault(t => t.Kind() == SyntaxKind.SingleLineDocumentationCommentTrivia);
                var xml = xmlCommentTrivia.GetStructure();
                var nodes = xml.DescendantNodes().Where(x => x.IsKind(SyntaxKind.XmlElement)).ToList();
                foreach (var n in nodes)
                {
                    var kids = n.ChildNodes().ToList();
                }
                Console.WriteLine(xml);
                logger.LogDebug($"{classDeclarationSyntax.Identifier.ToFullString().Trim()}.{method.Identifier.ToFullString().Trim()}");

                var action = new ControllerActionModel
                {
                    ActionName = method.Identifier.ValueText,
                    Summary = ""
                };
                GetMethodParameters(method, semanticModel, action, compilation);
                actions.Add(action);
            }
            return actions;
        }

        public void GetMethodParameters(MethodDeclarationSyntax methodDeclaration, SemanticModel semanticModel, ControllerActionModel controllerActionModel, Compilation compilation)
        {
            var list = methodDeclaration.AttributeLists.SelectMany(a => a.Attributes).ToList();
            var parameters = new List<ControllerActionParameter>();
            var parameterListSyntax = methodDeclaration.ChildNodes().OfType<ParameterListSyntax>().FirstOrDefault();
            foreach (var parameterSyntax in parameterListSyntax.Parameters)
            {
                var symbol = semanticModel.GetDeclaredSymbol(parameterSyntax);
                if (symbol == null)
                {
                    continue;
                }
                var type = symbol.Type as INamedTypeSymbol;
                if (type == null)
                {
                    continue;
                }

                var controllerParameter = new ControllerActionParameter
                {
                    ParameterName = parameterSyntax.Identifier.ValueText,
                    Required = !symbol.HasExplicitDefaultValue,
                    ParameterTypeName = type.ToString(),
                    ParameterTypeNamespace = type.ContainingNamespace.ToString()
                };
                if (type.ContainingNamespace.IsGlobalNamespace)
                {
                    
                    //var location = type.Locations.FirstOrDefault();
                    //var tree = location?.SourceTree;

                    //var root = tree.GetCompilationUnitRoot();
                    //var model = compilation.GetSemanticModel(tree);
                    //var ns = GetNamespace(root);

                    //controllerParameter.ParameterTypeNamespace = GetNamespace(root);
                }
                parameters.Add(controllerParameter);
                //var blockMethodParameter = BlockFactory.ParameterProvider.BlockMethodParameterFromMethodDeclaration(parameterSyntax, semanticModel, blockMethod);
                //if (blockMethodParameter != null)
                //{
                //    blockMethod.Parameters.Add(blockMethodParameter);
                //}
            }
            controllerActionModel.Parameters = parameters;
        }
    }
}