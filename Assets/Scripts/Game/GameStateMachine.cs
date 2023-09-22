using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject _player;

    public GameObject PanelUpgradeProgress;
    public CinemachineVirtualCamera VirtualCamera;

    public int EnemyDamage = 1;
    private bool isPlay;

    private Dictionary<Type, IGameState> _states;
    public IGameState CurrentState { get; private set; }

    private void Awake()
    {
        RegisterStates();
        EnterIn<LoadingGameState>();

    }

    private void RegisterStates()
    {
        _states = new Dictionary<Type, IGameState>
        {
            [typeof(LoadingGameState)] = new LoadingGameState(this),
            [typeof(MenuGameState)] = new MenuGameState(this),
            [typeof(PlayGameState)] = new PlayGameState(this),
            [typeof(GameOverState)] = new GameOverState(this)

        };
    }

    public void EnterIn<Tstate>() where Tstate : IGameState
    {
        if (_states.TryGetValue(typeof(Tstate), out IGameState state))
        {
            Debug.Log($"{name} entered state {state.GetType()}");
            CurrentState?.Exit();
            CurrentState = state;
            CurrentState.Enter();
        };
    }

    private void Update()
    {
        if (CurrentState is IUpdate update)
            update.Update();
    }

    public void Play()
    {
        if(!isPlay)
        {
            isPlay = true;
            EnterIn<PlayGameState>();

        }
    }
}

public class LoadingGameState : IGameState
{
    private GameObject playerPrefab;

    private GameStateMachine _gameStateMachine;

    private GameObject player;

    public LoadingGameState(GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
    }
    public void Enter()
    {
        SaveLoadService.Instance.Load();

        LoadPlayer();
        LoadData();
        _gameStateMachine.EnterIn<MenuGameState>();
    }

    private void LoadPlayer()
    {
        playerPrefab = Resources.Load<GameObject>("Player/player");

        player = GameObject.Instantiate(playerPrefab);
        _gameStateMachine._player = player;
        player.GetComponent<PlayerMovement>().Construct(_gameStateMachine);
        player.GetComponent<Health>().Construct(_gameStateMachine);


        _gameStateMachine.VirtualCamera.Follow = player.transform;
        _gameStateMachine.VirtualCamera.LookAt = player.transform;
    }

    private void LoadData()
    {

        if (SaveLoadService.Instance.PlayerData.isUseLazer)
            player.AddComponent<Lazer>().Construct(_gameStateMachine);

        if (SaveLoadService.Instance.PlayerData.isUseFireBall)
            player.AddComponent<FireBall>().Construct(_gameStateMachine);

        _gameStateMachine.EnemyDamage = (SaveLoadService.Instance.PlayerData.NowLevel / 10) + 1;

    }

    public void Exit()
    {
        
    }
}
public class MenuGameState : IGameState
{
    private GameStateMachine _gameStateMachine;

    public MenuGameState(GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
    }
    public void Enter()
    {
        _gameStateMachine.MenuPanel.SetActive(true);
    }

    public void Exit()
    {
        _gameStateMachine.MenuPanel.GetComponent<Animator>().SetTrigger("close");

    }
}
public class PlayGameState : IGameState
{
    private GameStateMachine _gameStateMachine;

    public PlayGameState(GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
    }
    public void Enter()
    {

        if (SaveLoadService.Instance.PlayerData.NowLevel % 5 == 0)
            EnemyFactory.instance.StartBossLevel(_gameStateMachine, _gameStateMachine._player.transform);
        else
            EnemyFactory.instance.StartLevel(_gameStateMachine, _gameStateMachine._player.transform);

    }

    public void Exit()
    {

    }
}
public class GameOverState : IGameState
{
    private GameStateMachine _gameStateMachine;

    public GameOverState(GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
    }
    public void Enter()
    {
        //_gameStateMachine.GameOverPanel.SetActive(true);
    }

    public void Exit()
    {

    }
}
