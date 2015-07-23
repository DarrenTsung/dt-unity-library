using System.Collections;
using System.Collections.Generic;

namespace DT.BehaviourTrees {
  /// <summary>
  /// Attributes of a Leaf Node
  /// 1. No children
  /// 2. Does logic per tick and returns state to parent
  /// 
  /// Default implementation here does nothing per tick
  /// </summary>
  public abstract class BTLeafNode : BTNode {
    public BTLeafNode(int nodeId, BehaviourTree tree, BTNode parent) : base(nodeId, tree, parent) {}
    
    protected override BTNode SelectChildToProcess() {
      return null;
    }
    
    protected override void HandleChildFinish(BTNode child) {
      Locator.Logger.LogError("BTLeafNode::HandleChildFinish - leaf node should not have a child that could've finished!");
    }
    
    protected override bool CanAddChild(BTNode child, ref string errorMessage) {
      return false;
    }
    
    protected override void HandleStart() {
      base.HandleStart();
      
      this.Init();
    }
  }
}