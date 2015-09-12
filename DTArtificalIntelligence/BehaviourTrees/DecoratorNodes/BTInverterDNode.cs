using System.Collections;
using System.Collections.Generic;

namespace DT.BehaviourTrees {
  /// <summary>
  /// Attributes of a Inverter (Decorator) Node
  /// 1. Only 1 child
  /// 2. Inverts the child's return value (child success == pass failure to parent) and passes to parent
  /// </summary>
  public class BTInverterDNode : BTDecoratorNode {
    public BTInverterDNode(int nodeId, BehaviourTree tree, BTNode parent) : base(nodeId, tree, parent) {}
    
    protected override void ReturnStateBasedOnFinishedChild(BTNode child) {
      if (child.State == BTNodeState.SUCCESS) {
        Fail();
      } else if (child.State == BTNodeState.FAILURE) {
        Succeed();
      } else {
        // let parent handle invalid state
        base.ReturnStateBasedOnFinishedChild(child);
      }
    }
  }
}