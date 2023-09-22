using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SmartCode.Configuration;
using SmartCode.Spider.CodeOutPut;

namespace SmartCode.Spider;

public class SpiderProjectBuilder:IProjectBuilder
{
    private readonly Project _project;
    private readonly IPluginManager _pluginManager;
    private readonly ILogger<SpiderProjectBuilder> _logger;


    public SpiderProjectBuilder(
        Project project
        , IPluginManager pluginManager
        , ILogger<SpiderProjectBuilder> logger)
    {
        _project = project;
        _pluginManager = pluginManager;
        _logger = logger;
    }
    
    public Task Build()
    {
        var  task= _pluginManager.Resolve<ICodeFileOutPutTask>();
        return task.Build(null);
    }
}