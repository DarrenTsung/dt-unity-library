using DT;
﻿using DT.EditableShapes;
using System.Collections;
﻿using UnityEngine;

public interface IEditablePolygonDelegate {
	void HandlePointsUpdated(PolygonPoint[] newPoints);
}
