using JetBrains.dotMemoryUnit;
using JetBrains.dotMemoryUnit.Kernel;
using NUnit.Framework;

namespace Svg.UnitTests
{
    [DotMemoryUnit(FailIfRunWithoutSupport = false)]
    [TestFixture]
    public class SvgElementMemoryUsageTests
    {
        [DotMemoryUnit(CollectAllocations=true)]
        [Test]
        public void EmptyClass_new()
        {   
            var snap1 = dotMemoryApi.GetSnapshot();

            var result = new EmptyClass();

            var snap2 = dotMemoryApi.GetSnapshot();
            var traffic = dotMemoryApi.GetTrafficBetween(snap1, snap2);
            var o = traffic.Where(w => w.Type.Is<EmptyClass>());
            Assert.AreEqual(1, o.AllocatedMemory.ObjectsCount);
            Assert.AreEqual(24, o.AllocatedMemory.SizeInBytes);
        }

        [DotMemoryUnit(CollectAllocations=true)]
        [Test]
        public void SvgElementEmpty_new()
        {
            var snap1 = dotMemoryApi.GetSnapshot();

            var result = new SvgElementEmpty();

            var snap2 = dotMemoryApi.GetSnapshot();
            var traffic = dotMemoryApi.GetTrafficBetween(snap1, snap2);

            var o = traffic.Where(w => w.Type.Is<SvgElementEmpty>());
            Assert.AreEqual(1, o.AllocatedMemory.ObjectsCount);
            Assert.AreEqual(200, o.AllocatedMemory.SizeInBytes);
        }
    }
}
