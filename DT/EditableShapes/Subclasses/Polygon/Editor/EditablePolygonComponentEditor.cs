using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEditor;
﻿using UnityEngine;

namespace DT.EditableShapes {
	[CustomEditor(typeof(EditablePolygonComponent))]
	public class EditablePolygonComponentEditor : EditablePointCloudComponentEditor<PolygonPoint, IEditablePolygonDelegate> {
	}
}