using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public IEnumerator Trade(){
        Debug.Log("Entering the shop");
        yield return ShopController.i.StartTrading(this);
    }
}
