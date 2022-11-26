using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Language;

namespace Engine
{
    public class ScriptContext
    {
        public string Script { get; set; }
        public string ContentName { get; set; }
        public string ApplicationName { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public static ConcurrentDictionary<DateTime, ScriptContext> History { get; } = new ConcurrentDictionary<DateTime, ScriptContext>();

        public ScriptContext()
        {
            History.TryAdd(DateTime.Now, this);
        }

        private Ast _ast; 
        public Ast Ast
        {
            get
            {
                if (_ast == null)
                {
                    var ast = Parser.ParseInput(Script, out Token[] tokens, out ParseError[] errors);
                    if (errors.Any()) return null;

                    _ast = ast;
                }

                return _ast;
            }
        }

        private List<string> _commands;

        public List<string> Commands
        {
            get
            {
                if (_commands == null)
                {
                    _commands = Ast.FindAll(x => x is CommandAst, true).Cast<CommandAst>().Select(m => m.GetCommandName()).Where(m => m != null).ToList();
                }
                return _commands;
            }
        }

        private List<string> _variables;

        public List<string> Variables
        {
            get
            {
                if (_variables == null)
                {
                    _variables = Ast.FindAll(x => x is VariableExpressionAst, true).Cast<VariableExpressionAst>().Select(m => m.VariablePath.UserPath).ToList();
                }
                return _variables;
            }
        }

        private List<string> _members;

        public List<string> Members
        {
            get
            {
                if (_members == null)
                {
                    _members = Ast.FindAll(x => x is MemberExpressionAst, true).Cast<MemberExpressionAst>().Select(m => m.Member.ToString()).ToList();
                }
                return _members;
            }
        }

        private List<string> _strings;

        public List<string> Strings
        {
            get
            {
                if (_strings == null)
                {
                    _strings = Ast.FindAll(x => x is StringConstantExpressionAst, true).Cast<StringConstantExpressionAst>().Select(m => m.Value).ToList();
                    _strings = _strings.Concat(Ast.FindAll(x => x is ExpandableStringExpressionAst, true).Cast<ExpandableStringExpressionAst>().Select(m => m.Value)).ToList();
                }
                return _strings;
            }
        }
    }
}
