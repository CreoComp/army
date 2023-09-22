using UnityEngine;

public class Lazer : MonoBehaviour
{
    [SerializeField] private float _timeToShoot = 1;

    private float time;
    private GameObject lazerSpherePrefab;
    private Material defaultMaterial;
    private GameObject lazerSphere;

    private GameStateMachine stateMachine;
    public void Construct(GameStateMachine gameStateMachine)
    {
        stateMachine = gameStateMachine;
        lazerSpherePrefab = Resources.Load<GameObject>("Lazer/LazerSphere");
        defaultMaterial = Resources.Load<Material>("Material/DefaultMaterial");
        lazerSphere = Instantiate(lazerSpherePrefab, new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z), Quaternion.identity);
        lazerSphere.transform.SetParent(transform);
    }

    private void Update()
    {
        if(stateMachine.CurrentState is PlayGameState)
        LazerTimer();
    }

    private void LazerTimer()
    {
        time += Time.deltaTime;
        if(time > _timeToShoot)
        {
            time = 0;
            Attack();
        }
    }

    private void Attack()
    {
        var enemy = EnemyFactory.instance.NearestMob(gameObject);
        InstantiateLine(enemy);
        enemy.GetComponent<Health>().GetDamage(1);
    }

    private void InstantiateLine(GameObject enemy)
    {

        var lineObject = new GameObject();
        var line = lineObject.AddComponent<LineRenderer>();
        line.positionCount = 2;
        line.startColor = Color.red;
        line.endColor = Color.red;
        line.material = defaultMaterial;
        line.SetPosition(0, lazerSphere.transform.position);
        line.SetPosition(1, enemy.transform.position);
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;

        Destroy(line, 0.2f);

    }
}
