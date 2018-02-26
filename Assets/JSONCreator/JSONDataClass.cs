using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// JSON data class.
/// It's a serializable class for the purpose of saving the sheet.
/// </summary>
[System.Serializable]
public class JSONDataClass
{
	public string key = "";
	public DataTypes valueDataType;
	public string value;
	public float indent = 0;
	public int indexInData = -1;
	public JSONDataClass parent;
	public int childCount;

	/// <summary>
	/// This variable is used for serialization to find the parent JSONDataClass instance in the jsonData list when loading and reassigning values of a saved sheet.
	/// </summary>
	[SerializeField] int parentIndexInData;

	/// <summary>
	/// Initializes a default instance of the <see cref="JSONDataClass"/> class, with key as 'root' and value null.
	/// </summary>
	public JSONDataClass ()
	{
		key = "root";
		valueDataType = DataTypes.Null;
		value = null;
		indent = 0;
		indexInData = -1;
		parent = null;
		childCount = -1;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="JSONDataClass"/> class for any new data.
	/// </summary>
	/// <param name="dataKey">Data key.</param>
	/// <param name="dataValueType">Data value type.</param>
	/// <param name="dataValue">Data value.</param>
	/// <param name="dataIndent">Data indent(multiples of 20).</param>
	/// <param name="dataParent">Data parent class.</param>
	public JSONDataClass (string dataKey, DataTypes dataValueType, string dataValue, float dataIndent, JSONDataClass dataParent)
	{
		key = dataKey;
		valueDataType = dataValueType;
		value = dataValue;
		indent = dataIndent;
		parent = dataParent;
	}

	/// <summary>
	/// Gets the index of the parent.
	/// </summary>
	/// <returns>The parent index.</returns>
	public int GetParentIndex ()
	{
		return parentIndexInData;
	}

	/// <summary>
	/// Sets the index of the parent.
	/// </summary>
	public void SetParentIndex ()
	{
		parentIndexInData = parent.indexInData;
	}
}
