using NUnit.Framework;

namespace Svg.UnitTests
{
    [TestFixture]
    public class SvgAttributeCollectionTests
    {
        [Test]
        public void TestGetInheritedAttribute()
        {
            var owner = new SvgCircle();
            var parent = new SvgFragment();
            parent.Children.Add(owner);
            parent.Attributes["test"] = "parent";

            owner.Attributes["test"] = "owner";
            Assert.AreEqual("owner", owner.Attributes.GetInheritedAttribute("test", true, "default"));
            Assert.AreEqual("owner", owner.Attributes.GetInheritedAttribute("test", false, "default"));

            owner.Attributes["test"] = "inherit";
            Assert.AreEqual("parent", owner.Attributes.GetInheritedAttribute("test", true, "default"));
            Assert.AreEqual("parent", owner.Attributes.GetInheritedAttribute("test", false, "default"));

            owner.Attributes.Remove("test");
            Assert.AreEqual("parent", owner.Attributes.GetInheritedAttribute("test", true, "default"));
            Assert.AreEqual("default", owner.Attributes.GetInheritedAttribute("test", false, "default"));
        }

        [Test]
        public void TestGetAttribute()
        {
            var owner = new SvgCircle();
            var parent = new SvgFragment();
            parent.Children.Add(owner);
            parent.Attributes["test"] = "parent";

            owner.Attributes["test"] = "owner";
            Assert.AreEqual("owner", owner.Attributes.GetAttribute<string>("test"));

            owner.Attributes["test"] = "inherit";
            Assert.AreEqual("inherit", owner.Attributes.GetAttribute<string>("test"));

            owner.Attributes.Remove("test");
            Assert.IsNull(owner.Attributes.GetAttribute<string>("test"));
        }
    }
}
