using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DT.BehaviourTrees {
  public enum BTNodeState {
    SUCCESS,
    FAILURE,
    RUNNING
  };
  
  /// <summary>
  /// Base node class for all nodes in the Behaviour Tree
  ///
  /// Note: nodes can propagate return states up the tree in a single tick
  ///       but cannot start child nodes which start their children in a single tick
  /// </summary>
  public abstract class BTNode {
    protected int _nodeId;
    public int NodeId {
      get { return _nodeId; }
    }
    
    protected BTNode _parent;
    public BTNode Parent {
      get { return _parent;}
    }
    
    protected BTNodeState _state;
    public BTNodeState State {
      get { return _state; }
    }
    
    protected List<BTNode> _children;
    public List<BTNode> Children {
      get { return _children; }
    }
    
    protected BehaviourTree _tree;
    
    public BTNode(int nodeId, BehaviourTree tree, BTNode parent) {
      _nodeId = nodeId;
      _tree = tree;
      _parent = parent;
      _state = BTNodeState.SUCCESS;
    }
    
    public bool IsRunning() {
      return _state == BTNodeState.RUNNING;
    }
    
    public virtual void Tick() {
    }
    
    public virtual void AddChild(BTNode child, ref string errorMessage) {
      if (!this.CanAddChild(child, ref errorMessage)) {
        return;
      }
      
      _children.Add(child);
    }
    
    protected abstract BTNode SelectChildToProcess();
    
    protected abstract void HandleChildFinish(BTNode child);
    
    protected virtual bool CanAddChild(BTNode child, ref string errorMessage) {
      return true;
    }
    
    protected virtual void ProcessOneChild() {
      BTNode child = this.SelectChildToProcess();
      if (child == null) {
        Debug.LogError("BTNode::ProcessOneChild - selected child to process is null!");
      }
      child.HandleStart();
    }
    
    protected virtual void HandleStart() {
      _tree.NodeDidStart(this);
    }
    
    protected virtual void Succeed() {
      _state = BTNodeState.SUCCESS;
      Finish();
    }
    
    protected virtual void Fail() {
      _state = BTNodeState.FAILURE;
      Finish();
    }
    
    protected virtual void Finish() {
      _tree.NodeDidFinish(this);
      _parent.HandleChildFinish(this);
    }
    
  }
}