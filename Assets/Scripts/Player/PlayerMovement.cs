using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speedMove = 10f;
    [SerializeField] private float speedRotation = 10f;
    private CharacterController ch;
    private Animator animator;

    private GameStateMachine _gameStateMachine;

    public void Construct(GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        ch = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (_gameStateMachine.CurrentState is PlayGameState)
            Movement();
    } 

    private void Movement()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        var moveVector = new Vector3(x, 0, z);

        if (moveVector.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveVector.x, moveVector.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
            ch.Move(moveVector.normalized * speedMove * Time.deltaTime);
            animator.SetBool("run", true);

        }
        else
            animator.SetBool("run", false);

    }

    private void OnCollisionEnter(Collision collision)
    {
        var enemyWeapon = collision.gameObject.GetComponent<EnemyWeapon>();
        if (enemyWeapon)
        {
            GetComponent<Health>().GetDamage(enemyWeapon.GetDamageAmount());

            EnemyFactory.instance.DestroyEnemy(enemyWeapon.gameObject);
        }

        var enemy = collision.gameObject.GetComponent<EnemyMovement>();
        if(enemy)
        {
            GetComponent<Health>().GetDamage(_gameStateMachine.EnemyDamage);
            EnemyFactory.instance.DestroyEnemy(collision.gameObject);
        }
    }
}
