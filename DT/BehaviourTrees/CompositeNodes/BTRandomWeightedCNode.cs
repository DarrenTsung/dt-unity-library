using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DT.BehaviourTrees {
  /// <summary>
  /// Attributes of a RandomWeighted (Composite) Node
  /// 1. Has 1 or more children
  /// 2. Processes it's children in a random weighted order and 
  ///    returns failure if [maxFailedNodes + 1] of the children fail
  /// </summary>
  public class BTRandomWeightedCNode: BTCompositeNode {
    protected const int DEFAULT_CHILD_NODE_WEIGHT = 10;
    protected const int DEFAULT_MAX_WEIGHT = 1000;
    
    public BTRandomWeightedCNode(int nodeId, BehaviourTree tree, BTNode parent) : base(nodeId, tree, parent, 0) {}
    
    public BTRandomWeightedCNode(int nodeId, BehaviourTree tree, BTNode parent, int maxFailedNodes) : base(nodeId, tree, parent, maxFailedNodes) {
      _nodeWeightMap = new Dictionary<BTNode, int>();
    }
    
    protected Dictionary<BTNode, int> _nodeWeightMap;
    
    public override void AddChild(BTNode child, ref string errorMessage) {
      base.AddChild(child, ref errorMessage);
      
      _nodeWeightMap[child] = DEFAULT_CHILD_NODE_WEIGHT;
    }
    
    public virtual void ChangePercentLikelihood(BTNode child, float percent) {
      // TODO (darren): implement this
      // make sure to round the percent to second decimal point
    }
    
    protected override BTNode SelectChildToProcess() {
      List<BTNode> nonChosenNodes = _children.Except(_chosenNodes).ToList();
      if (nonChosenNodes.Count > 0) {
        // TODO (darren): implement this
        return null;
      } else {
        return null;
      }
    }
  }
}