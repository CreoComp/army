using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour, IHealth
{
    public float health;

    private float _maxHealth;

    private bool isEnemy;
    private bool isBoss;

    [SerializeField] private Image HealthBar;
    [SerializeField] private TextMeshProUGUI HealthText;

    private bool isPlayer;

    private GameStateMachine _gameStateMachine;
    public void Construct(GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;

        if (GetComponent<EnemyMovement>())
            isEnemy = true;
        if (GetComponent<BossStateMachine>())
            isBoss = true;
        if (GetComponent<PlayerMovement>())
            isPlayer = true;

        _maxHealth = health;



        if (isPlayer)
        {
            SaveLoadService.Instance.ChangeCharacteristic += SetHealth;
            SetHealth();
        }
    }

    private void OnDisable()
    {
        if(isPlayer)
            SaveLoadService.Instance.ChangeCharacteristic -= SetHealth;
    }

    public void GetDamage(float damage)
    {
        if(_gameStateMachine.CurrentState is PlayGameState)
        {
            health -= damage;
            ChangeHealthBar();
            IsDeadCheck();
        }
    }

    private void IsDeadCheck()
    {
        if (health <= 0) 
            Dead();
    }

    private void Dead()
    {
        if(isEnemy || isBoss)
            EnemyFactory.instance.DestroyEnemy(gameObject);
        else if(isPlayer)
        {
            EnemyFactory.instance.PlayerDie();
        }
    }

    private void ChangeHealthBar()
    {
        if(HealthBar != null)
        {
            HealthBar.fillAmount = health / _maxHealth;
        }
        else if(HealthText != null)
        {
            HealthText.text = health + " / " + _maxHealth;
        }
    }
    public void SetHealth()
    {
        int value = SaveLoadService.Instance.PlayerData.HealthLevel * 5;
        if(value != _maxHealth)
        {
            _maxHealth = value;
            health = value;
            ChangeHealthBar();
        }
    }
}
