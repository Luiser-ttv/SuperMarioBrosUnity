using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public Sprite emptySprite;
    
    public bool multiCoinBrick = false;
    private bool _timerIsRunning = false;

    public GameObject brickBrokenPrefab;
    
    public GameObject spawn;
    private Transform _sprite;
    private bool _empty = false;

    private void Start()
    {
        _sprite = transform.GetChild(0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_empty || !other.CompareTag("Player")) return;
        
       
        
        if (spawn)
        {
            // Aparece el item
            Instantiate(spawn, transform.position, Quaternion.identity);
            if (multiCoinBrick)
            {
                if(!_timerIsRunning) StartCoroutine(MultiCoinTimer());
            }
            else
            {
                // Para las animaciones
                if (_sprite.GetComponent<Animator>()) _sprite.GetComponent<Animator>().enabled = false;
                _sprite.GetComponent<SpriteRenderer>().sprite = emptySprite;
                _empty = true; // No se mueve en futuros golpes
            }
        }
        else
        {
            if (other.GetComponent<PlayerMovementController>().MarioState == MarioState.Big)
            {
                SoundGuy.Instance.PlaySound("smb_breakblock");
                Instantiate(brickBrokenPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
        SoundGuy.Instance.PlaySound("smb_bump");
        StartCoroutine(BrickHitAnim());
    }

    private IEnumerator BrickHitAnim()
    {
        for (int i = 0; i < 16; i++)
        {
            _sprite.Translate(0,0.03125f, 0);
            yield return null; 
        }
        for (int i = 0; i < 16; i++)
        {
            _sprite.Translate(0,-0.03125f, 0);
            yield return null;
        }
    }
    
    private IEnumerator MultiCoinTimer()
    {
        _timerIsRunning = true;
        yield return new WaitForSeconds(5);
        multiCoinBrick = false; // Un bloque normal
        _timerIsRunning = false;
    }

}
