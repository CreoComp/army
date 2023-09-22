using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    public static EnemyFactory instance;

    private const float _mapSize = 50;
    public List<GameObject> _enemyList = new List<GameObject>();
    public List<GameObject> EnemyList => _enemyList;

    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _bossPrefab;

    private int EnemyOnLevel;
    [SerializeField] private float _enemyHealth;
    private int _nowEnemySpawned;
    private const int _maxEnemyOnMap = 300;

    private GameStateMachine _gameStateMachine;

    private Transform _player;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if(instance == this)
            Destroy(gameObject);

    }

    public void StartLevel(GameStateMachine gameStateMachine, Transform player)
    {
        _player = player;
        _gameStateMachine = gameStateMachine;

        EnemyOnLevel = SaveLoadService.Instance.PlayerData.NowLevel * 30;
        InstantiateEnemyes(EnemyOnLevel);

    }
    public void StartBossLevel(GameStateMachine gameStateMachine, Transform player)
    {
        _player = player;
        _gameStateMachine = gameStateMachine;

       var boss = Instantiate(_bossPrefab, new Vector3(0,0,15f), Quaternion.identity);
        _enemyList.Add(boss);
        boss.GetComponent<Health>().Construct(_gameStateMachine);

        _nowEnemySpawned++;
    }

    public void InstantiateEnemyes(int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            var enemy = Instantiate(_enemyPrefab, RandomPosition(), Quaternion.identity, transform);
            _enemyList.Add(enemy);
            enemy.GetComponent<Health>().Construct(_gameStateMachine);

            _nowEnemySpawned++;
            if (_nowEnemySpawned > _maxEnemyOnMap)
                return;
        }
    }

    public void InstantiateEnemyesAroundObject(Transform obj, float radiusSpawn, int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            float randomX = Random.Range(obj.transform.position.x - radiusSpawn, obj.transform.position.x + radiusSpawn);
            float randomZ = Random.Range(obj.transform.position.z - radiusSpawn, obj.transform.position.z + radiusSpawn);
            Vector3 randomPositionAroundBoss = new Vector3(randomX, 0, randomZ);
            var enemy = Instantiate(_enemyPrefab, randomPositionAroundBoss, obj.transform.rotation);
            enemy.GetComponent<Health>().Construct(_gameStateMachine);
            _enemyList.Add(enemy);

        }
    }

    public void DestroyEnemy(GameObject enemy)
    {
        MoneyStorage.Instance.AddMoney(1);
        if(_nowEnemySpawned >= EnemyOnLevel)
        {
            _enemyList.Remove(enemy);
            Destroy(enemy);
        }
        else
        {
            ReloadEnemy(enemy);
        }
        CheckIsGameComplete();
    }

    private void CheckIsGameComplete()
    {
        if (_enemyList.Count <= 0)
        {
            _gameStateMachine.EnterIn<GameOverState>();
            GameOver.instance.Win();
        }
    }

    public void PlayerDie()
    {
        _gameStateMachine.EnterIn<GameOverState>();
        GameOver.instance.Defeat();
    }

    private void ReloadEnemy(GameObject enemy)
    {
        enemy.transform.position = RandomPosition();
        enemy.GetComponent<Health>().health = _enemyHealth;
        _nowEnemySpawned++;
    }

    private Vector3 RandomPosition()
    {
        Vector3 position = new Vector3(Random.Range(-_mapSize, _mapSize), 0, Random.Range(-_mapSize, _mapSize));
        if (Vector3.Distance(position, _player.position) <= 25)
            return RandomPosition();

        return position;
    }

    public GameObject NearestMob(GameObject player)
    {
        GameObject nearestObject;
        nearestObject = _enemyList.OrderBy(obj => Vector3.Distance(obj.transform.position, player.transform.position))
                                            .FirstOrDefault();

        return nearestObject;
    }
}