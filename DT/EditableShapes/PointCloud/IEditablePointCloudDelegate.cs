using DT;
﻿using DT.EditableShapes;
using System.Collections;
﻿using UnityEngine;

public interface IEditablePointCloudDelegate<T> where T : PointCloudPoint {
	void HandlePointsUpdated(T[] newPoints);
}
