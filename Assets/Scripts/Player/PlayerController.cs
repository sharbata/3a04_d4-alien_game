using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;
    public LayerMask solidObjectLayer;

    //public event Action 

    private bool isMoving;
    private Vector2 input;

    [SerializeField] Dialog dialog;

    // // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    public void HandleUpdate()
    {
        if(!isMoving){
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if(input.x != 0) input.y = 0;

            if(input != Vector2.zero){
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;
                
                if (IsWalkable(targetPos)){
                    StartCoroutine(Move(targetPos));
                }
                else{
                    StartCoroutine(Interact());
                }
        }
        }


    }

    IEnumerator Move(Vector3 targetPos){

        isMoving = true;

        while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon){
            transform.position = Vector3.MoveTowards(transform.position,targetPos,moveSpeed*Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;
    }

    private bool IsWalkable(Vector3 targetPos){
        if (Physics2D.OverlapCircle(targetPos, 0.1f, solidObjectLayer) != null){
            Debug.Log("Not Walkable");
            //StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
            return false;
        }
        return true;
    }

    IEnumerator Interact(){
        var collider = Physics2D.OverlapCircle(transform.position, 0.3f, solidObjectLayer);
        if (collider != null){
            Debug.Log("At shop");
            yield return collider.GetComponent<InteractionController>()?.Interact(transform);
        }       
    }
    /* private IEnumerator AtShop(){
        Debug.Log("At shop");
        yield return Shop.Trade();
    } */

    /* void onTriggerEnter(Collider other){
        if (other.transform.CompareTag("Shop")){
            Debug.Log("At shop");
            StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
        }
    } */

}
