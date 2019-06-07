using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using BT;
using BT.Decorators;

namespace BT_Tests
{
    public class ConditionSuccess : Decorator
    {
        public override bool OnNodeCondition(BT.BT bt)
        {
            return true;
        }
    }
    public class ConditionFail : Decorator
    {
        public override bool OnNodeCondition(BT.BT bt)
        {
            return false;
        }
    }


    [TestFixture]
    public class DecoratorsTests
    {
        [Test]
        public void DecoratorAppendNullThrowsException()
        {
            var node = new TestNode();
            Assert.That(() => node.AppendDecorator(null), Throws.ArgumentNullException);
        }
        [Test]
        public void DecoratorAppendNoException()
        {
            var node = new TestNode();
            Assert.That(() => node.AppendDecorator(new ConditionFail()), Throws.Nothing);
        }




        [Test]
        public void ConditionFailSingle()
        {
            var root = new SequenceNode();
            var succ = new TaskSuccessNode();
            succ.AppendDecorator(new ConditionFail());
            root.Append(succ);
            root.Append(new TaskFailureNode());

            Assert.That(root.Run(null), Is.EqualTo(NodeState.Failure));         //should skip the success condition
            Assert.That(succ.executed, Is.EqualTo(false));
        }
        [Test]
        public void ConditionSuccessSingle()
        {
            var root = new SequenceNode();
            var succ = new TaskSuccessNode();
            succ.AppendDecorator(new ConditionSuccess());
            root.Append(succ);

            var fail = new TaskFailureNode();
            fail.AppendDecorator(new ConditionFail());
            root.Append(fail);

            Assert.That(root.Run(null), Is.EqualTo(NodeState.Success));         //should skip the fail node
            Assert.That(succ.executed, Is.EqualTo(true));
            Assert.That(fail.executed, Is.EqualTo(false));
        }

        [Test]
        public void ConditionMultiple()
        {
            var root = new SequenceNode();
            var succ = new TaskSuccessNode();
            succ.AppendDecorator(new ConditionSuccess());
            succ.AppendDecorator(new ConditionFail());
            root.Append(succ);
            root.Append(new TaskFailureNode());

            //expected condition false
            Assert.That(root.Run(null), Is.EqualTo(NodeState.Failure));
            Assert.That(succ.executed, Is.EqualTo(false));
        }

        [Test]
        public void ConditionFromBlackboard_Green()
        {
            BT.BT bt = new BT.BT(null);
            bt.BlackBoard.SetValue("foo", 10);
            bt.BlackBoard.SetValue("bar", 10);

            var dec = new BlackBoardCondition<IntCondition, int>("foo", "bar", ConditionType.Equal);
            Assert.That(dec.OnNodeCondition(bt), Is.EqualTo(true));
        }
        [Test]
        public void ConditionFromBlackboard_Red()
        {
            BT.BT bt = new BT.BT(null);
            bt.BlackBoard.SetValue("foo", 10);
            bt.BlackBoard.SetValue("bar", 5);

            var dec = new BlackBoardCondition<IntCondition, int>("foo", "bar", ConditionType.Equal);
            Assert.That(dec.OnNodeCondition(bt), Is.EqualTo(false));
        }
        [Test]
        public void CoditionGreater_Green()
        {
            BT.BT bt = new BT.BT(null);
            bt.BlackBoard.SetValue("foo", 10);
            bt.BlackBoard.SetValue("bar", 5);

            var dec = new BlackBoardCondition<IntCondition, int>("foo", "bar", ConditionType.Greater);
            Assert.That(dec.OnNodeCondition(bt), Is.EqualTo(true));
        }
        [Test]
        public void CoditionGreater_Red()
        {
            BT.BT bt = new BT.BT(null);
            bt.BlackBoard.SetValue("foo", 100);
            bt.BlackBoard.SetValue("bar", 5);

            var dec = new BlackBoardCondition<IntCondition, int>("bar", "foo", ConditionType.Greater);
            Assert.That(dec.OnNodeCondition(bt), Is.EqualTo(false));
        }

        [Test]
        public void CoditionLess_Green()
        {
            BT.BT bt = new BT.BT(null);
            bt.BlackBoard.SetValue("foo", 0);
            bt.BlackBoard.SetValue("bar", 200);

            var dec = new BlackBoardCondition<IntCondition, int>("foo", "bar", ConditionType.Less);
            Assert.That(dec.OnNodeCondition(bt), Is.EqualTo(true));
        }
        [Test]
        public void CoditionLess_Red()
        {
            BT.BT bt = new BT.BT(null);
            bt.BlackBoard.SetValue("foo", 0);
            bt.BlackBoard.SetValue("bar", 200);

            var dec = new BlackBoardCondition<IntCondition, int>("bar", "foo", ConditionType.Less);
            Assert.That(dec.OnNodeCondition(bt), Is.EqualTo(false));
        }

        [Test]
        public void ConditionBool_Green()
        {
            BT.BT bt = new BT.BT(null);
            bt.BlackBoard.SetValue("foo", true);

            var dec = new BlackBoardBoolCondition("foo");
            Assert.That(dec.OnNodeCondition(bt), Is.EqualTo(true));
        }
        [Test]
        public void ConditionBool_Red()
        {
            BT.BT bt = new BT.BT(null);
            bt.BlackBoard.SetValue("foo", false);

            var dec = new BlackBoardBoolCondition("foo");
            Assert.That(dec.OnNodeCondition(bt), Is.Not.EqualTo(true));
        }

        [Test]
        public void WriteOnFailure_Green()
        {
            var root = new SequenceNode();
            var fail = new TaskFailureNode();
            fail.AppendDecorator(new SetValueOnFail("foo", () => 1));
            root.Append(fail);

            BT.BT bt = new BT.BT(root);
            bt.BlackBoard.SetValue("foo", 0);

            Assert.That(bt.BlackBoard.ReadValue<int>("foo"), Is.EqualTo(0));
            bt.Run();
            Assert.That(bt.BlackBoard.ReadValue<int>("foo"), Is.EqualTo(1));
        }
        [Test]
        public void WriteOnFailure_Red()
        {
            var root = new SequenceNode();
            var suc = new TaskSuccessNode();
            suc.AppendDecorator(new SetValueOnFail("foo", () => 1));
            root.Append(suc);

            BT.BT bt = new BT.BT(root);
            bt.BlackBoard.SetValue("foo", 0);

            Assert.That(bt.BlackBoard.ReadValue<int>("foo"), Is.EqualTo(0));
            bt.Run();
            Assert.That(bt.BlackBoard.ReadValue<int>("foo"), Is.EqualTo(0));
        }

        [Test]
        public void Tag_Green()
        {
            var root = new SequenceNode();
            var suc = new TaskSuccessNode();
            suc.AppendDecorator(new Tag("foo"));
            root.Append(suc);

            var bt = new BT.BT(root);
            bt.Run();

            Assert.That(bt.GetActiveTag(), Is.EqualTo("foo"));
        }

        [Test]
        public void SetValueAfterExecution_Green()
        {
            var root = new SequenceNode();
            var bt = new BT.BT(root);

            var suc = new TaskSuccessNode();
            suc.AppendDecorator(new SetValueAfterExecution("foo", () => 10)); //for example set target position 
            root.Append(suc);

            bt.Run();

            Assert.That(bt.BlackBoard.ReadValue<int>("foo"), Is.EqualTo(10));
        }

        [Test]
        public void ConditionConstant_Green()
        {
            var root = new SequenceNode();
            var bt = new BT.BT(root);
            bt.BlackBoard.SetValue("foo", 0);

            var suc = new TaskSuccessNode();
            suc.AppendDecorator(new BlackBoardConstantCondition<IntCondition, int>("foo", 10, ConditionType.Equal)); //for example set target position 
            root.Append(suc);

            bt.Run();

            Assert.That(suc.executed, Is.EqualTo(false));
        }
        [Test]
        public void ConditionConstant_Red()
        {
            var root = new SequenceNode();
            var bt = new BT.BT(root);
            bt.BlackBoard.SetValue("foo", 10);

            var suc = new TaskSuccessNode();
            suc.AppendDecorator(new BlackBoardConstantCondition<IntCondition, int>("foo", 10, ConditionType.Equal)); //for example set target position 
            root.Append(suc);

            bt.Run();

            Assert.That(suc.executed, Is.Not.EqualTo(false));
        }
    }
}
