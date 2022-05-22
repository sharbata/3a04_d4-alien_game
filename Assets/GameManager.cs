using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject selectedSkin;
    public GameObject Player;

    private Sprite playerSprite;

    // Start is called before the first frame update
    void Start()
    {
        playerSprite = selectedSkin.GetComponent<SpriteRenderer>().sprite;
        Player.GetComponent<SpriteRenderer>().sprite = playerSprite;
    }

}
