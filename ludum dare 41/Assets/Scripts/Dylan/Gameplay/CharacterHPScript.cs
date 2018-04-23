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
        print(hp);
        GetComponent<PhotonView>().RPC("RPC_UpdateHP", PhotonTargets.All, hp);
    }

    [PunRPC]
    void RPC_UpdateHP(int hp)
    {
        print("rpc is " + hp);
        HP = hp;

        //To start timer to set hp back to 2
        if(hp == 1)
        {
            restoreHPTimer = restoreHpTime;
        }
    }
}
