using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
public class FileDataHandler<T>
{
    private string DataDirPath = "";
    private string DataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        DataDirPath = dataDirPath;
        DataFileName = dataFileName;
    }

    public T Load()
    {
        string fullPath = Path.Combine(DataDirPath, DataFileName);
        T loadedData = default;
        if(File.Exists(fullPath))
        {
            
            try
            {
                string dataToLoad = "";
                
                using (FileStream fs = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(fs))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                
                loadedData = JsonUtility.FromJson<T>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.LogError("Error loading file: " + e.Message);
            }
        }
        return loadedData;
    }

    public void Save(T data)
    {
        string fullPath = Path.Combine(DataDirPath, DataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream fs = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(dataToStore);
                }
            }
        }catch(Exception e)
        {
            Debug.LogError("Error saving file: " + e.Message);
        }
    }
}
