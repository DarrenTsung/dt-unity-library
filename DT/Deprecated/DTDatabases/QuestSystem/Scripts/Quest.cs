using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DT.QuestSystem {
  public class Quest {
    protected int _questId;
    public int QuestId {
      get { return _questId; }
      set { _questId = value; }
    }
    
    protected string _name;
    public string Name {
      get { return _name; }
      set { _name = value; }
    }
    
    public Quest() {
      _questId = -1;
      _name = "Default";
    }
    
    public Quest(int questId) {
      _questId = questId;
      _name = "Unamed Quest " + questId;
    }
  }
}