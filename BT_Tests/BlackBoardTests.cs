using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT;
using NUnit.Framework;

namespace BT_Tests
{
    [TestFixture]
    public class BlackBoardTests
    {
        [Test]
        public void BlackBoardAddNull()
        {
            var bb = new BlackBoard();
            Assert.That(()=> bb.SetValue("pos", null), Throws.Nothing);
        }
        [Test]
        public void BlackBoardAddVal()
        {
            var bb = new BlackBoard();
            Assert.That(() => bb.SetValue("pos", 3f), Throws.Nothing);
        }
        [Test]
        public void BlackBoardAddSameValMultipleTimesNoEx_Green()
        {
            var bb = new BlackBoard();
            bb.SetValue("pos", 10);
            Assert.That(() => bb.SetValue("pos", 5), Throws.Nothing);
            Assert.That(bb.ReadValue<int>("pos"), Is.EqualTo(5));
        }
        [Test]
        public void BlackBoardAddSameValMultipleTimesNoEx_Red()
        {
            var bb = new BlackBoard();
            bb.SetValue("pos", 5);
            Assert.That(() => bb.SetValue("pos", 20), Throws.Nothing);
            Assert.That(bb.ReadValue<int>("pos"), Is.Not.EqualTo(5));
        }
        [Test]
        public void BlackBoardReadWrongTypeWithImplicitCastEx()
        {
            var bb = new BlackBoard();
            bb.SetValue("pos", 5);
            Assert.That(() => bb.ReadValue<float>("pos"), Throws.Exception);
        }
        [Test]
        public void BlackBoardReadWrongTypeWithoutImplicitCastEx()
        {
            var bb = new BlackBoard();
            bb.SetValue("pos", true);
            Assert.That(() => bb.ReadValue<Node>("pos"), Throws.Exception);
        }
        [Test]
        public void BlackBoardReadFloat()
        {
            var bb = new BlackBoard();
            bb.SetValue("foo", 10f);
            Assert.That(() => bb.ReadValue<float>("foo"), Throws.Nothing);
            Assert.That(bb.ReadValue<float>("foo"), Is.EqualTo(10f).Within(float.Epsilon));
        }
        [Test]
        public void BlackBoardReadFloatIntoIntEx()
        {
            var bb = new BlackBoard();
            bb.SetValue("foo", -50f);
            Assert.That(() => bb.ReadValue<int>("foo"), Throws.Exception.TypeOf<InvalidCastException>());
        }
        [Test]
        public void BlackBoardReadUnexistingKeyEx()
        {
            var bb = new BlackBoard();
            bb.SetValue("foo", 10f);
            Assert.That(() => bb.ReadValue<float>("bar"), Throws.Exception.TypeOf<KeyNotFoundException>());
        }
        [Test]
        public void BlackBoardReadTypeObjectNoEx()
        {
            var bb = new BlackBoard();
            bb.SetValue("foo", 10f);
            Assert.That(() => bb.ReadValue<object>("foo"), Throws.Nothing);
        }
        [Test]
        public void BlackBoardReadTypeObject()
        {
            var bb = new BlackBoard();
            bb.SetValue("foo", 1);
            Assert.That(bb.ReadValue<object>("foo"), Is.EqualTo(1));    //not sure about this
        }
    }
}
