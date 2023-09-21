using System;
using System.Diagnostics;
using System.Threading.Tasks;

using System.Threading;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using SmartCode.Configuration;

namespace SmartCode.xiw
{
    class Program
    {
        static async Task Main(string[] args)
        {
            List<CodeFileInfo> CodeFileInfo = new List<CodeFileInfo>()
            {
                new CodeFileInfo()
                {
                    ProjectName="amazon",
                    ProjectPath = "D:\\Project\\erpamazonworker",
                    Path = new Dictionary<string, string>()
                    {
                        {"{0}","Erp.Amazon.Models\\StatisticalAnalysis\\ProfitRate"},
                        {"{0}Repository","Erp.Amazon.Repositories\\Implement\\StatisticalAnalysis\\ProfitRate"},
                        {"I{0}Repository","Erp.Amazon.Repositories\\Interface\\StatisticalAnalysis\\ProfitRate"},
                    }
                }
            };
 var tt=           Newtonsoft.Json.JsonConvert.SerializeObject(CodeFileInfo);
            ThreadPool.SetMinThreads(1000, 1000);
            ThreadPool.SetMaxThreads(1000, 1000);
            await CommandLineApplication.ExecuteAsync<SmartCodeCommand>(args);
        }
    }
}