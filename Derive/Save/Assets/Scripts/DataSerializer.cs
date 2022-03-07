using System.IO;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("Data")]
public class DataSerializer : MonoBehaviour
{
    public string path;
    private Encoding encoding = Encoding.UTF8;
    //Encoding encoding = Encoding.GetEncoding("UTF8");

    void SetPath()
    {
        path = Application.persistentDataPath;
    }

    public void SaveData<T>(T data)
    {
        SetPath();
        
        StreamWriter streamWriter = new StreamWriter(Path.Combine(path, typeof(T).Name + ".xml"), false);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        
        // Coroutine pour ne pas permettre au joueur de spam la save et de write deux fois dans le strWriter
        xmlSerializer.Serialize(streamWriter, data);
        streamWriter.Close();
        Debug.Log("Saved " + typeof(T).Name);
    }

    public T LoadData<T>()
    {
        SetPath();
        string completePath = Path.Combine(path, typeof(T).Name + ".xml");
        
        if (File.Exists(completePath))
        {
            FileStream fileStream = new FileStream(completePath, FileMode.Open);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            T data = (T) xmlSerializer.Deserialize(fileStream);
            fileStream.Close();
            Debug.Log("Loaded " + typeof(T).Name);

            return data;
        }
        Debug.LogWarning("Path doesn't exist");
        
        return default;
    }
}
