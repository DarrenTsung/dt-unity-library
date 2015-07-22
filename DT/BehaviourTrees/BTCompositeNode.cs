using System.Collections;
using System.Collections.Generic;

namespace DT.BehaviourTrees {
  /// <summary>
  /// Attributes of a Composite Node
  /// 1. Has 1 or more children
  /// 2. Processes it's children in some order (depends on which type of Composite node it is) and 
  ///    returns failure if the number of nodes failing exceeds maxFailedNodes (depends on type)
  /// </summary>
  public class BTCompositeNode : BTNode {
    public BTCompositeNode(int nodeId, BehaviourTree tree, BTNode parent) : base(nodeId, tree, parent) {
      _maxFailedNodes = 0;
    }
    
    protected List<BTNode> _chosenNodes;
    protected int _maxFailedNodes;
    protected int _failedNodes;
    
    protected override BTNode SelectChildToProcess() {
      foreach (BTNode child in _children) {
        if (!_chosenNodes.Contains(child)) {
          _chosenNodes.Add(child);
          return child;
        }
      }
      return null;
    }
    
    protected override void HandleChildFinish(BTNode child) {
      if (child.State == BTNodeState.FAILURE) {
        _failedNodes++;
      } else if (child.State != BTNodeState.SUCCESS) {
        Locator.Logger.LogError("BTCompositeNode::HandleChildFinish - invalid child state!");
      }
      
      if (_failedNodes > _maxFailedNodes) {
        Fail();
      } else {
        if (_chosenNodes.Count < _children.Count) {
          ProcessOneChild();
        } else {
          Succeed();
        }
      }
    }
  }
}