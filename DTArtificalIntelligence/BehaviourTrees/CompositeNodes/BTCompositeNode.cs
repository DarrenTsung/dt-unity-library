using System.Collections;
using System.Collections.Generic;

namespace DT.BehaviourTrees {
  /// <summary>
  /// Attributes of a Composite Node
  /// 1. Has 1 or more children
  /// 2. Processes it's children in some order (depends on which type of Composite node it is) and 
  ///    returns success or failure
  /// </summary>
  public abstract class BTCompositeNode : BTNode {
    protected List<BTNode> _chosenNodes;
    
    public BTCompositeNode(int nodeId, BehaviourTree tree, BTNode parent) : base(nodeId, tree, parent) {}
  }
}