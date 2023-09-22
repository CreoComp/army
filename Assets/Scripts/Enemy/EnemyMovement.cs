using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private GameObject _player;
    private NavMeshAgent agent;


    private void Start()
    {
        _player = FindObjectOfType<PlayerMovement>().gameObject;
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(SetDestination());
    }
    IEnumerator SetDestination()
    {
        while(true)
        {
            yield return new WaitForSeconds(.5f);
            agent.SetDestination(_player.transform.position);

        }
    }
}
