using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;          // used for Except
using Vexe.Runtime.Types;     // used for Message

namespace DT.BehaviourTrees {
  public class BehaviourTree {
    protected Dictionary<string, object> _dataContext;
    protected List<BTNode> _activeNodes;

    protected BTNode root;

    public BehaviourTree() {
      _dataContext = new Dictionary<string, object>();
      _activeNodes = new List<BTNode>();
    }

    public void Tick() {
      foreach (BTNode node in _activeNodes) {
        node.Tick();
      }
    }
    
    public void NodeDidStart(BTNode node) {
      if (_activeNodes.Contains(node)) {
        Locator.Logger.LogError("NodeDidStart - started node was in active nodes!");
        return;
      }
      _activeNodes.Add(node);
    }
    
    public void NodeDidFinish(BTNode node) {
      if (!_activeNodes.Contains(node)) {
        Locator.Logger.LogError("NodeDidFinish - finished node was not in active nodes!");
        return;
      }
      _activeNodes.Remove(node);
    }
  }
}