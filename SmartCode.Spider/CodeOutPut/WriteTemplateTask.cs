using System.Collections.Generic;
using System.Threading.Tasks;
using SmartCode.Configuration;
using SmartCode.TemplateEngine;

namespace SmartCode.Spider.CodeOutPut;

public class WriteTemplateTask: IWriteTemplateTask
{
    private CommandLine _commandLine;
    private Project _project;
    private readonly IPluginManager _pluginManager;

    public WriteTemplateTask(CommandLine commandLine, Project project, IPluginManager pluginManager)
    {
        _commandLine = commandLine;
        _project = project;
        _pluginManager = pluginManager;
    }
    public void Initialize(IDictionary<string, object> parameters)
    {
        Initialized = true;
    }

    public bool Initialized { get; private set; }
    public string Name { get; } = "WriteTemplate";
    public async Task Build(BuildContext context)
    {
         await _pluginManager.Resolve<ITemplateEngine>(context.Build.TemplateEngine.Name).Render(context);
         await _pluginManager.Resolve<IOutput>(context.Build.Output.Type).Output(context);
    }
}