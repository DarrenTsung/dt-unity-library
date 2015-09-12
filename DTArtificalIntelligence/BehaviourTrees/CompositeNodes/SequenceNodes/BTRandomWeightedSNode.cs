using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DT.BehaviourTrees {
  /// <summary>
  /// Attributes of a RandomWeighted (Sequence) Node
  /// 1. Has 1 or more children
  /// 2. Processes it's children in a random weighted order and 
  ///    returns failure if [maxFailedNodes + 1] of the children fail
  /// </summary>
  public class BTRandomWeightedSNode: BTSequenceCNode {
    protected const int DEFAULT_CHILD_NODE_WEIGHT = 100;
    protected const int MAX_WEIGHT = 10000;
    
    protected Dictionary<int, int> _nodeWeightMap;
    
    public BTRandomWeightedSNode(int nodeId, BehaviourTree tree, BTNode parent) : base(nodeId, tree, parent, 0) {}
    
    public BTRandomWeightedSNode(int nodeId, BehaviourTree tree, BTNode parent, int maxFailedNodes) : base(nodeId, tree, parent, maxFailedNodes) {
      _nodeWeightMap = new Dictionary<int, int>();
    }
    
    public override void AddChild(BTNode child, ref string errorMessage) {
      base.AddChild(child, ref errorMessage);
      
      _nodeWeightMap[child.NodeId] = DEFAULT_CHILD_NODE_WEIGHT;
    }
    
    public float PercentLikelihood(int nodeId) {
      int sum = 0;
      foreach (KeyValuePair<int, int> pair in _nodeWeightMap) {
        sum += pair.Value;
      }
      
      return _nodeWeightMap[nodeId] / sum;
    }
    
    public int Weight(int nodeId) {
      if (!_nodeWeightMap.ContainsKey(nodeId)) {
        Locator.Logger.LogError("BTRandomWeightedSNode::Weight - querying about a nodeId that doesn't exist in the weight map!");
        return -1;
      }
      return _nodeWeightMap[nodeId];
    }
    
    public void SetWeight(int nodeId, int newWeight) {
      if (_tree.GetNode(nodeId) == null) {
        Locator.Logger.LogError("BTRandomWeightedSNode::SetWeight - invalid node id passed in! Not registered with tree.");
        return;
      }
      _nodeWeightMap[nodeId] = newWeight;
    }
    
    protected override BTNode SelectChildToProcess() {
      List<BTNode> nonChosenNodes = _children.Except(_chosenNodes).ToList();
      if (nonChosenNodes.Count > 0) {
        int chosenNodeId = _nodeWeightMap.PickRandomWeighted<int>();
        return _tree.GetNode(chosenNodeId);
      } else {
        return null;
      }
    }
  }
}