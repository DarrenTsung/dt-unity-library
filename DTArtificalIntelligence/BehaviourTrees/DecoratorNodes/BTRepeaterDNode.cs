using System.Collections;
using System.Collections.Generic;

namespace DT.BehaviourTrees {
  /// <summary>
  /// Attributes of a Repeater (Decorator) Node
  /// 1. Only 1 child
  /// 2. Repeats the child a set number of times (or infinite), returning the value of the last run
  ///
  /// Default number of repeats is infinite (-1)
  /// </summary>
  public class BTRepeaterDNode : BTDecoratorNode {
    protected int _numberOfRepeats;
    
    public BTRepeaterDNode(int nodeId, BehaviourTree tree, BTNode parent) : this(nodeId, tree, parent, -1) {}
    
    public BTRepeaterDNode(int nodeId, BehaviourTree tree, BTNode parent, int numRepeats) : base(nodeId, tree, parent) {
      _numberOfRepeats = numRepeats;
    }
    
    protected override void ReturnStateBasedOnFinishedChild(BTNode child) {
      if (_numberOfRepeats == 0) {
        if (child.State == BTNodeState.SUCCESS) {
          Succeed();
        } else if (child.State == BTNodeState.FAILURE) {
          Fail();
        } else {
          // let parent handle invalid state
          base.ReturnStateBasedOnFinishedChild(child);
        }
      } else {
        this.ProcessOneChild();
        
        if (_numberOfRepeats > 0) {
          _numberOfRepeats--;
        }
      }
    }
  }
}