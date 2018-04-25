using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetupSCript : MonoBehaviour {
    //Random level to load
    public int level;
    //Scene list, used for level picker
    List<string> sceneList;
    GameObject sceneDataObj;


    void Start()
    {
        //Get scenes from other script
        sceneList = GameObject.Find("NetworkObj").GetComponent<ServerHandler>().sceneList;
        sceneDataObj = GameObject.Find("SceneDataController_Obj");
    }

    public void LocalGameSetup_Click()
    {
        level = Random.Range(1, sceneList.Count + 1);
        sceneDataObj.GetComponent<InterSceneController>().SetOnlineStatus(false);
        SceneManager.LoadScene(level);
    }
}
