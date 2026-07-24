using Engine.Configuration;
using NSubstitute;
using Engine.Analyze;
using System.Collections.Generic;
using Xunit;

namespace Engine.Tests.Analyze
{
    public class AnalyzerTest
    {
        [Fact]
        public void ShouldBlockWhenAiScannerMarksScriptAsHarmful()
        {
            var configProvider = Substitute.For<IConfigProvider>();
            configProvider.GetConfiguration().Returns(new Engine.Configuration.Configuration
            {
                AI = new AiConfiguration
                {
                    Enabled = true,
                    Provider = "OpenAI",
                    Model = "test-model",
                    ApiKey = "test-key"
                }
            });

            var scanner = Substitute.For<IAiScriptScanner>();
            scanner.Scan(Arg.Any<ScriptContext>(), Arg.Any<AiConfiguration>()).Returns(AiScanResult.Harmful);

            var analyzer = new Analyzer(
                new ICondition[0],
                new Config(new[] { configProvider }),
                new IAction[0],
                scanner);

            Assert.Equal(AnalyzeResult.AdminBlock, analyzer.Analyze(new ScriptContext { Script = "Get-Process" }));
        }

        private Analyzer analyzer;

        [Fact]
        public void ShouldBlockWithMultipleConditions()
        {
            var condition = Substitute.For<ICondition>();
            condition.Analyze(Arg.Any<ScriptContext>(), Arg.Any<Condition>()).Returns(true);
            condition.Name.Returns("test");

            var condition2 = Substitute.For<ICondition>();
            condition2.Analyze(Arg.Any<ScriptContext>(), Arg.Any<Condition>()).Returns(true);
            condition2.Name.Returns("test2");

            var configProvider = Substitute.For<IConfigProvider>();
            configProvider.GetConfiguration().Returns(new Engine.Configuration.Configuration
            {
                Actions = new List<Engine.Configuration.Action>
                {
                    new Engine.Configuration.Action
                    {
                         Name = "block",
                         Type = "block"
                    }
                },
                Rules = new List<Rule>
                {
                    new Rule
                    {
                        Actions = new List<ActionRef>
                        {
                            new ActionRef
                            {
                                Name = "block"
                            }
                        },
                        Conditions = new List<Condition>
                        {
                            new Condition
                            {
                                Property = "test"
                            },
                            new Condition
                            {
                                Property = "test2"
                            }
                        }
                    }
                }
            });

            var action = Substitute.For<IAction>();
            action.Type.Returns("block");
            action.Audit(Arg.Any<AuditMessage>(), Arg.Any<Engine.Configuration.Action>()).Returns(AnalyzeResult.AdminBlock);

            var config = new Config(new List<IConfigProvider> { configProvider });

            analyzer = new Analyzer(new List<ICondition> { condition, condition2 }, config, new List<IAction> { action });
            var result = analyzer.Analyze(new ScriptContext());

            Assert.Equal(AnalyzeResult.AdminBlock, result);
        }

        [Fact]
        public void ShouldBlock()
        {
            var condition = Substitute.For<ICondition>();
            condition.Analyze(Arg.Any<ScriptContext>(), Arg.Any<Condition>()).Returns(true);
            condition.Name.Returns("test");

            var configProvider = Substitute.For<IConfigProvider>();
            configProvider.GetConfiguration().Returns(new Engine.Configuration.Configuration
            {
                Actions = new List<Engine.Configuration.Action>
                {
                    new Engine.Configuration.Action
                    {
                         Name = "block",
                         Type = "block"                         
                    }
                },
                Rules = new List<Rule>
                {
                    new Rule
                    {
                        Actions = new List<ActionRef>
                        {
                            new ActionRef
                            {
                                Name = "block"
                            }
                        },
                        Conditions = new List<Condition>
                        {
                            new Condition
                            {
                                Property = "test"
                            }
                        }
                    }
                }
            });

            var action = Substitute.For<IAction>();
            action.Type.Returns("block");
            action.Audit(Arg.Any<AuditMessage>(), Arg.Any<Engine.Configuration.Action>()).Returns(AnalyzeResult.AdminBlock);
            
            var config = new Config(new List<IConfigProvider> { configProvider });

            analyzer = new Analyzer(new List<ICondition> { condition }, config, new List<IAction> { action });
            var result = analyzer.Analyze(new ScriptContext());

            Assert.Equal(AnalyzeResult.AdminBlock, result);
        }
    }
}
