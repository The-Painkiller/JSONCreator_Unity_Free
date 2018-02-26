using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class JSONCreatorHelper : EditorWindow
{

	/// <summary>
	/// Drop-down menu for new data entry with options for data types.
	/// If a new data is being added to the root, Array and Object type are shown as well, otherwise these 2 data types are hidden, as this tool currently doesn't support nested Arrays/Objects.
	/// </summary>
	/// <param name="isRoot">If set to <c>true</c> , parent will be root.</param>
	/// <param name="parentJSONData">Parent JSON data. Passed only if you're adding new data to an Array/Object.</param>
	public static void DropdownMenu (bool isRoot = true, JSONDataClass parentJSONData = null)
	{
		if (isRoot) {
			GenericMenu menu = new GenericMenu ();
			menu.AddItem (new GUIContent ("Int"), false, AddRootData, (object)DataTypes.Int);
			menu.AddItem (new GUIContent ("Float"), false, AddRootData, (object)DataTypes.Float);
			menu.AddItem (new GUIContent ("String"), false, AddRootData, (object)DataTypes.String);
			menu.AddItem (new GUIContent ("Bool"), false, AddRootData, (object)DataTypes.Bool);
			menu.AddItem (new GUIContent ("Array"), false, AddRootData, (object)DataTypes.Array);
			menu.AddItem (new GUIContent ("Object"), false, AddRootData, (object)DataTypes.Object);
			menu.ShowAsContext ();
		} else {
			GenericMenu menu = new GenericMenu ();

			menu.AddItem (new GUIContent ("Int"), false, AddArrayData, new List<object> () {
				(object)DataTypes.Int,
				parentJSONData
			});
			menu.AddItem (new GUIContent ("Float"), false, AddArrayData, new List<object> () {
				(object)DataTypes.Float,
				parentJSONData
			});
			menu.AddItem (new GUIContent ("String"), false, AddArrayData, new List<object> () {
				(object)DataTypes.String,
				parentJSONData
			});
			menu.AddItem (new GUIContent ("Bool"), false, AddArrayData, new List<object> () {
				(object)DataTypes.Bool,
				parentJSONData
			});

			menu.ShowAsContext ();
		}
	}

	/// <summary>
	/// Adds new data in root.
	/// JSONDataClass is instantiated, all values are set and finally added to the jsonData list belonging to the JSONCreator script.
	/// In this case, the static rootClass is assigned as parent.
	/// </summary>
	/// <param name="data">The data object passed as parameter here consists of the DataType of the data to be added.
	/// This is then parsed back to the enum value and saved in the new instantiated data class.</param>
	static void AddRootData (object data)
	{
		DataTypes dataType = (DataTypes)Enum.Parse (typeof(DataTypes), data.ToString ());
		JSONDataClass jsonData = new JSONDataClass ();
		switch (dataType) {
		case DataTypes.Array:
			jsonData.key = "";
			jsonData.value = null;
			jsonData.valueDataType = DataTypes.Array;
			jsonData.indent = 20;
			jsonData.parent = JSONCreator.rootClass;
			jsonData.childCount = 0;
			JSONCreator.jsonData.Add (jsonData);
			jsonData.indexInData = JSONCreator.jsonData.Count - 1;
			break;

		case DataTypes.Object:
			jsonData.key = "";
			jsonData.value = null;
			jsonData.valueDataType = DataTypes.Object;
			jsonData.indent = 20;
			jsonData.parent = JSONCreator.rootClass;
			jsonData.childCount = 0;
			JSONCreator.jsonData.Add (jsonData);
			jsonData.indexInData = JSONCreator.jsonData.Count - 1;
			break;

		case DataTypes.Bool:
			jsonData.key = "";
			jsonData.value = false.ToString ();
			jsonData.valueDataType = DataTypes.Bool;
			jsonData.indent = 20;
			jsonData.parent = JSONCreator.rootClass;
			JSONCreator.jsonData.Add (jsonData);
			jsonData.indexInData = JSONCreator.jsonData.Count - 1;
			break;

		case DataTypes.Float:
			jsonData.key = "";
			jsonData.value = 0f.ToString ();
			jsonData.valueDataType = DataTypes.Float;
			jsonData.indent = 20;
			jsonData.parent = JSONCreator.rootClass;
			JSONCreator.jsonData.Add (jsonData);
			jsonData.indexInData = JSONCreator.jsonData.Count - 1;
			break;

		case DataTypes.Int:
			jsonData.key = "";
			jsonData.value = 0.ToString ();
			jsonData.valueDataType = DataTypes.Int;
			jsonData.indent = 20;
			jsonData.parent = JSONCreator.rootClass;
			JSONCreator.jsonData.Add (jsonData);
			jsonData.indexInData = JSONCreator.jsonData.Count - 1;
			break;

		case DataTypes.String:
			jsonData.key = "";
			jsonData.value = "";
			jsonData.valueDataType = DataTypes.String;
			jsonData.indent = 20;
			jsonData.parent = JSONCreator.rootClass;
			JSONCreator.jsonData.Add (jsonData);
			jsonData.indexInData = JSONCreator.jsonData.Count - 1;
			break;
		}
	}

	/// <summary>
	/// Adds new data to an Array/Object.
	/// JSONDataClass is instantiated, all values are set and finally added to the jsonData list belonging to the JSONCreator script.
	/// In this case, the Array/Object data, from which the menu was triggered, is assigned as parent.
	/// </summary>
	/// <param name="data">The data object passed as parameter here consists of the DataType of the data to be added as well as reference to parent Array/Object data in the exact order.
	/// First value is then parsed back to the enum value and saved in the new instantiated data class.
	/// Second value is used to assign parent property to the new data as well as change child related properties of the parent data itself.</param>
	static void AddArrayData (object data)
	{
		List<object> tempData = (List<object>)data;

		DataTypes dataType = (DataTypes)Enum.Parse (typeof(DataTypes), tempData [0].ToString ());
		JSONDataClass parentData = (JSONDataClass)tempData [1];

		JSONDataClass jsonData = new JSONDataClass ();

		if (parentData.valueDataType == DataTypes.Array) {
			jsonData.key = (parentData.childCount).ToString ();
		} else {
			jsonData.key = "";
		}

		switch (dataType) {
		case DataTypes.Bool:
			jsonData.value = false.ToString ();
			jsonData.valueDataType = DataTypes.Bool;
			jsonData.indent = parentData.indent + 20f;
			jsonData.parent = parentData;
			break;

		case DataTypes.Float:
			jsonData.value = 0f.ToString ();
			jsonData.valueDataType = DataTypes.Float;
			jsonData.indent = parentData.indent + 20f;
			jsonData.parent = parentData;
			break;

		case DataTypes.Int:
			jsonData.value = 0.ToString ();
			jsonData.valueDataType = DataTypes.Int;
			jsonData.indent = parentData.indent + 20f;
			jsonData.parent = parentData;
			break;

		case DataTypes.String:
			jsonData.value = "";
			jsonData.valueDataType = DataTypes.String;
			jsonData.indent = parentData.indent + 20f;
			jsonData.parent = parentData;
			break;
		}

		///All the data are mainly a part of 1 common list, whether added to Root or to a subset.
		///So below functions are done as a check that whether the new child data of a subset is being added at the end of the jsonData list or added in the middle of the list somewhere.
		if ((parentData.indexInData == JSONCreator.jsonData.Count - 1) || JSONCreator.jsonData [JSONCreator.jsonData.Count - 1].parent == jsonData.parent) {
			jsonData.indexInData = JSONCreator.jsonData.Count - 1;
			JSONCreator.jsonData.Add (jsonData);
		} else {
			int parentIndex = JSONCreator.jsonData.IndexOf (parentData);
			if (parentData.childCount == 0) {
				jsonData.indexInData = parentIndex + 1;
				JSONCreator.jsonData.Insert (parentIndex + 1, jsonData);
			} else {
				jsonData.indexInData = parentIndex + 1 + parentData.childCount;
				JSONCreator.jsonData.Insert (parentIndex + 1 + parentData.childCount, jsonData);
			}
		}

		///Here the child count of the parent data is increased.
		JSONCreator.jsonData [parentData.indexInData].childCount++;

		///Each time a new data is added, specially if added to the middle of the jsonData list, 'indexInData' value of other data instances can get outdated. 
		///Therefore, after addition/deleting a data, a simple iteration is done to reassign new values to each data instance.
		for (int i = 0; i < JSONCreator.jsonData.Count; i++) {
			JSONCreator.jsonData [i].indexInData = i;
		}
	}

	/// <summary>
	/// Deletes the data instance.
	/// </summary>
	/// <param name="index">Index of the data being deleted in jsonData list is passed.</param>
	/// <param name="isRoot">If set to <c>true</c>, then the data being deleted was a part of the Root and not a child of a subset.</param>
	public static void DeleteData (int index, bool isRoot = true)
	{
		JSONDataClass removeableData = JSONCreator.jsonData [index];

		///if the data is of Array or Object, then first all its child data is removed.
		if (removeableData.valueDataType == DataTypes.Array || removeableData.valueDataType == DataTypes.Object) {
			if (removeableData.childCount > 0) {
				int count = removeableData.childCount;
				JSONCreator.jsonData.RemoveRange (index + 1, count);
			}

			if (!isRoot) {
				int parentIndex = removeableData.parent.indexInData;
				JSONCreator.jsonData [parentIndex].childCount--;
			}
		} else {
			///If the data is a child of some other data instance, then the child count of the parent is decremented.
			if (!isRoot) {
				int parentIndex = removeableData.parent.indexInData;
				JSONCreator.jsonData [parentIndex].childCount--;
			}
		}
		JSONCreator.jsonData.RemoveAt (index);

		///Each time a new data is added, specially if added to the middle of the jsonData list, 'indexInData' value of other data instances can get outdated. 
		///Therefore, after addition/deleting a data, a simple iteration is done to reassign new values to each data instance.
		for (int i = 0; i < JSONCreator.jsonData.Count; i++) {
			JSONCreator.jsonData [i].indexInData = i;
		}
	}
}
