using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization;
using System;

/// <summary>
/// This is what handles the simple binary encryption.
/// </summary>
public class SerializationManager : MonoBehaviour
{
	public static bool Save(string saveName, object saveData)
	{
		BinaryFormatter formatter = GetBinaryFormatter();

		if (!Directory.Exists(Application.persistentDataPath + "/saves"))
		{
			Directory.CreateDirectory(Application.persistentDataPath + "/saves");
		}

		string path = Application.persistentDataPath + "/saves/" + saveName + ".save";

		FileStream file = File.Create(path);

		formatter.Serialize(file, saveData);

		file.Close();

		return true;
	}

	public static object Load(string path)
	{
		if (!File.Exists(path))
		{
			return null;
		}

		BinaryFormatter formatter = GetBinaryFormatter();

		FileStream file = File.Open(path, FileMode.Open);

		try
		{
			object save = formatter.Deserialize(file);
			file.Close();
			return save;
		}
		catch
		{
			Debug.LogErrorFormat("Failed to load file at {0}", path);
			file.Close();
			return null;
		}

	}

	public static BinaryFormatter GetBinaryFormatter()
	{
		BinaryFormatter formatter = new BinaryFormatter();

		SurrogateSelector selector = new SurrogateSelector();

		Vector3SerializationSurrogate vector3Surrogate = new Vector3SerializationSurrogate();
		QuaternionSerializationSurrogate quaternionSurrogate = new QuaternionSerializationSurrogate();

		selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);
		selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSurrogate);

		formatter.SurrogateSelector = selector;

		return formatter;
	}
}
