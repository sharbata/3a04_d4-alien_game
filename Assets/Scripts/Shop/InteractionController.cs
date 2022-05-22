using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    Shop shop;

    private void Awake(){
        shop = GetComponent<Shop>();
    }

    public IEnumerator Interact(Transform initiator){
        if (shop != null){
            Debug.Log("Interacting with shop");
            yield return shop.Trade();
        }      
    }
}
