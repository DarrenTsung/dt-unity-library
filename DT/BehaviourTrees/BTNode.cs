using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DT.BehaviourTrees {
  public enum BTNodeState {
    SUCCESS,
    FAILURE,
    RUNNING
  };
  
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
        Locator.Logger.LogError("BTNode::ProcessOneChild - selected child to process is null!");
      }
      child.HandleStart();
    }
    
    protected virtual void HandleStart() {
      _tree.NodeDidStart(this);
      this.Tick();
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