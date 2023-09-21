﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using SmartCode.App;
using SmartCode.Utilities;

namespace SmartCode.xiw
{
    [Subcommand(typeof(MarketCommand))]
    [VersionOptionFromMember("-v|--version", MemberName = nameof(GetVersion))]
    public class SmartCodeCommand
    {
        /// <summary>
        /// 默认配置文件路径
        /// </summary>
        static string DEFAULT_CONFIG_PATH = AppPath.Relative("SmartCode.yml");

        [Argument(0, Description = "Config Path")]
        [FileExists]
        public String ConfigPath { get; set; }
        
        [Option("-ci|--culture-info", Description = "CultureInfo")]
        public String CultureInfo { get; set; }
        
        [Option("-mn |--model-name", Description = "生成的文件名称")]
        public string ModelName { get; set; }
        
        [Option("-ft|--file-type",Description = "文件安装哪个类型进行生成")]
        public string ModelType { get; set; }

        private async Task OnExecute()
        {
            if (!String.IsNullOrEmpty(CultureInfo))
            {
                System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo(CultureInfo);
                System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            }

            if (String.IsNullOrEmpty(ConfigPath))
            {
                var useDefaultConfig =
                    Prompt.GetYesNo(
                        $"If you do not enter ConfigPath, you will use the default configuration:{DEFAULT_CONFIG_PATH}",
                        true);
                ConfigPath = useDefaultConfig
                    ? DEFAULT_CONFIG_PATH
                    : Prompt.GetString("Please enter the path to build configuration file:");
            }

            var dic = new Dictionary<string, string>()
            {
                {nameof(ModelName),ModelName},
                {nameof(ModelType),ModelType}
            };
            SmartCodeApp app = new DefaultSmartCodeAppBuilder().Build(ConfigPath,dic);
            await app.Run();
        }

        private static string GetVersion()
            => typeof(SmartCodeCommand).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;

        [Command(name: "pull", Description = "Pull Git Template.")]
        public class MarketCommand
        {
            private readonly IConsole _console;

            public MarketCommand(IConsole console)
            {
                _console = console;
            }

            [Option(Description = "Source Path")] public String Source { get; set; }
            [Option(Description = "Target Path")] public String Target { get; set; }

            private string ParseGitArgs()
            {
                var toPath = Path.Combine(AppContext.BaseDirectory, "RazorTemplates", Target);
                return $"clone --progress -v \"{Source}\" \"{toPath}\"";
            }

            private void OnExecute(IConsole console)
            {
                var startInfo = new ProcessStartInfo("git")
                {
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    ErrorDialog = true,
                    Arguments = ParseGitArgs()
                };

                using (var process = Process.Start(startInfo))
                {
                    process.OutputDataReceived += Process_OutputDataReceived;
                    ;
                    process.ErrorDataReceived += Process_ErrorDataReceived;
                    ;
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit();
                }
            }

            private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
            {
                if (!string.IsNullOrEmpty(e.Data))
                    _console.WriteLine(e.Data);
            }

            private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
            {
                if (!string.IsNullOrEmpty(e.Data))
                    _console.WriteLine(e.Data);
            }
        }
    }
}