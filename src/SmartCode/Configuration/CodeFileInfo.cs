using System.Collections.Generic;

namespace SmartCode.Configuration;

public class CodeFileInfo
{
    /// <summary>
    /// 项目的名称
    /// </summary>
    public string ProjectName { get; set; }
    
    /// <summary>
    /// 项目位置
    /// </summary>
    public string ProjectPath { get; set; }

    /// <summary>
    /// 代码结构位置
    /// </summary>
    public Dictionary<string, string> Path = new Dictionary<string, string>();
}