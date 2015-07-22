using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DT.BehaviourTrees {
  public enum BTNodeState {
    SUCCESS,
    FAILURE,
    RUNNING
  };
  
  public class BTNode {
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
    
    public BTNode(int nodeId, BTNode parent) {
      _nodeId = nodeId;
      _parent = parent;
      _state = BTNodeState.SUCCESS;
    }
    
    public bool IsRunning() {
      return _state == BTNodeState.RUNNING;
    }
    
    public virtual void Tick() {
    }
    
    protected virtual bool CanAddChild(BTNode child, ref string errorMessage) {
      return true;
    }
    
    public virtual void AddChild(BTNode child, ref string errorMessage) {
      if (!this.CanAddChild(child, ref errorMessage)) {
        return;
      }
      
      _children.Add(child);
    }
    
    protected virtual BTNode SelectChildToProcess() {
      Locator.Logger.LogError("SelectChildToProcess - not implemented and/or overriden!");
      return null;
    }
    
    protected virtual void StartProcessingChild() {
      BTNode child = this.SelectChildToProcess();
      child.HandleStart();
    }
    
    protected virtual void HandleStart() {
      this.Tick();
    }
    
    protected virtual void HandleChildFinish(BTNode child) {
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
      _parent.HandleChildFinish(this);
    }
    
  }
}