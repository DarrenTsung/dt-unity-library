using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DT.BehaviourTrees {
  /// <summary>
  /// Attributes of a Random (Sequence) Node
  /// 1. Has 1 or more children
  /// 2. Processes it's children in a random order and 
  ///    returns failure if [maxFailedNodes + 1] of the children fail
  /// </summary>
  public class BTRandomSNode: BTSequenceCNode {
    public BTRandomSNode(int nodeId, BehaviourTree tree, BTNode parent) : base(nodeId, tree, parent, 0) {}
    
    public BTRandomSNode(int nodeId, BehaviourTree tree, BTNode parent, int maxFailedNodes) : base(nodeId, tree, parent, maxFailedNodes) {}
    
    protected override BTNode SelectChildToProcess() {
      List<BTNode> nonChosenNodes = _children.Except(_chosenNodes).ToList();
      if (nonChosenNodes.Count > 0) {
        return nonChosenNodes.PickRandom();
      } else {
        return null;
      }
    }
  }
}