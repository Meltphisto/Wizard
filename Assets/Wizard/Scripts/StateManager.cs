using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GameState
{
    MenuState,
    StartState,
    PlayerState,
    EnemyState,
    Transition,
    EndState
}
public class StateManager : MonoBehaviour
{
    [SerializeField] GameState gameState;

    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject gateVFX;
    [SerializeField] GameObject playerUI;
    [SerializeField] GameObject transition;
    [SerializeField] GameObject enemyBroadCast;
    [SerializeField] ShotSwitcher shotSwitcher;
    ElementManager elementManager;
    EnemyManager enemyManager;
 
    public GameState State { get { return gameState; } }

    private void Start()
    {
        elementManager = FindObjectOfType<ElementManager>();
        enemyManager = FindObjectOfType<EnemyManager>();
        shotSwitcher = FindObjectOfType<ShotSwitcher>();
        ChangeState(GameState.MenuState);
    }
    public void ChangeState(GameState state)
    {
        
        switch (state)
        {
            case GameState.StartState:
                StartCoroutine(HandleStartEvent());
                break;
            case GameState.Transition:
                StartCoroutine(PlayTransition(GameState.PlayerState,1.5f));
                break;
            case GameState.PlayerState:
                HandlePlayerState();
                break;
            case GameState.EnemyState:
                StartCoroutine(BroadCastEnemyAmount());
                break;
        }
        gameState = state;

    }
    public void StartGameFromMenu()
    {
        startMenu.SetActive(false);
        gateVFX.SetActive(true);
        ChangeState(GameState.StartState);

    }
    IEnumerator HandleStartEvent()
    {
        yield return new WaitForSeconds(2f);
        shotSwitcher.SwitchShot(1, 3f);
        yield return new WaitForSeconds(1f);
        StartCoroutine(enemyManager.SpawnAtTurnBegin());

        yield return new WaitForSeconds(3.5f);
        ChangeState(GameState.Transition);
    }

    IEnumerator PlayTransition(GameState state, float transitionDuration)
    {
        switch (state)
        {
            case GameState.StartState:
                break;
            case GameState.PlayerState:
                transition.GetComponent<TMP_Text>().text = "Player turn";
                break;
            case GameState.EnemyState:
                transition.GetComponent<TMP_Text>().text = "Enemy turn";
                break;
        }
        transition.SetActive(true);
        yield return new WaitForSeconds(transitionDuration);
        ChangeState(state);
        transition.SetActive(false);
    }

    void HandlePlayerState()
    {
        playerUI.SetActive(true);
        elementManager.GainElementAtPlayerTurn(3);
    }
    public void ChangeEnemyState()
    {
        playerUI.SetActive(false);
        StartCoroutine(PlayTransition(GameState.EnemyState, 1.5f));
    }

    IEnumerator BroadCastEnemyAmount()
    {
        enemyManager.SetEnemyMoveState();
        if (enemyManager.WillSpawnEnemy())
        {
            StartCoroutine(enemyManager.SpawnAtTurnBegin());
            yield return new WaitForSeconds(3f);

            enemyBroadCast.GetComponentInChildren<TMP_Text>().text = "New enemy joined the battle.";
            enemyBroadCast.SetActive(true);
        }
        else
        {
            if(enemyManager.EnemyLeft > 0)
            {
                enemyBroadCast.GetComponentInChildren<TMP_Text>().text = "No enemy spawned this turn.";
            }
            else if(enemyManager.willSpawn)
            {
                enemyBroadCast.GetComponentInChildren<TMP_Text>().text = "All enemies are on the battlefield.";
                enemyManager.willSpawn = false;
            }
            enemyBroadCast.SetActive(true);
        }
    }

    public void HandleEnemyMove()
    {
        enemyBroadCast.SetActive(false);
        StartCoroutine(HandleEnemyState());
    }

    IEnumerator HandleEnemyState()
    {
        StartCoroutine(enemyManager.MoveAllEnemy());
        yield return new WaitForSeconds(enemyManager.transform.childCount * 0.8f);
        StartCoroutine(PlayTransition(GameState.PlayerState, 1.5f));
    }

    


}
