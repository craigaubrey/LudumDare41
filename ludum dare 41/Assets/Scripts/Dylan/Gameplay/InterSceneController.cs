using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterSceneController : MonoBehaviour {
    [SerializeField]
    bool online;

    public void SetOnlineStatus(bool isOnline) { online = isOnline; }
    public bool GetOnlineStatus() {return online; }
}
