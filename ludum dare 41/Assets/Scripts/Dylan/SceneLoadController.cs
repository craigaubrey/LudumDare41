using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadController : MonoBehaviour
{
    //Player character prefab
    GameObject mainPlayer;

    //On awake
    private void Awake()
    {
        //Get prefabs
        mainPlayer = Resources.Load<GameObject>("MainPlayer");

        //add scene load
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    //Spawn player on scene load
    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "DylanTest")
        {
            SpawnPlayer();
        }
    }

    //Add instance to network
    void SpawnPlayer()
    {
        PhotonNetwork.Instantiate(mainPlayer.name, mainPlayer.transform.position, mainPlayer.transform.rotation, 0);
    }
}
