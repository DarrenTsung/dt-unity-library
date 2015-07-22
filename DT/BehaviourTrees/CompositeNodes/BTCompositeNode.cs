using System.Collections;
using System.Collections.Generic;

namespace DT.BehaviourTrees {
  /// <summary>
  /// Attributes of a Composite Node
  /// 1. Has 1 or more children
  /// 2. Processes it's children in some order (depends on which type of Composite node it is) and 
  ///    returns failure if the number of nodes failing exceeds maxFailedNodes (depends on type)
  ///
  /// If no maxFailedNodes is specified, 0 will be used (any node failing will fail the Composite node)
  /// </summary>
  public abstract class BTCompositeNode : BTNode {
    public BTCompositeNode(int nodeId, BehaviourTree tree, BTNode parent) : this(nodeId, tree, parent, 0) {}
    
    public BTCompositeNode(int nodeId, BehaviourTree tree, BTNode parent, int maxFailedNodes) : base(nodeId, tree, parent) {
      _maxFailedNodes = maxFailedNodes;
    }
    
    protected List<BTNode> _chosenNodes;
    protected int _maxFailedNodes;
    protected int _failedNodes;
    
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