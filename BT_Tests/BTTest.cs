using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using BT;

namespace BT_Tests
{
    public class TestNode : Node
    {
        public override NodeState Run(BT.BT bt)
        {
            return default(NodeState);
        }
    }
    public class TaskSuccessNode : TaskNode
    {
        public bool executed = false;
        public override NodeState Run(BT.BT bt)
        {
            executed = true;
            return NodeState.Success;
        }
    }
    public class TaskFailureNode : TaskNode
    {
        public bool executed = false;
        public override NodeState Run(BT.BT bt)
        {
            executed = true;
            return NodeState.Failure;
        }
    }
    public class TaskRunningNode : TaskNode
    {
        public int counter = 0;
        public override NodeState Run(BT.BT bt)
        {
            if (++counter < 5)
                return NodeState.Running;

            counter = 0;
            return NodeState.Success;   //NB success means that the Selector will be interrupted
        }
    }


    [TestFixture]
    public class BTTest
    {
        [Test]
        public void BtInitNoException()
        {
            Node root = new SelectorNode();
            Assert.That(() => new BT.BT(root), Throws.Nothing);   
        }
        [Test]
        public void BtInitEmptyNoException()
        {
            Assert.That(() => new BT.BT(null), Throws.Nothing);
        }

        [Test]
        public void BtRunEmptyThrowsException()
        {
            Assert.That(() => new BT.BT(null).Run(), Throws.Exception.TypeOf<NullReferenceException>());
        }
        [Test]
        public void BtRunThrowsNoException()
        {
            var root = new SelectorNode();
            Assert.That(() => new BT.BT(root).Run(), Throws.Nothing);
        }



        //node append
        [Test]
        public void NodeAppendNullThrowsException()
        {
            var node = new TestNode();
            Assert.That(() => node.Append(null), Throws.ArgumentNullException);
        }
        [Test]
        public void NodeAppend_Green()
        {
            var node = new TestNode();
            var node2 = new TestNode();
            node.Append(node2);

            Assert.That(node.ChildrenCount, Is.Not.EqualTo(0));
        }
        [Test]
        public void NodeAppend_Red()
        {
            var node = new TestNode();
            Assert.That(node.ChildrenCount, Is.EqualTo(0));
        }
        [Test]
        public void NodeAppendItself()
        {
            var node = new TestNode();
            Assert.That(() => node.Append(node), Throws.ArgumentException);
        }
        [Test]
        public void NodeAppendNoNull()
        {
            var node = new TestNode();
            var node2 = new TestNode();
            Assert.That(() => node.Append(node2), Throws.Nothing);
            Assert.That(node.ChildrenCount, Is.Not.EqualTo(0));
            Assert.That(node.ChildrenCount, Is.EqualTo(1));
        }
        [Test]
        public void NodeAppendMultiple()
        {
            var node = new TestNode();
            var node2 = new TestNode();
            Assert.That(() => node.Append(node2), Throws.Nothing);
            Assert.That(() => node.Append(node2), Throws.ArgumentException);
            Assert.That(node.ChildrenCount, Is.EqualTo(1));
        }
        [Test]
        public void NodeParentNull()
        {
            var node = new TestNode();
            Assert.That(node.Parent, Is.Null);
        }
        [Test]
        public void NodeParentNotNull()
        {
            var node = new TestNode();
            var node2 = new TestNode();
            node.Append(node2);
            Assert.That(node.Parent, Is.Null);
            Assert.That(node2.Parent, Is.Not.Null);
        }

        //Selector node
        [Test]
        public void Selector_Green()
        {
            var root = new SelectorNode();
            root.Append(new TaskSuccessNode());
            var failure = new TaskFailureNode();
            root.Append(failure);

            Assert.That(() => root.Run(null), Throws.Nothing);
            Assert.That(root.Run(null), Is.EqualTo(NodeState.Success)); //first is success so I expect the first node returns true
            Assert.That(failure.executed, Is.EqualTo(false));
        }
        [Test]
        public void Selector_Red()
        {
            var root = new SelectorNode();
            var failure = new TaskFailureNode();
            root.Append(failure);
            root.Append(new TaskSuccessNode());

            Assert.That(() => root.Run(null), Throws.Nothing);
            Assert.That(root.Run(null), Is.Not.EqualTo(NodeState.Failure)); //ignore failures and return last success
            Assert.That(failure.executed, Is.EqualTo(true));
        }
        [Test]
        public void Selector_AllFailures()
        {
            var root = new SelectorNode();
            var failure = new TaskFailureNode();
            root.Append(failure);
            root.Append(new TaskFailureNode());

            Assert.That(() => root.Run(null), Throws.Nothing);
            Assert.That(root.Run(null), Is.EqualTo(NodeState.Failure));
        }
        [Test]
        public void SelectorRunning()
        {
            var Selector = new SelectorNode();
            var running = new TaskRunningNode();
            var node = new TaskSuccessNode();
            Selector.Append(running);
            Selector.Append(node);

            Assert.That(Selector.Run(null), Is.EqualTo(NodeState.Running));
            Assert.That(Selector.Run(null), Is.EqualTo(NodeState.Running));
            Assert.That(Selector.Run(null), Is.EqualTo(NodeState.Running));
            Assert.That(Selector.Run(null), Is.EqualTo(NodeState.Running));

            Assert.That(Selector.Run(null), Is.EqualTo(NodeState.Success));
            Assert.That(node.executed, Is.EqualTo(false));  //next node in Selector will be never executed
        }
        [Test]
        public void SelectorRunningWithFailure()
        {
            var Selector = new SelectorNode();
            var failure = new TaskFailureNode();
            var running = new TaskRunningNode();
            var node = new TaskSuccessNode();
            Selector.Append(failure);
            Selector.Append(running);
            Selector.Append(node);

            //same asserts as before, failure will be ignored
            Assert.That(Selector.Run(null), Is.EqualTo(NodeState.Running));
            Assert.That(Selector.Run(null), Is.EqualTo(NodeState.Running));
            Assert.That(Selector.Run(null), Is.EqualTo(NodeState.Running));
            Assert.That(Selector.Run(null), Is.EqualTo(NodeState.Running));

            Assert.That(Selector.Run(null), Is.EqualTo(NodeState.Success));
            Assert.That(failure.executed, Is.EqualTo(true));
            Assert.That(node.executed, Is.EqualTo(false));  //next node in Selector will be never executed
        }

        //sequence
        [Test]
        public void Sequence_Green()
        {
            var sequence = new SequenceNode();
            sequence.Append(new TaskSuccessNode());
            sequence.Append(new TaskSuccessNode());
            sequence.Append(new TaskSuccessNode());

            Assert.That(sequence.Run(null), Is.EqualTo(NodeState.Success));
        }
        [Test]
        public void Sequence_Red()
        {
            var sequence = new SequenceNode();
            sequence.Append(new TaskFailureNode());
            sequence.Append(new TaskSuccessNode());
            sequence.Append(new TaskSuccessNode());

            Assert.That(sequence.Run(null), Is.Not.EqualTo(NodeState.Success));
        }
        [Test]
        public void SequenceFailCalled()
        {
            var sequence = new SequenceNode();
            var fail = new TaskFailureNode();
            var success = new TaskSuccessNode();
            sequence.Append(new TaskSuccessNode());
            sequence.Append(fail);
            sequence.Append(success);

            Assert.That(sequence.Run(null), Is.EqualTo(NodeState.Failure));
            Assert.That(fail.executed, Is.EqualTo(true));
            Assert.That(success.executed, Is.EqualTo(false));
        }
        [Test]
        public void SequenceRunning_Green()
        {
            var sequence = new SequenceNode();
            var run = new TaskRunningNode();
            var succ = new TaskSuccessNode();

            sequence.Append(run);
            sequence.Append(succ);

            Assert.That(sequence.Run(null), Is.EqualTo(NodeState.Running));
            Assert.That(sequence.Run(null), Is.EqualTo(NodeState.Running));
            Assert.That(sequence.Run(null), Is.EqualTo(NodeState.Running));
            Assert.That(sequence.Run(null), Is.EqualTo(NodeState.Running));

            Assert.That(succ.executed, Is.EqualTo(false));  //node not called
            Assert.That(sequence.Run(null), Is.EqualTo(NodeState.Success)); //step forward, tree unlocked, next node called
            Assert.That(succ.executed, Is.EqualTo(true));   //condition satisfied
        }
        [Test]
        public void SequenceRunning_Red()
        {
            var sequence = new SequenceNode();
            var run = new TaskRunningNode();
            var fail = new TaskFailureNode();
            var succ = new TaskSuccessNode();

            sequence.Append(run);
            sequence.Append(fail);
            sequence.Append(succ);

            Assert.That(sequence.Run(null), Is.EqualTo(NodeState.Running));
            Assert.That(sequence.Run(null), Is.EqualTo(NodeState.Running));
            Assert.That(sequence.Run(null), Is.EqualTo(NodeState.Running));
            Assert.That(sequence.Run(null), Is.EqualTo(NodeState.Running));

            Assert.That(sequence.Run(null), Is.EqualTo(NodeState.Failure));

            Assert.That(fail.executed, Is.EqualTo(true));   //condition satisfied
            Assert.That(succ.executed, Is.EqualTo(false));   //condition satisfied
        }
    }
}
