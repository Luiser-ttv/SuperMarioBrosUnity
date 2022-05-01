using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Vector3 _startPos;
    private CoinManager CM;
    

    private void Awake()
    {
        CM = GameObject.FindGameObjectWithTag("GameController").GetComponent<CoinManager>();
    }
    void Start()
    {
        _startPos = transform.position;
        SoundGuy.Instance.PlaySound("smb_coin");
        StartCoroutine(Run());
        transform.Translate(0,1,0); 
    }
    
    private IEnumerator Run()
    {
        for (int i = 0; i < 32; i++)
        {
            transform.Translate(0,Mathf.Cos(i/10f)/4, 0);
            yield return null;
        }

        CM.AddCoins(1);

        Destroy(gameObject);
    }
}
