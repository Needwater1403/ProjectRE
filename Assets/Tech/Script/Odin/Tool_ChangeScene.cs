using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
#if UNITY_EDITOR
public class Tool_ChangeScene : OdinEditorWindow
{
    
    [MenuItem("Tools/Change Scene Tool")]
    private static void OpenWindow()
    {
        GetWindow<Tool_ChangeScene>().Show();
    }

    private static void LoadScene(string path)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene(path);
    }
    
    [ButtonGroup]
    private void MainMenuScene()
    {
        LoadScene("Assets/Scenes/MainMenuScene.unity");
    }
    
    [ButtonGroup]
    private void PersistentData()
    {
        LoadScene("Assets/Scenes/PersistentData.unity");
    }
    
    [ButtonGroup]
    private void TutorArea()
    {
        LoadScene("Assets/Scenes/GaruMausoleum.unity");
    }
    
    [ButtonGroup]
    private void Graveyard()
    {
        LoadScene("Assets/Scenes/Graveyard.unity");
    }
    
    [ButtonGroup]
    private void TheForgottenShrine()
    {
        LoadScene("Assets/Scenes/TheForgottenShrine.unity");
    }
}
#endif
