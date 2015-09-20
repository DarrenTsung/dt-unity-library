using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT.BehaviourTrees {
  /// <summary>
  /// Attributes of a Selector (Composite) Node
  /// 1. Has 1 or more children
  /// 2. Processes it's children from first to last and 
  ///    returns success if [minSucceededNodes + 1] of the children succeed (minSucceededNodes defaults to 0)
  /// </summary>
  public class BTSelectorCNode: BTCompositeNode {
    protected int _minSucceededNodes;
    protected int _succeededNodes;
    
    public BTSelectorCNode(int nodeId, BehaviourTree tree, BTNode parent) : this(nodeId, tree, parent, 0) {}
    
    public BTSelectorCNode(int nodeId, BehaviourTree tree, BTNode parent, int minSucceededNodes) : base(nodeId, tree, parent) {
      _minSucceededNodes = minSucceededNodes;
    }
    
    protected override void HandleChildFinish(BTNode child) {
      if (child.State == BTNodeState.SUCCESS) {
        _succeededNodes++;
      } else if (child.State != BTNodeState.FAILURE) {
        Debug.LogError("BTSequenceCNode::HandleChildFinish - invalid child state!");
      }
      
      if (_succeededNodes > _minSucceededNodes) {
        Succeed();
      } else {
        if (_chosenNodes.Count < _children.Count) {
          ProcessOneChild();
        } else {
          Fail();
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