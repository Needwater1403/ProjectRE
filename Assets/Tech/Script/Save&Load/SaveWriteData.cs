using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class SaveWriteData 
{
    public string saveFileDirectoryPath = "";
    public string saveFileName = "";
    private readonly string encryptionCode = "anorlondo";
    
    // CHECK IF SAVE FILE ALREADY EXIST
    public bool CheckToSeeIfFileExist()
    {
        return File.Exists(Path.Combine(saveFileDirectoryPath, saveFileName));
    }

    // DELETE EXISTED SAVE FILE
    public void DeleteSaveFile()
    {
        File.Delete(Path.Combine(saveFileDirectoryPath, saveFileName));
    }
    
    // CREATING NEW SAVE FILE
    public void CreateNewCharacterSaveFile(CharacterSaveData characterSaveData)
    {
        var savePath = Path.Combine(saveFileDirectoryPath, saveFileName);

        try
        {
            // CREATE DIRECTORY
            Directory.CreateDirectory(Path.GetDirectoryName(savePath) ?? string.Empty);
            Debug.Log("CREATED NEW FILE SAVE AT: " + savePath);
            
            // SERIALIZE C# -> JSON
            var dataToStore = JsonUtility.ToJson(characterSaveData, true);
            
            // ENCRYPTION
            dataToStore = EncryptDecrypt(dataToStore);
            
            // WRITE FILE TO SYSTEM
            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("ERROR WHILST TRYING TO SAVE CHARACTER DATA AT: " + savePath + "\n" + e);
        }
    }
    
    // LOAD A SAVE FILE
    public CharacterSaveData LoadSaveFile()
    {
        CharacterSaveData characterData = null;
        var loadPath = Path.Combine(saveFileDirectoryPath, saveFileName);
        
        if (File.Exists(loadPath))
        {
            try
            {
                string dataToLoad = "";
                using (var stream = new FileStream(loadPath, FileMode.Open))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                
                // DECRYPTION
                dataToLoad = EncryptDecrypt(dataToLoad);
                
                // DESERIALIZE JSON -> C#
                characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.Log("ERROR WHILST TRYING TO LOAD CHARACTER DATA AT: " + loadPath + "\n" + e);
            }
        }
        return characterData;
    }
    
    // ENCRYPTION
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (var i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCode[i % encryptionCode.Length]);
        }

        return modifiedData;
    }
}
