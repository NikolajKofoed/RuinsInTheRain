using System.Collections;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private float roamChangeDir = 2f;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool stopMovingWhileAttacking = false;
    [SerializeField] private bool canRoamOnYAxis = false;

    private bool canAttack = true;

    private enum State
    {
        Roaming,
        Attacking
    }

    private State state;
    private EnemyPathFinding enemyPathFinding;
    private Vector2 roamPosition;
    private float timeRoaming = 0f;

    private void Awake()
    {
        enemyPathFinding = GetComponent<EnemyPathFinding>();
        state = State.Roaming;
    }

    private void Start()
    {
        roamPosition = GetRoamingPosition();
    }

    // Update is called once per frame
    void Update()
    {
        MovementStateControl();
    }

    private void MovementStateControl()
    {
        switch (state)
        {
            case State.Attacking:
                Attacking();
                break;
            case State.Roaming:
                Roaming();
                break;
        }
    }

    private void Roaming()
    {
        timeRoaming += Time.deltaTime;

        enemyPathFinding.MoveTo(roamPosition);

        if(Vector2.Distance(transform.position, Player2D.Instance.transform.position) < attackRange)
        {
            state = State.Attacking;
        }

        if(timeRoaming > roamChangeDir)
        {
            roamPosition = GetRoamingPosition();
        }
    }

    private void Attacking()
    {
        if (Vector2.Distance(transform.position, Player2D.Instance.transform.position) > attackRange)
        {
            state = State.Roaming;
        }

        if (attackRange != 0 && canAttack == false) { return; }
        canAttack = false;
        (enemyType as IEnemy).Attack();

        if (stopMovingWhileAttacking)
        {
            enemyPathFinding.StopMoving();
        }
        else
        {
            enemyPathFinding.MoveTo(roamPosition);
        }

        StartCoroutine(AttackCooldownRoutine());

    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private Vector2 GetRoamingPosition()
    {
        timeRoaming = 0f;
        if (canRoamOnYAxis)
        {
            return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        }
        return new Vector2(Random.Range(-1f, 0f), Random.Range(-1f, 0f)).normalized;
    }
}
