using Engine.Actions;
using Xunit;

namespace Engine.Tests.Actions
{
    public class BlockActionTest
    {
        [Fact]
        public void ShouldReturnAdminBlock()
        {
            var blockAction = new BlockAction();
            Assert.Equal(AnalyzeResult.AdminBlock, blockAction.Audit(null, new Configuration.Action
            {
            }));
        }
    }
}
