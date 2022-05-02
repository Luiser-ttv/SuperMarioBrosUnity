using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firework : MonoBehaviour
{
    private ScoreManager SM;

    private void Awake()
    {
        SM = GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreManager>();
       
    }


    public void Explode()
    {
        if (SM)
        {
            SM.AddPoints(500);
        }

        SoundGuy.Instance.PlaySound("", true);
        SoundGuy.Instance.PlaySound("smb_stage_clear",false);
    }


}
