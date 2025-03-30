using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager Instance;
    public PlayerManager player;
    
    [Title("World Scene Index")]
    public int worldSceneIndex = 1;
    public bool isLoadedPersistentData;
    
    private SaveWriteData saveWriteData;
    // [SerializeField] private bool saveGame;
    // [SerializeField] private bool loadGame;
    
    [Title("Current Character Data")] 
    public CharacterSlot currentSlot;
    public CharacterSaveData currentCharacterSaveData;
    private string fileName;

    [Title("Character Slots")] 
    //public List<CharacterSaveData> characterSaveDataList;
    public CharacterSaveData characterSlot1;
    public CharacterSaveData characterSlot2;
    public CharacterSaveData characterSlot3;
    public CharacterSaveData characterSlot4;
    public CharacterSaveData characterSlot5;
    
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
        LoadAllSaveFiles();
    }

    public string DecideCharacterFileNameBasedOnCharacterSlot(CharacterSlot characterSlot)
    {
        var temp = "";
        switch (characterSlot)
        {
            case CharacterSlot.CharacterSlot_01:
                temp = "CharacterSlot_01";
                break;
            case CharacterSlot.CharacterSlot_02:
                temp = "CharacterSlot_02";
                break;
            case CharacterSlot.CharacterSlot_03:
                temp = "CharacterSlot_03";
                break;
            case CharacterSlot.CharacterSlot_04:
                temp = "CharacterSlot_04";
                break;
            case CharacterSlot.CharacterSlot_05:
                temp = "CharacterSlot_05";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(characterSlot), characterSlot, null);
        }
        return temp;
    }

    public bool CreateNewGame()
    {
        // CHECK FOR EMPTY SAVE SLOT
        if (CheckForFreeSaveSlot())
        {
            // CREATE A NEW FILE WITH FILE NAME DEPEND ON WHICH SLOT WE ARE USING
            currentCharacterSaveData = new CharacterSaveData();
            StartGame();
            return true;
        }

        Debug.Log("NO SAVE SLOT REMAINING !");
        return false;
    }

    private void StartGame()
    {
        SaveGame(true);
        StartCoroutine(LoadWorldScene(currentCharacterSaveData.loadedSceneIndex, true));
    }
    
    [ButtonGroup()]
    [Button("Load Game")]
    public void LoadGame()
    {
        // LOAD AN EXISTED FILE WITH FILE NAME DEPEND ON WHICH SLOT WE ARE USING
        fileName = DecideCharacterFileNameBasedOnCharacterSlot(currentSlot);
        saveWriteData = new SaveWriteData(); 
        
        // GENERALLY WORKS ON MULTIPLE MACHINE TYPES (Application.persistentDataPath)
        saveWriteData.saveFileDirectoryPath = Application.persistentDataPath;
        saveWriteData.saveFileName = fileName;
        currentCharacterSaveData = saveWriteData.LoadSaveFile();
        StartCoroutine(LoadWorldScene(currentCharacterSaveData.loadedSceneIndex, false));
    }

    [ButtonGroup()]
    [Button("Save Game")]
    public void SaveGame(bool isNewGame = false)
    {
        // SAVE THE CURRENT FILE UNDER A FILE NAME DEPEND ON WHICH SLOT WE ARE USING
        fileName = DecideCharacterFileNameBasedOnCharacterSlot(currentSlot);
        saveWriteData = new SaveWriteData();
        
        // GENERALLY WORKS ON MULTIPLE MACHINE TYPES (Application.persistentDataPath)
        saveWriteData.saveFileDirectoryPath = Application.persistentDataPath;
        saveWriteData.saveFileName = fileName;
        
        // PASS CHARACTERS DATA FROM GAME TO FILE
        //player.SaveGameDataToCurrentCharacterData(ref currentCharacterSaveData, isNewGame);
        
        // WRITE DATA IN TO JSON
        saveWriteData.CreateNewCharacterSaveFile(currentCharacterSaveData);
    }
    
    // LOAD ALL SAVE FILES TO GAME WHEN START
    
    public void LoadAllSaveFiles()
    {
        //characterSaveDataList = new List<CharacterSaveData>();
        saveWriteData = new SaveWriteData();
        saveWriteData.saveFileDirectoryPath = Application.persistentDataPath;
        
        saveWriteData.saveFileName = DecideCharacterFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot_01);
        characterSlot1 = saveWriteData.LoadSaveFile();
        
        saveWriteData.saveFileName = DecideCharacterFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot_02);
        characterSlot2 = saveWriteData.LoadSaveFile();
        
        saveWriteData.saveFileName = DecideCharacterFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot_03);
        characterSlot3 = saveWriteData.LoadSaveFile();
        
        saveWriteData.saveFileName = DecideCharacterFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot_04);
        characterSlot4 = saveWriteData.LoadSaveFile();
        
        saveWriteData.saveFileName = DecideCharacterFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot_05);
        characterSlot5 = saveWriteData.LoadSaveFile();
    }

    // DELETE SAVE SLOT
    public void DeleteSaveSlot(CharacterSlot characterSlot)
    {
        // SAVE THE CURRENT FILE UNDER A FILE NAME DEPEND ON WHICH SLOT WE ARE USING
        fileName = DecideCharacterFileNameBasedOnCharacterSlot(characterSlot);
        saveWriteData = new SaveWriteData();
        
        // GENERALLY WORKS ON MULTIPLE MACHINE TYPES (Application.persistentDataPath)
        saveWriteData.saveFileDirectoryPath = Application.persistentDataPath;
        saveWriteData.saveFileName = fileName;
        saveWriteData.DeleteSaveFile();
    }
    private bool CheckForFreeSaveSlot()
    {
        saveWriteData = new SaveWriteData();
        saveWriteData.saveFileDirectoryPath = Application.persistentDataPath;

        #region Check Each Save Slot

        fileName = DecideCharacterFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot_01);
        saveWriteData.saveFileName = fileName;
        if (!saveWriteData.CheckToSeeIfFileExist())
        {
            currentSlot = CharacterSlot.CharacterSlot_01;
            Debug.Log(currentSlot);
            return true;
        }
        
        fileName = DecideCharacterFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot_02);
        saveWriteData.saveFileName = fileName;
        if (!saveWriteData.CheckToSeeIfFileExist())
        {
            currentSlot = CharacterSlot.CharacterSlot_02;
            Debug.Log(currentSlot);
            return true;
        }
        
        fileName = DecideCharacterFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot_03);
        saveWriteData.saveFileName = fileName;
        if (!saveWriteData.CheckToSeeIfFileExist())
        {
            currentSlot = CharacterSlot.CharacterSlot_03;
            Debug.Log(currentSlot);
            return true;
        }
        
        fileName = DecideCharacterFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot_04);
        saveWriteData.saveFileName = fileName;
        if (!saveWriteData.CheckToSeeIfFileExist())
        {
            currentSlot = CharacterSlot.CharacterSlot_04;
            Debug.Log(currentSlot);
            return true;
        }
        
        fileName = DecideCharacterFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot_05);
        saveWriteData.saveFileName = fileName;
        if (!saveWriteData.CheckToSeeIfFileExist())
        {
            currentSlot = CharacterSlot.CharacterSlot_05;
            Debug.Log(currentSlot);
            return true;
        }

        #endregion
        
        return false;
    }

    private List<AsyncOperation> sceneToLoad = new List<AsyncOperation>();
    private AsyncOperation persistentDataAsync;
    public IEnumerator LoadWorldScene(List<int> sceneIndex, bool isNewGame)
    {
        TransitionManager.Instance.SetLoadingScreen(true);
        TitleScreenManager.Instance.StopBGM();
        yield return new WaitForSeconds(3f);
        persistentDataAsync = SceneManager.LoadSceneAsync("PersistentData");
        while (!persistentDataAsync.isDone)
        {
            yield return null;
        }
        PlayerManager.Instance.SetFreezeStatus(true);
        foreach (var index in sceneIndex)
        {
            sceneToLoad.Add(SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive));
        }
        foreach (var t in sceneToLoad)
        {
            while (!t.isDone)
            {
                yield return null;
            }
        }
        TransitionManager.Instance.SetLoadingScreen(false, 0, !isNewGame);
        // SET ACTIVE SCENE TO GET ITS LIGHTING
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneIndex[0]));
    }

    public void LoadCharacterData()
    {
        //player.LoadGameDataFromCurrentCharacterData(ref currentCharacterSaveData);
    }
}
