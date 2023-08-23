using Microsoft.Extensions.Logging;
using SmartCode.Configuration;

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
        throw new NotImplementedException();
    }
}