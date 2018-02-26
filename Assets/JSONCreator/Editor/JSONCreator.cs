using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using System.IO;
using System.Text;

/// <summary>
/// Basic data structure that holds a key/value data and all its other details in the JSON.
/// </summary>
public struct JSONData
{
	public string key;
	public object value;
	public string parent;
	public DataTypes valueDataType;
	public float indent;
}

public class JSONCreator : EditorWindow
{
	/// <summary>
	/// The list that holds all the JSON data.
	/// </summary>
	public static List<JSONDataClass> jsonData = new List<JSONDataClass> ();

	string path, fileName;

	/// <summary>
	/// The final JSON string that goes into the JSON file.
	/// </summary>
	StringBuilder finalJSONString = new StringBuilder ();

	/// <summary>
	/// For any data that is not the child of an array or object data type, root class is assigned parent.
	/// </summary>
	public static JSONDataClass rootClass;

	void OnEnable ()
	{
		path = Path.Combine (Application.dataPath, "JSONCreator/JSONs/");
		fileName = "untitledJSON";
		rootClass = new JSONDataClass ();
	}

	[MenuItem ("Tools/Painkiller's Works/JSON Creator")]
	public static void DisplayWindow ()
	{
		EditorWindow.GetWindow (typeof(JSONCreator));
	}

	void OnGUI ()
	{
		GUILayout.Label ("JSON Creator", EditorStyles.boldLabel);
		GUILayout.Space (10f);

#region JSON Data Display

		///Display and addition of new JSON data with Root as parent is managed here.
		GUILayout.BeginHorizontal (GUILayout.Width (100f));
		GUILayout.Label ("Root", EditorStyles.label);
		if (GUILayout.Button ("+", GUILayout.Width (50f))) {

			///This logic takes care that each time a unique key string is added to the Root. JSON does not support 2 keys of the same name in the same hierarchy.
			///For example, a key named "Int" in Root and a key named "Int" as a child of an Object is valid, but 2 keys named "Int" in Root or as children of the same Object are invalid.
			bool isKeyValid = true;
			Dictionary<string, List<string>> distinctKeys = new Dictionary<string, List<string>> ();
			int count = 0;
			foreach (JSONDataClass j in jsonData) {
				if (!distinctKeys.ContainsKey (j.parent.key)) {
					List<string> tempKey = new List<string> ();
					tempKey.Add (j.key);
					distinctKeys.Add (j.parent.key, tempKey);
					count++;
				} else {
					if (!distinctKeys [j.parent.key].Contains (j.key)) {
						distinctKeys [j.parent.key].Add (j.key);
						count++;
					}
				}
			}
			distinctKeys.Clear ();

			for (int i = 0; i < jsonData.Count; i++) {
				
				if (string.IsNullOrEmpty (jsonData [i].key) || count < jsonData.Count) {
					isKeyValid = false;
					break;
				} else {
					isKeyValid = true;
				}
			}

			///If all your last entered keys are unique, then a drop-down for new entry is shown, otherwise an error message is displayed.
			if (isKeyValid) {
				JSONCreatorHelper.DropdownMenu ();
			} else {
				EditorUtility.DisplayDialog ("Invalid Key Name(s)", "Please don't leave key name empty and choose a unique key name.", "OK");
			}
		}
		if (GUILayout.Button ("Clear", GUILayout.Width (50f))) {
			if (EditorUtility.DisplayDialog ("Clear", "Are you sure you want to clear all data?", "Yes", "No")) {
				jsonData.Clear ();
			}
		}
		GUILayout.EndHorizontal ();
		DisplayData ();
#endregion

		GUILayout.Space (30f);

#region Compiling-Saving JSON
		GUILayout.BeginHorizontal (GUILayout.Width (200f));
		GUILayout.Label ("JSON File Name: ", EditorStyles.boldLabel);
		fileName = GUILayout.TextField (fileName, GUILayout.Width (150f));
		if (GUILayout.Button ("Save JSON", GUILayout.Width (150f))) {
			CompileJSONString ();
			if (File.Exists (Path.Combine (path, fileName + ".json"))) {
				if (EditorUtility.DisplayDialog ("JSON Exists", "A JSON file with the name same as \"" + fileName + "\" already exists. Would you like to overwrite?", "Yes", "No")) {
					File.Delete (Path.Combine (path, fileName + ".json"));
					File.WriteAllText (Path.Combine (path, fileName + ".json"), finalJSONString.ToString ());
					EditorUtility.DisplayDialog ("Saved", "JSON written at " + path, "OK");
				}
			} else {
				File.WriteAllText (Path.Combine (path, fileName + ".json"), finalJSONString.ToString ());
				EditorUtility.DisplayDialog ("Saved", "JSON written at " + path, "OK");
			}
		}
		GUILayout.EndHorizontal ();
#endregion
	}

	void DisplayData ()
	{
		try {
			for (int i = 0; i < jsonData.Count; i++) {
				if (jsonData [i].valueDataType != DataTypes.Array && jsonData [i].valueDataType != DataTypes.Object) {/// Where the data is not an Array/Object type, both, key and value are shown.
					
					GUILayout.BeginHorizontal ();
					GUILayout.Space (jsonData [i].indent);
					GUILayout.Label ("(" + jsonData [i].valueDataType.ToString () [0] + ")", GUILayout.Width (25f));
					jsonData [i].key = GUILayout.TextField (jsonData [i].key, GUILayout.Width (150f));
					GUILayout.Label (":", GUILayout.Width (10f));

					///If the value of a data is a Boolean, for consistency reasons, only a Toggle box is shown in the sheet.
					/// If this box is checked, the value is 'true', otherwise 'false'.
					if (jsonData [i].valueDataType != DataTypes.Bool) {
						jsonData [i].value = GUILayout.TextField (jsonData [i].value.ToString (), GUILayout.Width (150f));
					} else {
						jsonData [i].value = GUILayout.Toggle (bool.Parse (jsonData [i].value.ToString ()), bool.Parse (jsonData [i].value.ToString ()).ToString (), GUILayout.Width (50f)).ToString ();
					}

					///Data delete button.
					if (GUILayout.Button ("x", GUILayout.Width (25f))) {
						JSONCreatorHelper.DeleteData (i, jsonData [i].parent.key == "root");
						break;
					}
					GUILayout.EndHorizontal ();


				} else {/// Where the data is an Array/Object type, only the key is shown. The value is kept null.
					
					GUILayout.BeginHorizontal ();
					GUILayout.Space (jsonData [i].indent);
					jsonData [i].key = GUILayout.TextField (jsonData [i].key, GUILayout.Width (150f));

					///This logic takes care that each time a unique key string is added to the given Array/Object parent. JSON does not support 2 keys of the same name in the same hierarchy.
					///For example, a key named "Int" in Root and a key named "Int" as a child of an Object is valid, but 2 keys named "Int" in Root or as children of the same Object are invalid.
					if (GUILayout.Button ("+", GUILayout.Width (50f))) {
						bool isKeyValid = true;
						Dictionary<string, List<string>> distinctKeys = new Dictionary<string, List<string>> ();
						int count = 0;
						foreach (JSONDataClass j in jsonData) {
							if (!distinctKeys.ContainsKey (j.parent.key)) {
								List<string> tempKey = new List<string> ();
								tempKey.Add (j.key);
								distinctKeys.Add (j.parent.key, tempKey);
								count++;
							} else {
								if (!distinctKeys [j.parent.key].Contains (j.key)) {
									distinctKeys [j.parent.key].Add (j.key);
									count++;
								}
							}
						}
						distinctKeys.Clear ();

						for (int j = 0; j < jsonData.Count; j++) {

							if (string.IsNullOrEmpty (jsonData [j].key) || count < jsonData.Count) {
								isKeyValid = false;
								break;
							} else {
								isKeyValid = true;
							}
						}

						///If all your last entered keys withn the given subset are unique, then a drop-down for new entry is shown, otherwise an error message is displayed.
						if (isKeyValid) {
							JSONCreatorHelper.DropdownMenu (false, jsonData [i]);
						} else {
							EditorUtility.DisplayDialog ("Invalid Key Name(s)", "Please don't leave key name empty and choose a unique key name.", "OK");
						}
					}
					if (GUILayout.Button ("x", GUILayout.Width (25f))) {
						JSONCreatorHelper.DeleteData (i, jsonData [i].parent.key == "root");
						break;
					}
					GUILayout.EndHorizontal ();
			
				}
			}
		} catch (Exception e) {
			Debug.Log ("Whoops! Problem with reading data. ERROR: " + e);
		}
	}

	/// <summary>
	/// Iterates through each JSON data in the list and compiles the final JSON in the finalJSONString stringBuilder. This is then transferred to a text file.
	/// </summary>
	void CompileJSONString ()
	{
		///In case of arrays or objects, where there's a need to open and close extra brackets, the below 2 values are used to keep a check on the current subset(Array/Object).
		///As soon as the iterator reaches the last child of the current subset, a bracket is closed.
		int currentArrayVariableIndex = -1;
		int childCount = -1;

		///Cleaning any garbage already existing in the stringBuilder.
		finalJSONString.Remove (0, finalJSONString.Length);


		finalJSONString.Append ("{");

		for (int i = 0; i < jsonData.Count; i++) {
			finalJSONString.Append ("\n");
			finalJSONString.Append (AddSpaces (jsonData [i].indent));

			///Where the parent of a data is not an Array, key is appended into the data string.
			///Keys of the children of an array(which show in the sheet as 0,1,2...n) are never appended into the final JSON string.
			if (jsonData [i].parent.valueDataType != DataTypes.Array) {
				finalJSONString.Append ("\"");
				finalJSONString.Append (jsonData [i].key);
				finalJSONString.Append ("\"");
				finalJSONString.Append (" : ");
			}

			///If a data is type Array/Object, a suitable bracket ([ or {) is opened.
			if (jsonData [i].valueDataType == DataTypes.Array) {
				
				finalJSONString.Append ("[");
				currentArrayVariableIndex = i;

			} else if (jsonData [i].valueDataType == DataTypes.Object) {
				
				finalJSONString.Append ("{");
				currentArrayVariableIndex = i;

			} else {

				///If a data value is a boolean, the words 'true' or 'false' are appended.
				/// If a value is a string, it's appended with quotation marks.
				/// Otherwise the value is simply appended without any special case.
				if (jsonData [i].valueDataType == DataTypes.Bool) {
					if (bool.Parse (jsonData [i].value.ToString ())) {
						finalJSONString.Append ("true");
					} else {
						finalJSONString.Append ("false");
					}
				} else if (jsonData [i].valueDataType != DataTypes.String) {
					finalJSONString.Append (jsonData [i].value);
				} else {
					finalJSONString.Append ("\"");
					finalJSONString.Append (jsonData [i].value);
					finalJSONString.Append ("\"");
				}
			}

			///If the parent of a data is an Array or an Object, and as soon as the iterator reaches the last child of the subset, a suitable bracket ([ or {) is closed.
			if (jsonData [i].parent.valueDataType == DataTypes.Array || jsonData [i].parent.valueDataType == DataTypes.Object) {
				childCount = jsonData [i].parent.childCount;
				if (i == jsonData [i].parent.indexInData + childCount) {
					if (jsonData [i].parent.valueDataType == DataTypes.Array) {
						finalJSONString.Append ("\n" + AddSpaces (jsonData [i].parent.indent) + "]");
					} else {
						finalJSONString.Append ("\n" + AddSpaces (jsonData [i].parent.indent) + "}");
					}
				}
			}

			///A ',' after every data except if there has been a bracket opened.
			if (i != jsonData.Count - 1 && (finalJSONString [finalJSONString.Length - 1] != '[' && finalJSONString [finalJSONString.Length - 1] != '{')) {
				finalJSONString.Append (",");
			}
		}

		finalJSONString.Append ("\n}");
	}

	/// <summary>
	/// Adds tab based indentation to every data while compiling the final JSON string in the string builder.
	/// Indentations in every data go in the multiples of 20, so if a data is at the root, its indentation is 20; if a data is a child of a subset, its indentaion is 40.
	/// Accordingly the result of division between the indentation value and 20 is the count or number of tabs(\t) that will be added before every data in the JSON string.
	/// </summary>
	/// <returns>Indentation tabs(\t)</returns>
	/// <param name="indent">Indent float value of every data.</param>
	string AddSpaces (float indent)
	{
		int count = ((int)indent / 20);
		string spaces = "";
		for (int i = 0; i < count; i++) {
			spaces += "\t";
		}
		return spaces;
	}
}