using System.Collections;
using System.Collections.Generic;

namespace DT.BehaviourTrees {
  /// <summary>
  /// Attributes of a Inverter (Decorator) Node
  /// 1. Only 1 child
  /// 2. Inverts the child's return value (child success == pass failure to parent) and passes to parent
  /// </summary>
  public class BTDInverterNode : BTDecoratorNode {
    public BTDInverterNode(int nodeId, BehaviourTree tree, BTNode parent) : base(nodeId, tree, parent) {
      
    }
    
    protected virtual void ReturnStateBasedOnFinishedChild(BTNode child) {
      if (child.State == BTNodeState.SUCCESS) {
        Fail();
      } else if (child.State == BTNodeState.FAILURE) {
        Succeed();
      } else {
        // let parent handle failure state
        base.ReturnStateBasedOnFinishedChild(child);
      }
    }
  }
}