// Credit: Lev-Lukomskyi (Unity Forums)

namespace CustomAttributes
{
	using UnityEngine;
	public class ShowOnlyAttribute : PropertyAttribute
	{
	}
}

namespace CustomDrawer
{
#if UNITY_EDITOR
	using UnityEditor;
	using UnityEngine;
	using CustomAttributes;
	using UnityEngine.UIElements;

	[CustomPropertyDrawer(typeof(ShowOnlyAttribute))]
	public class ShowOnlyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
		{
			string valueStr;

			switch (prop.propertyType)
			{
				case SerializedPropertyType.Integer:
					valueStr = prop.intValue.ToString();
					break;
				case SerializedPropertyType.Boolean:
					valueStr = prop.boolValue.ToString();
					break;
				case SerializedPropertyType.Float:
					valueStr = prop.floatValue.ToString("0.0");
					break;
				case SerializedPropertyType.String:
					valueStr = prop.stringValue;
					break;
				case SerializedPropertyType.Vector2:
					valueStr = prop.vector2Value.ToString();
					break;
				case SerializedPropertyType.Vector3:
					valueStr = prop.vector3Value.ToString();
					break;
				default:
					valueStr = "(not supported)";
					break;
			}

			EditorGUI.LabelField(position, label.text, valueStr);

		}
	}



#endif
}

