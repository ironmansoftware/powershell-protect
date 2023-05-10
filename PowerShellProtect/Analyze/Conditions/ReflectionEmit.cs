using Engine;
using Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Language;
using System.Text;

namespace PowerShellProtect.Analyze.Conditions
{
    internal class ReflectionEmit : ICondition
    {
        public string Name => "ReflectionEmit";

        public string Description => "There was an attempt to define a dynamic assembly in memory using the System.Reflection.Emit namespace.";

        public bool Analyze(ScriptContext context, Condition condition)
        {
            if (context?.Ast == null) return false;
            return context.Ast.FindAll(m => m is MemberExpressionAst ast && ast.Member.ToString().Equals("DefineDynamicAssembly", StringComparison.OrdinalIgnoreCase), true).Any() ||
                   context.Ast.FindAll(m => m is MemberExpressionAst ast && ast.Member.ToString().Equals("DefineDynamicModule", StringComparison.OrdinalIgnoreCase), true).Any() || 
                   context.Ast.FindAll(m => m is MemberExpressionAst ast && ast.Member.ToString().Equals("DefineField", StringComparison.OrdinalIgnoreCase), true).Any() ||
                   context.Ast.FindAll(m => m is MemberExpressionAst ast && ast.Member.ToString().Equals("DefineMethod", StringComparison.OrdinalIgnoreCase), true).Any() ||
                   context.Ast.FindAll(m => m is MemberExpressionAst ast && ast.Member.ToString().Equals("DefineType", StringComparison.OrdinalIgnoreCase), true).Any(); 
        }
    }
}
