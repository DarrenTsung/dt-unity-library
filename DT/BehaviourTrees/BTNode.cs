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
    
    public BTNode(BTNode parent) {
      _parent = parent;
    }
    
    public bool IsRunning() {
      return _state == BTNodeState.RUNNING;
    }
    
    public virtual void Tick() {
    }
    
    public virtual List<BTNode> Children {
      get { 
        Debug.LogError("Children - not implemented!"); 
        return null;
      }
    }
    
    protected virtual BTNode SelectChildToProcess() {
      Debug.LogError("SelectChildToProcess - not implemented and/or overriden!");
      return null;
    }
    
    protected virtual void StartProcessingChild() {
      _child.HandleStart();
    }
    
    protected virtual void HandleStart() {
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