using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;          // used for Except

namespace DT.BehaviourTrees {
  public class BehaviourTree {
    protected Dictionary<string, object> _dataContext;
    protected Dictionary<int, BTNode> _nodeMap;
    protected List<BTNode> _activeNodes;

    protected BTNode root;
    
    protected string _name;
    public string Name {
      get { return _name; }
    }

    public BehaviourTree(string name) {
      _dataContext = new Dictionary<string, object>();
      _nodeMap = new Dictionary<int, BTNode>();
      _activeNodes = new List<BTNode>();
      
      _name = name;
    }

    public void Tick() {
      foreach (BTNode node in _activeNodes) {
        node.Tick();
      }
    }
    
    public void NodeDidStart(BTNode node) {
      if (_activeNodes.Contains(node)) {
        Debug.LogError("NodeDidStart - started node was in active nodes!");
        return;
      }
      _activeNodes.Add(node);
    }
    
    public void NodeDidFinish(BTNode node) {
      if (!_activeNodes.Contains(node)) {
        Debug.LogError("NodeDidFinish - finished node was not in active nodes!");
        return;
      }
      _activeNodes.Remove(node);
    }
    
    public BTNode GetNode(int nodeId) {
      if (_nodeMap.ContainsKey(nodeId)) {
        return _nodeMap[nodeId];
      }
      return null;
    }
  }
}