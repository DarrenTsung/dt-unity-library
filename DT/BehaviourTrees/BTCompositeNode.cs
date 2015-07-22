using System.Collections;
using System.Collections.Generic;

namespace DT.BehaviourTrees {
  /// <summary>
  /// Attributes of a Composite Node
  /// 1. 
  /// 2. Modifies the child's return value and passes to parent
  /// 
  /// Default implementation here just passes the child's return value to the parent
  /// </summary>
  public class BTCompositeNode : BTNode {
    public BTCompositeNode(int nodeId, BTNode parent) : base(nodeId, parent) {
      
    }
  }
}