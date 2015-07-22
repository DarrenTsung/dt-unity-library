using System.Collections;
using System.Collections.Generic;

namespace DT.BehaviourTrees {
  /// <summary>
  /// Attributes of a Decorator Node
  /// 1. Only 1 child
  /// 2. Modifies the child's return value and passes to parent
  /// 
  /// Default implementation here just passes the child's return value to the parent
  /// </summary>
  public class BTDecoratorNode : BTNode {
    public BTDecoratorNode(int nodeId, BehaviourTree tree, BTNode parent) : base(nodeId, tree, parent) {
      
    }
    
    protected BTNode Child {
      get {
        if (_children.Count != 1) {
          Locator.Logger.LogError("Decorator Node with invalid number of children!");
        }
        return _children[0];
      }
    }
    
    protected override bool CanAddChild(BTNode child, ref string errorMessage) {
      if (_children.Count > 0) {
        errorMessage = "Decorator Node cannot have more than 1 child!";
        return false;
      }
      return true;
    }
    
    protected override BTNode SelectChildToProcess() {
      return this.Child;
    }
    
    protected override void HandleChildFinish(BTNode child) {
      if (child.IsRunning()) {
        Locator.Logger.LogError("HandleChildFinish - called when child is running");
        return;
      }
     
      this.ReturnStateBasedOnFinishedChild(child);
    }
    
    protected virtual void ReturnStateBasedOnFinishedChild(BTNode child) {
      if (child.State == BTNodeState.SUCCESS) {
        Succeed();
      } else if (child.State == BTNodeState.FAILURE) {
        Fail();
      } else {
        Locator.Logger.LogError("ReturnStateBasedOnFinishedChild - child state is not handled (BTDecoratorNode): " + child.State);
        return;
      }
    }
  }
}