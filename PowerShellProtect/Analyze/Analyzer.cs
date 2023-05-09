using Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Audit;
using Engine.Actions;
using Engine.Analyze.Conditions;
using PowerShellProtect.Analyze.Conditions;

namespace Engine
{
    public class Analyzer
    {
        private readonly IDictionary<string, ICondition> _conditions;
        internal readonly Config _config;
        private readonly IDictionary<string, IAction> _actions;
        private readonly List<ICondition> _builtInConditions;
        public Analyzer()
        {
            _config = new Config();

            _actions = new List<IAction>
            {
                new FileAuditAction(),
                new BlockAction(),
                new TcpAuditAction(),
                new HttpAuditAction(),
                new UdpAuditAction(),
                new EventLogAuditAction(),
                new UniversalAuditAction()
            }.ToDictionary(m => m.Type.ToLower(), m => m);

            _conditions = new List<ICondition> {
                new AdministratorCondition(),
                new ApplicationCondition(),
                new ApplicationHashCondition(),
                new AssemblyCondition(),
                new AssemblyHashCondition(),
                new DomainControllerCondition(),
                new ComputerNameCondition(),
                new DomainCondition(),
                new CommandCondition(),
                new ScriptCondition(),
                new ContentPathCondition(),
                new VariableCondition(),
                new MemberCondition(),
                new ScriptStringCondition(),
                new AIisBestPracticeCondition(),
                new AIisSuspiciousCondition()
            }.ToDictionary(m => m.Name.ToLower(), m => m);

            _builtInConditions = new List<ICondition>
            {
                new AmsiBypass(),
                new LoggingBypass(),
                new DisableDefender(),
                new PowerSploit(),
                new AssemblyLoad(),
                new ReflectionEmit(),
                new MarshalClass(),
                new PersistentWmi(),
                new BloudHound(),
                new Kerberoasting(),
                new InvokeExpression(),
                new Log4J()
            };

            foreach (var builtInCondition in _builtInConditions)
            {
                _conditions.Add(builtInCondition.Name.ToLower(), builtInCondition);
            }
        }

        internal Analyzer(IEnumerable<ICondition> conditions, Config config, IEnumerable<IAction> actions)
        {
            _conditions = conditions.ToDictionary(m => m.Name.ToLower(), m => m);
            _config = config;
            _actions = actions.ToDictionary(m => m.Type.ToLower(), m => m);
            _builtInConditions = new List<ICondition>();
        }

        public AnalyzeResult Analyze(ScriptContext scriptContext)
        {
            var configuration = _config.GetConfiguration();

            var rules = configuration.Rules;

            if (configuration.BuiltIn?.Enabled == null || configuration.BuiltIn?.Enabled == true)
            {
                foreach (var condition in _builtInConditions.Where(m => configuration?.BuiltIn?.DisabledConditions == null || !configuration.BuiltIn.DisabledConditions.Select(x => x.ToLower()).Contains(m.Name.ToLower())))
                {
                    try
                    {
                        var result = condition.AnalyzeAsync(scriptContext, null);
                        if (result)
                        {
                            Log.LogError($"PowerShell Protect blocked a script from running due to a violation of the {condition.Name} rule. {condition.Description}", 101);
                            return AnalyzeResult.AdminBlock;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.LogError($"Failed to run condition: {condition.Name}. {Environment.NewLine} {ex.Message} {Environment.NewLine} {scriptContext.Script}");
                    }

                }
            }

            var analyzerResult = AnalyzeResult.Ok;
            foreach (var rule in rules)
            {
                bool match = true;

                var conditions = rule.Conditions;

                foreach (var condition in conditions)
                {
                    if (!_conditions.ContainsKey(condition.Property.ToLower()))
                    {
                        throw new Exception("Unknown property: " + condition.Property);
                    }

                    var c = _conditions[condition.Property.ToLower()];
                    var result = c.AnalyzeAsync(scriptContext, condition);
                    if (result && rule.AnyCondition)
                    {
                        match = true;
                        break;
                    }
                    else if (!result && !rule.AnyCondition)
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    foreach (var actionRef in rule.Actions)
                    {
                        var action = configuration.Actions.FirstOrDefault(m => m.Name.Equals(actionRef.Name, System.StringComparison.OrdinalIgnoreCase));
                        if (action == null) throw new Exception("Unknown action: " + actionRef.Name);

                        if (!_actions.ContainsKey(action.Type.ToLower()))
                        {
                            throw new Exception("Unknown action type: " + action.Type);
                        }

                        var destination = _actions[action.Type.ToLower()];

                        var auditMessage = new AuditMessage
                        {
                            ApplicationName = scriptContext.ApplicationName,
                            Rule = rule.Name,
                            ContentPath = scriptContext.ContentName,
                            Script = scriptContext.Script,
                            ComputerName = Environment.MachineName,
                            DomainName = Environment.UserDomainName,
                            UserName = Environment.UserName
                        };

                        var result = destination.Audit(auditMessage, action);
                        if (result == AnalyzeResult.AdminBlock)
                        {
                            analyzerResult = result;
                        }
                    }
                }
            }

            return analyzerResult;
        }
    }
}
