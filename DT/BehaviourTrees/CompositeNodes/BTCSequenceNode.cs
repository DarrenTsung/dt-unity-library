using System.Collections;
using System.Collections.Generic;

namespace DT.BehaviourTrees {
  /// <summary>
  /// Attributes of a Sequence (Composite) Node
  /// 1. Has 1 or more children
  /// 2. Processes it's children from first to last and 
  ///    returns failure if [maxFailedNodes + 1] of the children fail
  /// </summary>
  public class BTCSequenceNode: BTCompositeNode {
    public BTCSequenceNode(int nodeId, BehaviourTree tree, BTNode parent) : base(nodeId, tree, parent, 0) {}
    
    public BTCSequenceNode(int nodeId, BehaviourTree tree, BTNode parent, int maxFailedNodes) : base(nodeId, tree, parent, maxFailedNodes) {}
    
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