using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCoin : MonoBehaviour
{
    private CoinManager CM;
    private void Awake()
    {
        CM = GameObject.FindGameObjectWithTag("GameController").GetComponent<CoinManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        SoundGuy.Instance.PlaySound("smb_coin");
        CM.AddCoins(1);
        Destroy(gameObject);
    }
}
