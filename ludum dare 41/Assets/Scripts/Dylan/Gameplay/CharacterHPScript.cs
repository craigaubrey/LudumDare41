using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHPScript : MonoBehaviour {

    public int HP;
    float restoreHPTimer = 0;
    [SerializeField]
    float restoreHpTime;

    private void Update()
    {
        ShowHealthStatus();
        DoRestoreHpCooldown();
    }

    void ShowHealthStatus()
    {
        if (HP == 1)
            GetComponent<SpriteRenderer>().color = Color.red;
        else if(GetComponent<PhotonView>().isMine)
            GetComponent<SpriteRenderer>().color = Color.white;
    }

    void DoRestoreHpCooldown()
    {
        if (restoreHPTimer > 0)
        {
            restoreHPTimer -= Time.deltaTime;
            if (restoreHPTimer <= 0)
                HP = 2;

        }
                
    }

    public void UpdateHealth(int hp)
    {
        if (GameObject.Find("SceneDataController_Obj").GetComponent<InterSceneController>().GetOnlineStatus() == true)
            GetComponent<PhotonView>().RPC("RPC_UpdateHP", PhotonTargets.All, hp);
        else
            OfflineUpdateHealth(hp);
    }

    void OfflineUpdateHealth(int hp)
    {
        HP = hp;

        //To start timer to set hp back to 2
        if (hp == 1)
        {
            restoreHPTimer = restoreHpTime;
        }
    }

    [PunRPC]
    void RPC_UpdateHP(int hp)
    {
        HP = hp;

        //To start timer to set hp back to 2
        if(hp == 1)
        {
            restoreHPTimer = restoreHpTime;
        }
    }
}
