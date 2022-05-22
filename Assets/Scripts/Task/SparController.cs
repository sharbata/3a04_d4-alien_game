using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SparController : MonoBehaviour
{
    [SerializeField] private SpecimenBase specimenBase;
    
    [SerializeField] private bool opponentIsHuman = true;

    [SerializeField] private float baseChanceOfAttack = 0.1f;
    [SerializeField] private float chanceOfOpponentMisstep = 0.3f;
    
    [SerializeField] private float defendDamageReduction = 0.3f;
    [SerializeField] private float baseDamage = 15f;
    [SerializeField] private float vulnerableDamageMultiplier = 2.5f;
    [SerializeField] public float opponentDamageMultiplier = 1.05f;
    
    [SerializeField] private float minWaitTime = 1.0f;
    [SerializeField] private float maxWaitTime = 8.0f;
    [SerializeField] private float maxMessageDisplayTime = 2.0f;

    [SerializeField] private Button AttackButton;
    [SerializeField] private Button DefendButton;
    [SerializeField] private Button ExitButton;
    
    private ColorBlock defaultAttackColorBlock;
    private ColorBlock defaultDefendColorBlock;
    
    [SerializeField] private Slider PlayerHealthBar;
    [SerializeField] private Slider OpponentHealthBar;
    
    [SerializeField] private Text PlayerHPText;
    [SerializeField] private Text OpponentHPText;
    [SerializeField] private Text gameCommentaryText;
    
    private int playerHealth = 100;
    private int opponentHealth = 100;
    
    private bool playerIsAttacking;
    private bool playerIsDefending;
    private bool opponentIsVulnerable;
    private bool opponentIsAttacking;

    private Coroutine idleEventCoroutine;

    private int energyConsumed;
    private int experienceGained;
    private int moneyEarned;
    private int strengthGained;
    private int resilienceGained;

    private void Awake()
    {
        if (opponentIsHuman)
        {
            energyConsumed = 2;
            experienceGained = 1;
            moneyEarned = 0;
            strengthGained = 1;
            resilienceGained = 1;

        }
        else
        {
            energyConsumed = 4;
            experienceGained = 2;
            moneyEarned = 300;
            strengthGained = 1;
            resilienceGained = 1;
        }
    }

    public void StartGame()
    {
        this.gameObject.SetActive(true);
        
        playerHealth = 100;
        opponentHealth = 100;
        
        PlayerHealthBar.value = 100;
        OpponentHealthBar.value = 100;
        
        PlayerHPText.text = "100";
        OpponentHPText.text = "100";
        
        playerIsAttacking = false;
        playerIsDefending = false;
        opponentIsVulnerable = false;
        opponentIsAttacking = false;
        
        defaultAttackColorBlock = AttackButton.colors;
        defaultDefendColorBlock = DefendButton.colors;

        if (specimenBase.EnergyPoints < energyConsumed)
        {
            specimenBase.EnergyPoints = 0;
        }
        else
        {
            specimenBase.EnergyPoints -= energyConsumed;
        }
        
        StartCoroutine (RunGame(minWaitTime, maxWaitTime));
    }

    IEnumerator RunGame(float minWaitTime, float maxWaitTime)
    {
        PrintEventMessage(GetStartMessage());
        while (!IsGameOver())
        {
            float startTime = Time.time;
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            Debug.Log("Waiting for: " + waitTime);

            yield return WaitUnlessGameOver(waitTime);
            if (IsGameOver()) { break; }
            yield return AttackByOpponent();
        }
        StopCoroutine(idleEventCoroutine);

        if (opponentHealth <= 0)
        {
            PrintEventMessage(GetWinMessage());
            specimenBase.Experience += experienceGained;
            specimenBase.Strength += strengthGained;
            specimenBase.Resilience += resilienceGained;
            moneyEarned = 0;
        }
        if (playerHealth <= 0)
        {
            PrintEventMessage(GetLoseMessage());
        }

        ExitButton.interactable = true;
    }

    IEnumerator WaitUnlessGameOver(float waitTime)
    {
        float startTime = Time.time;
        while ((Time.time - startTime) < (waitTime))
        {
            if (IsGameOver())
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    
    private bool IsGameOver()
    {
        return playerHealth <= 0 || opponentHealth <= 0;
    }

    private IEnumerator AttackByOpponent()
    {
        if (playerIsAttacking || IsGameOver())
        {
            yield break;
        }
        
        if (Random.value <= chanceOfOpponentMisstep)
        {
            PrintEventMessage(GetOpponentVulnerableMessage());
            StartOpponentIdleMessageTimer();
            yield return MakeOpponentVulnerable(1.0f); 
        }
        else
        {
            opponentIsAttacking = true;
            bool alreadyDefending = playerIsDefending;
            float parryTime = 1.0f;
            HighlightDefendButton();
            PrintEventMessage(GetOpponentAttackingMessage());
            bool parried = false;
            bool blocked = false;
            bool canBlock = true;
            bool bothPlayersAttacked = false;
            float startTime = Time.time;
            Debug.Log("ATTACKING ==== ");
            while ((Time.time - startTime) < (parryTime))
            {
                if (playerIsDefending)
                {
                    parried = !alreadyDefending;
                    blocked = alreadyDefending && canBlock;
                    break;
                }
                if (playerIsAttacking)
                {
                    bothPlayersAttacked = true;
                    break;
                }
                
                // if statement reaches here that means player let go of defend button or never held it
                canBlock = false;
                yield return null;
            }

            if (blocked)
            {
                startTime = Time.time;
                float minimumAttackTime = 0.8f;
                while ((Time.time - startTime) < (minimumAttackTime))
                {
                    if (!playerIsDefending)
                    {
                        blocked = false;
                        break;
                    }

                    yield return null;
                }
                
            }
            ResetDefendButton();

            if (bothPlayersAttacked)
            {
                Debug.Log("== BOTH ATTACKED ==");
                PrintEventMessage(GetAttackAtSameTimeMessage());
                StartOpponentIdleMessageTimer();
                DamagePlayer(baseDamage * defendDamageReduction);
                DamageOpponent(baseDamage * defendDamageReduction);
                playerIsAttacking = false;
            }
            else if (blocked)
            {
                Debug.Log("== BLOCKED ==");
                PrintEventMessage(GetPlayerBlockedMessage());
                StartOpponentIdleMessageTimer();
                DamagePlayer(baseDamage * defendDamageReduction);
            }
            else if (parried)
            {
                Debug.Log("== PARRIED ==");
                PrintEventMessage(GetPlayerParriedMessage());
                StartOpponentIdleMessageTimer();
                opponentIsAttacking = false;
                yield return MakeOpponentVulnerable(1.5f);
            }
            else
            {
                Debug.Log("== PLAYER HURT ==");
                PrintEventMessage(GetPlayerHurtMessage());
                StartOpponentIdleMessageTimer();
                DamagePlayer(baseDamage);
            }
            opponentIsAttacking = false;
        }
    }

    private void HighlightDefendButton()
    {
        var vulnerableColorBlock = DefendButton.colors;
        vulnerableColorBlock.normalColor = Color.blue;
        vulnerableColorBlock.highlightedColor = Color.blue;
        DefendButton.colors = vulnerableColorBlock;
    }

    private void ResetDefendButton()
    {
        DefendButton.colors = defaultDefendColorBlock;
    }
    private void HighlightAttackButton()
    {
        var vulnerableColorBlock = AttackButton.colors;
        vulnerableColorBlock.normalColor = Color.red;
        vulnerableColorBlock.highlightedColor = Color.red;
        AttackButton.colors = vulnerableColorBlock;
    }

    private void ResetAttackButton()
    {
        AttackButton.colors = defaultAttackColorBlock;
    }

    private IEnumerator MakeOpponentVulnerable(float vulnerableTime)
    {
        opponentIsVulnerable = true;
        HighlightAttackButton();
        yield return WaitUnlessGameOver(vulnerableTime);
        opponentIsVulnerable = false;
        ResetAttackButton();
    }


    public void Attack()
    {
        if (IsGameOver())
        {
            return;
        }
        
        playerIsDefending = false;
        playerIsAttacking = true;

        if (opponentIsAttacking)
        {
            return;
        }
        
        if (opponentIsVulnerable)
        {
            PrintEventMessage(GetPlayerLandedHitMessage());
            DamageOpponent(baseDamage * vulnerableDamageMultiplier);
            opponentIsVulnerable = false;
            ResetAttackButton();
            StartOpponentIdleMessageTimer();
        }
        else if (Random.value <= baseChanceOfAttack)
        {
            PrintEventMessage(GetPlayerLandedHitMessage());
            DamageOpponent(baseDamage);
            StartOpponentIdleMessageTimer();
        }
        else
        {
            PrintEventMessage(GetPlayerMissedHitMessage());
            DamagePlayer(baseDamage * vulnerableDamageMultiplier);
            StartOpponentIdleMessageTimer();
        }
        
        playerIsAttacking = false;
    }

    private void DamagePlayer(float damageAmount)
    {
        damageAmount *= opponentDamageMultiplier;
        if (playerHealth - (int) damageAmount <= 0)
        {
            playerHealth = 0;
        }
        else
        {
            playerHealth -= (int) damageAmount;
        }
        
        PlayerHealthBar.value = playerHealth;
        PlayerHPText.text = playerHealth.ToString();
    }

    private void DamageOpponent(float damageAmount)
    {
        if (opponentHealth - (int) damageAmount <= 0)
        {
            opponentHealth = 0;
        }
        else
        {
            opponentHealth -= (int) damageAmount;
        }
        OpponentHealthBar.value = opponentHealth;
        OpponentHPText.text = opponentHealth.ToString();
    }

    public void StartDefending()
    {
        if (playerIsDefending || IsGameOver())
        {
            return;
        }
        
        playerIsAttacking = false;
        playerIsDefending = true;
    }
    
    public void StopDefending()
    {
        playerIsDefending = false;
    }

    private void PrintEventMessage(string message)
    {
        gameCommentaryText.text = FormatForDisplay(message);
    }

    private void StartOpponentIdleMessageTimer()
    {
        if (idleEventCoroutine != null)
        {
            StopCoroutine(idleEventCoroutine);
        }
        idleEventCoroutine = StartCoroutine(DelayedPrintIdleMessage());
    }
    
    
    private IEnumerator DelayedPrintIdleMessage()
    {
        yield return new WaitForSeconds(maxMessageDisplayTime);
        if (IsGameOver())
        {
            yield break;
        }
        gameCommentaryText.text = FormatForDisplay(GetOpponentIdleMessage());
    }

    private void ResetEventMessageBox()
    {
        gameCommentaryText.text = "";
    }
    private string FormatForDisplay(string message)
    {
        return message.ToUpper();
    }
    private string GetStartMessage()
    {
        string message = "";
        if (opponentIsHuman)
        {
            message = "Your opponent starts walking towards you while pointing their sword.";
        }
        else
        {
            message = "The beast slowly creeps towards you.";
        }
        return message;
    }private string GetOpponentIdleMessage()
    {
        string message = "";
        if (opponentIsHuman)
        {
            message = "Your opponent circles you slowly...";
        }
        else
        {
            message = "The beast takes a step back and lets out a deep roar.";
        }
        
        return message;
    }
    private string GetOpponentAttackingMessage()
    {
        string message = "";
        if (opponentIsHuman)
        {
            message = "Opponent is attacking you!";
        }
        else
        {
            message = "The beast lunges forward and swipes at you.";
        }
        
        return message;
    }

    private string GetOpponentVulnerableMessage()
    {
        string message = "";
        if (opponentIsHuman)
        {
            message = "Your opponent misstepped! Take advantage while they are invulnerable!";
        }
        else
        {
            message = "The beast is stunned. Finish him off quickly!";
        }
        return message;
    }

    private string GetPlayerHurtMessage()
    {
        string message = "Ouch! You've been hit!";
        return message;
    }
    private string GetPlayerBlockedMessage()
    {
        string message = "";
        if (opponentIsHuman)
        {
            message = "Thud! You blocked your opponent's blow!";
        }
        else
        {
            message = "Thud! You blocked the blow, but you are not unscathed.";
        }
        return message;
    }
    private string GetPlayerLandedHitMessage()
    {
        string message = "";
        if (opponentIsHuman)
        {
            message = "SLASH! You hit your opponent!";
        }
        else
        {
            message = "SLASH! You hit the beast where it hurts!";
        }
        return message;
    }
    private string GetPlayerMissedHitMessage()
    {
        string message = "";
        if (opponentIsHuman)
        {
            message = "You missed your swing! Your opponent counterattacked!";
        }
        else
        {
            message = "You missed your swing! The beast spared no time attacking back!";
        }
        return message;
    }
    private string GetPlayerParriedMessage()
    {
        string message = "";
        if (opponentIsHuman)
        {
            message = "Parried! Attack back quick to land a heavy blow!";
        }
        else
        {
            message = "Stunned! Attack back quick to land a heavy blow!";
        }
        
        return message;
    }

    private string GetWinMessage()
    {
        string message = "";
        if (opponentIsHuman)
        {
            message = "The crowd around you erupts in cheers, you've won!";;
        }
        else
        {
            message = "The beast lets out one last roar before falling limp. You've won.";
        }

        return message;
    }
    
    private string GetLoseMessage()
    {
        string message = "";
        if (opponentIsHuman)
        {
            message = "You raise your hands in surrender. Better luck next time.";
        }
        else
        {
            message = "Just as the world starts to spin you hear a hunter's caravan coming to the rescue.";
        }
        return message;
    }
    
    private string GetAttackAtSameTimeMessage()
    {
        string message = "CLASH! Both of you traded blows!";
        return message;
    }

}
