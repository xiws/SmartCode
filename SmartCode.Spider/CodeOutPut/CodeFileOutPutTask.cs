using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SmartCode.Configuration;

namespace SmartCode.Spider.CodeOutPut;

public class CodeFileOutPutTask:ICodeFileOutPutTask
{
    private CommandLine _commandLine;
    private Project _project;

    public CodeFileOutPutTask(CommandLine commandLine, Project project)
    {
        _commandLine = commandLine;
        _project = project;
    }

    public void Initialize(IDictionary<string, object> parameters)
    {
        Initialized = true;
        Name = "CodeFileOutPut";
    }

    public bool Initialized { get; private set; }
    public string Name { get; private set; }
    public Task Build(BuildContext context)
    {
        var projectInfo = _project.CodeFileInfo.FirstOrDefault(val => val.ProjectName == _commandLine.ModelType);
        var path = projectInfo.ProjectPath;
        List<KeyValuePair<string, string>> fileList = new List<KeyValuePair<string, string>>();
        foreach (var item in projectInfo.Path)
        {
            fileList.Add(new KeyValuePair<string, string>($"{string.Format(item.Key,_commandLine.ModelName)}.cs",$"{path}/{item.Value}"));
        }

        foreach (var item in fileList)
        {
            var filename = $"{item.Value}/{item.Key}";
            if(File.Exists(filename)) continue;
            if (!Directory.Exists(item.Value)) Directory.CreateDirectory(item.Value);
            var sw = File.CreateText(filename);
            if (item.Key == _commandLine.ModelName + ".cs")
            {
                sw.Flush();
                sw.Close();
                sw.Dispose();
                continue;
            }

            if (item.Key == _commandLine.ModelName + "Repository.cs")
            {
                var  rep = TemplateModel();
                var repositoryName = $"{_commandLine.ModelName}Repository";
                string result = rep.Replace("{0}", _commandLine.ModelName)
                    .Replace("{1}",repositoryName)
                    .Replace("{2}", $"I{repositoryName}");
                sw.Write(result);
                sw.Flush();
                sw.Close();
                sw.Dispose();
                continue;
            }
            var  interfaceTxt = Interface();
            var repName = $"{_commandLine.ModelName}Repository";
            string txt = interfaceTxt.Replace("{0}", _commandLine.ModelName)
                .Replace("{1}", $"I{repName}");
            
            sw.Write(txt);
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
        
        return Task.CompletedTask;
    }

    public string TemplateModel()
    {
        return @"
using Erp.Amazon.Models.StatisticalAnalysis.ProfitRate;
using Erp.Amazon.Repositories.Interface.StatisticalAnalysis.ProfitRate;
using Microsoft.Extensions.Logging;
using Zhcxkj.DbContext.Interface;
using Zhcxkj.DbRepository.Implement;

namespace Erp.Amazon.Repositories.Implement.StatisticalAnalysis.ProfitRate;

public class  {1} : Repository<{0}, long>, {2}
{
    public {1}(IDbContext dbContext, ILogger<{1}> logger) : base(dbContext, ""AutoChooseDb"")
    {
        // 可以在此处进行其他操作
    }
}
";
    }

    private string Interface()
    {
        return @"using Erp.Amazon.Models.StatisticalAnalysis.ProfitRate;
using Zhcxkj.DbRepository.Interface;

namespace Erp.Amazon.Repositories.Interface.StatisticalAnalysis.ProfitRate;

public interface  {1} : IRepository<{0}, long>
{
    // 可以在此处添加自定义的仓储方法
}";
    }
}