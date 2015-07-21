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

    public BTree() {
      _dataContext = new Dictionary<string, object>();
      _activeNodes = new List<BTNode>();
    }

    public void Tick() {
      foreach (BTNode node in _activeNodes) {
        node.Tick();
      }
      UpdateActiveNodes();
    }
    
    protected void UpdateActiveNodes() {
      List<BTNode> inactiveNodes = new List<BTNode>();
      List<BTNode> activeNodesToPossiblyAdd = new List<BTNode>();
      foreach (BTNode node in _activeNodes) {
        if (node.IsRunning()) {
          inactiveNodes.Add(node);
          
          // loop through all the children + parent of the node
          if (node.Parent.IsRunning()) {
            activeNodesToPossiblyAdd.Add(node.Parent);
          }
          foreach (BTNode child in node.Children) {
            if (child.IsRunning()) {
              activeNodesToPossiblyAdd.Add(child);
            }
          }
        }
      }
      
      foreach (BTNode node in inactiveNodes) {
        _activeNodes.Remove(node);
      }
      
      foreach (BTNode node in activeNodesToPossiblyAdd) {
        if (!_activeNodes.Contains(node)) {
          _activeNodes.Add(node);
        }
      }
    }
  }
}