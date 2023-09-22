using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private float _timeToShoot = 3;
    [SerializeField] private float _force = 60;
    [SerializeField] private float _damage = 1;

    private float time;
    private GameObject fireBallSpherePrefab;

    private GameStateMachine stateMachine;
    public void Construct(GameStateMachine gameStateMachine)
    {
        stateMachine = gameStateMachine;
        fireBallSpherePrefab = Resources.Load<GameObject>("FireBall/FireBall");
    }
   
    private void Update()
    {
        if (stateMachine.CurrentState is PlayGameState)
            Timer();
    }

    private void Timer()
    {
        time += Time.deltaTime;
        if (time >= _timeToShoot)
        {
            time = 0;
            InstantiateFireBall();
        }
    }

    private void InstantiateFireBall()
    {
        var fireBallObject = Instantiate(fireBallSpherePrefab, transform.position, Quaternion.identity);

        var nearestEnemy = EnemyFactory.instance.NearestMob(gameObject);

        Vector3 vectorToMove = new Vector3(nearestEnemy.transform.position.x - transform.position.x, 0, nearestEnemy.transform.position.z - transform.position.z);

        fireBallObject.GetComponent<Rigidbody>().AddForce(vectorToMove.normalized * _force, ForceMode.Impulse);
        fireBallObject.AddComponent<FireBallWeapon>().Construct(_damage);
        Destroy(fireBallObject, 5f);
    }
}
