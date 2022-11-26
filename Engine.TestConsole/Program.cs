using Engine.Actions;
using Engine.Analyze.Conditions;
using Engine.Audit;
using Engine.Configuration;
using System.Collections.Generic;

namespace Engine.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ProtectEngine.Analyze("invoke-mimikatz", "", "powershell.exe");
        }

        
    }
}
