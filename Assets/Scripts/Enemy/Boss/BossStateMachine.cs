using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class BossStateMachine : MonoBehaviour
{
    private NavMeshAgent _agent;
    private GameObject _player;
    private Animator _animator;

    private const float _timeToCast = 10f;
    private float time;

    private int lastHitIndex; //0 - spell 1 - summon

    [SerializeField] private float _damageElectricBall = 1;
    [SerializeField] private float _damagePunch = 1;
    [SerializeField] private int _enemyesToSpawnInSummon = 10;


    private Dictionary<Type, IBossState> _states;
    public IBossState CurrentState { get; private set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _player = FindObjectOfType<PlayerMovement>().gameObject;
        RegisterStates();
    }
    private void Start()
    {
        EnterIn<BossMoveState>();
    }

    private void RegisterStates()
    {
        _states = new Dictionary<Type, IBossState>
        {
            [typeof(BossMoveState)] = new BossMoveState(this, _agent, _player, _animator),
            [typeof(BossShotElectricBallSpell)] = new BossShotElectricBallSpell(this, _animator, _player, _damageElectricBall),
            [typeof(BossPunchState)] = new BossPunchState(this, _animator, _player, _damagePunch),
            [typeof(BossSummonEnemyState)] = new BossSummonEnemyState(this, _animator, _enemyesToSpawnInSummon)

        };
    }

    public void EnterIn<Tstate>() where Tstate : IBossState
    {
        if(_states.TryGetValue(typeof(Tstate), out IBossState state))
        {
           // Debug.Log($"{name} entered state {state.GetType()}");

            CurrentState?.Exit();
            CurrentState = state;
            CurrentState.Enter();
        };
    }

    private void Update()
    {
        if(CurrentState is IUpdate update) 
            update.Update();

        Timer();
    }

    private void Timer()
    {
        time += Time.deltaTime;

        if (time >= _timeToCast)
        {
            time = 0;
            if (lastHitIndex == 0)
            {
                EnterIn<BossSummonEnemyState>();
                lastHitIndex = 1;
            }
            else if (lastHitIndex == 1)
            {
                EnterIn<BossShotElectricBallSpell>();
                lastHitIndex = 0;
            }
        }
    }

    public void AnimationTriggerDo()
    {
        if (CurrentState is IAnimationTriggerDo animationTriggerDo)
            animationTriggerDo.TriggerDo();
    }

    public void Debugger(string text) => Debug.Log(text);

}

public class BossMoveState : IBossState, IUpdate
{
    private BossStateMachine _bossStateMachine;
    private NavMeshAgent _agent;
    private GameObject _player;
    private Animator _animator;

    public BossMoveState(BossStateMachine bossStateMachine, NavMeshAgent agent, GameObject player, Animator animator)
    {
        _bossStateMachine = bossStateMachine;
        _agent = agent;
        _player = player;
        _animator = animator;
    }

    public void Enter()
    {
        _agent.SetDestination(_player.transform.position);

        _animator.SetBool("MoveState", true);
        _agent.isStopped = false;
    }

    public void Exit()
    {
        _agent.isStopped = true;

        _animator.SetBool("MoveState", false);

    }

    public void Update()
    {
        _agent.SetDestination(_player.transform.position);

        if (Vector3.Distance(_player.transform.position, _bossStateMachine.transform.position) < 3)
            _bossStateMachine.EnterIn<BossPunchState>();
    }
}

public class BossShotElectricBallSpell : IBossState, IAnimationTriggerDo, IUpdate
{
    private BossStateMachine _bossStateMachine;
    private Animator _animator;
    private GameObject _electricBallSpherePrefab;
    private GameObject _player;

    private const float _force = 60;
    private float _damage;


    public BossShotElectricBallSpell(BossStateMachine bossStateMachine, Animator animator, GameObject player, float damage)
    {
        _bossStateMachine = bossStateMachine;
        _animator = animator;
        _player = player;
        _damage = damage;


        _electricBallSpherePrefab = Resources.Load<GameObject>("FireBall/ElectricBall");
    }

    public void Enter()
    {
        _animator.SetTrigger("CastSpell");
    }

    public void Exit()
    {

    }
    private void InstantiateElectricBall()
    {
        var electricBallObject = GameObject.Instantiate(_electricBallSpherePrefab, _bossStateMachine.transform.position, Quaternion.identity);

        Vector3 vectorToMove = new Vector3(_player.transform.position.x - _bossStateMachine.transform.position.x, 1, _player.transform.position.z - _bossStateMachine.transform.position.z);

        electricBallObject.GetComponent<Rigidbody>().AddForce(vectorToMove.normalized * _force, ForceMode.Impulse);
        electricBallObject.AddComponent<FireBallWeapon>().Construct(_damage);
        GameObject.Destroy(electricBallObject, 5f);
    }

    public void TriggerDo()
    {
        InstantiateElectricBall();
        _bossStateMachine.EnterIn<BossMoveState>();

    }

    public void Update()
    {
        _bossStateMachine.transform.LookAt(_player.transform);
    }
}

public class BossPunchState : IBossState, IAnimationTriggerDo
{
    private BossStateMachine _bossStateMachine;
    private Animator _animator;
    private GameObject _player;
    private float _damage;

    public BossPunchState(BossStateMachine bossStateMachine, Animator animator, GameObject player, float damage)
    {
        _bossStateMachine = bossStateMachine;
        _animator = animator;
        _player = player;
        _damage = damage;
    }

    public void Enter()
    {
        if (UnityEngine.Random.Range(0, 100) > 50)
            _animator.SetInteger("PunchIndex", 0);
        else
            _animator.SetInteger("PunchIndex", 1);

        _animator.SetTrigger("Punch");
    }

    public void Exit()
    {

    }

    public void TriggerDo()
    {
        Collider[] hitColliders = Physics.OverlapSphere(_bossStateMachine.transform.forward + _bossStateMachine.transform.position, 3f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == _player)
                _player.GetComponent<Health>().GetDamage(_damage);
        }
        _bossStateMachine.EnterIn<BossMoveState>();
    }
}

public class BossSummonEnemyState : IBossState, IAnimationTriggerDo
{
    private BossStateMachine _bossStateMachine;
    private Animator _animator;
    private GameObject _enemyPrefab;

    private int _enemyesToSpawnInSummon;
    private const float radiusSpawn = 4f;

    public BossSummonEnemyState(BossStateMachine bossStateMachine, Animator animator, int enemyesToSpawnInSummon)
    {
        _bossStateMachine = bossStateMachine;
        _animator = animator;
        _enemyesToSpawnInSummon = enemyesToSpawnInSummon;

        _enemyPrefab = Resources.Load<GameObject>("Enemy/Enemy");
    }

    public void Enter()
    {
        _animator.SetTrigger("SummonEnemy");
    }

    public void Exit()
    {

    }

    private void Summon()
    {
        EnemyFactory.instance.InstantiateEnemyesAroundObject(_bossStateMachine.transform, radiusSpawn, _enemyesToSpawnInSummon);
        _bossStateMachine.EnterIn<BossMoveState>();
    }

    public void TriggerDo()
    {
        Summon();
    }
}
