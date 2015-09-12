using System.Collections;
using System.Collections.Generic;

namespace DT.BehaviourTrees {
  /// <summary>
  /// Attributes of a RepeatUntilFailure (Decorator) Node
  /// 1. Only 1 child
  /// 2. Repeats until the child returns failure (to flip the logic, place an inverter under this node)
  /// </summary>
  public class BTRepeatUntilFailureDNode : BTDecoratorNode {
    public BTRepeatUntilFailureDNode(int nodeId, BehaviourTree tree, BTNode parent) : base(nodeId, tree, parent) {}
    
    protected override void ReturnStateBasedOnFinishedChild(BTNode child) {
      if (child.State == BTNodeState.SUCCESS) {
        this.ProcessOneChild();
      } else if (child.State == BTNodeState.FAILURE) {
        Fail();
      } else {
        // let parent handle invalid state
        base.ReturnStateBasedOnFinishedChild(child);
      }
    }
  }
}