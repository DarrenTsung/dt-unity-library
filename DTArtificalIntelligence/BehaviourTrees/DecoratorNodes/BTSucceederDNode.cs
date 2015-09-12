using System.Collections;
using System.Collections.Generic;

namespace DT.BehaviourTrees {
  /// <summary>
  /// Attributes of a Succeeder (Decorator) Node
  /// 1. Only 1 child
  /// 2. Always succeeds, no matter what the child returns
  /// </summary>
  public class BTSucceederDNode : BTDecoratorNode {
    public BTSucceederDNode(int nodeId, BehaviourTree tree, BTNode parent) : base(nodeId, tree, parent) {}
    
    protected override void ReturnStateBasedOnFinishedChild(BTNode child) {
      if (child.State == BTNodeState.SUCCESS || child.State == BTNodeState.FAILURE) {
        Succeed();
      } else {
        // let parent handle invalid state
        base.ReturnStateBasedOnFinishedChild(child);
      }
    }
  }
}