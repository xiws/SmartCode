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
        Name = "代码文件输出";
    }

    public bool Initialized { get; private set; }
    public string Name { get; private set; }
    public Task Build(BuildContext context)
    {
        var projectInfo = _project.CodeFileInfo.FirstOrDefault(val => val.ProjectName == _commandLine.ProjectName);
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
            File.CreateText(filename);
        }
        
        return Task.CompletedTask;
    }
}