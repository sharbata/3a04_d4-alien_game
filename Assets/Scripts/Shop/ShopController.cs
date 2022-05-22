using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShopState {Menu, Buying, Busy}



public class ShopController : MonoBehaviour
{
    [SerializeField] WalletUI walletUI;

    ShopState state;

    public static ShopController i {get; private set;}

    private void Awake(){
        i = this;
    }
    public IEnumerator StartTrading(Shop shop){
        int selectedChoice = 0;
        yield return DialogManager.Instance.ShowDialogText("Welcome to the shop!",waitForInput:false,
        choices:  new List<string>() {"Buy", "Quit"},
        onChoiceSelected: choiceIndex => selectedChoice=choiceIndex);
        if (selectedChoice == 0){
            //Buy
           walletUI.Show();
           yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
           walletUI.Close();
        }
        else if (selectedChoice == 1){
            // Quit
            yield break;
        }
    }

}
