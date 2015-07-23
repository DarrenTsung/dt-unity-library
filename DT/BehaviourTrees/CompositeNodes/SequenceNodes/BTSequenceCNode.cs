using System.Collections;
using System.Collections.Generic;

namespace DT.BehaviourTrees {
  /// <summary>
  /// Attributes of a Sequence (Composite) Node
  /// 1. Has 1 or more children
  /// 2. Processes it's children from first to last and 
  ///    returns failure if [maxFailedNodes + 1] of the children fail
  /// </summary>
  public class BTSequenceCNode: BTCompositeNode {
    protected int _maxFailedNodes;
    protected int _failedNodes;
    
    public BTSequenceCNode(int nodeId, BehaviourTree tree, BTNode parent) : this(nodeId, tree, parent, 0) {}
    
    public BTSequenceCNode(int nodeId, BehaviourTree tree, BTNode parent, int maxFailedNodes) : base(nodeId, tree, parent) {
      _maxFailedNodes = maxFailedNodes;
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
    
    protected override BTNode SelectChildToProcess() {
      foreach (BTNode child in _children) {
        if (!_chosenNodes.Contains(child)) {
          _chosenNodes.Add(child);
          return child;
        }
      }
      return null;
    }
  }
}