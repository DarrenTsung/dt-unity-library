using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEditor;
﻿using UnityEngine;

namespace DT.EditableShapes {
	[CustomEditor(typeof(EditableLineComponent), true)]
	public class EditableLineComponentEditor : EditablePointCloudComponentEditor<LinePoint, IEditableLineDelegate> {
	}
}