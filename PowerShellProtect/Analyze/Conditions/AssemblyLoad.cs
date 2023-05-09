using Engine;
using Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Language;
using System.Reflection;
using System.Text;

namespace PowerShellProtect.Analyze.Conditions
{
    internal class AssemblyLoad : ICondition
    {
        public string Name => "AssemblyLoad";

        public string Description => "An attempt was made to load an assembly from memory.";

        public bool AnalyzeAsync(ScriptContext context, Condition condition)
        {
            if (context?.Ast == null) return false;
            var memberExpressions = context.Ast.FindAll(m => m is MemberExpressionAst, true).Cast<MemberExpressionAst>().Where(m => m.Member.ToString().Equals("load", StringComparison.OrdinalIgnoreCase));
            return memberExpressions.Any(m => m.Expression is TypeExpressionAst ast && ast.TypeName.FullName.ToLower().Contains("assembly"));
        }
    }
}
