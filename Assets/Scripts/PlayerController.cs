using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float clickIgnoreRadius = 0.5f;
    [SerializeField] private float attackDistance = 0.5f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackInterval = 1.5f;

    private Vector2 _currentDirection = Vector2.down;
    private Transform _playerDirIndicator;
    private BoxCollider2D _playerCollider;
    private Transform _playerAttackPivot;
    private float _attackIntervalElapsed;

    /* Like the Awake function, Start is called exactly once in the lifetime of the script. However, Awake is called when the script object is initialised, regardless of whether or not the script is enabled. Start may not be called on the same frame as Awake if the script is not enabled at initialisation time. If variable initialisation can be deferred until the script is enabled, use Start (lazy loading). */
    void Start()
    {
        _playerDirIndicator = GetComponentInChildren<Transform>();
        _playerCollider = GetComponent<BoxCollider2D>();
        _playerAttackPivot = transform.Find("Player Attack Pivot");
    }

    /* Update is called every frame, if the MonoBehaviour is enabled. */
    void Update()
    {
        if (!Input.GetMouseButton(0)) return;

        Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 currentPosition = transform.position;

        MovePlayer(currentPosition, targetPosition);
        HandleCollisions(currentPosition);
        RotatePlayer(currentPosition, targetPosition, _playerDirIndicator);
        HandleAttack(targetPosition);
    }

    private void MovePlayer(Vector2 currentPosition, Vector2 targetPosition)
    {
        Vector2 distance = targetPosition - currentPosition;
        var distanceSqrLen = distance.sqrMagnitude;

        if (distanceSqrLen >= clickIgnoreRadius * clickIgnoreRadius)
        {
            Vector2 dir = distance.normalized;
            Vector2 velocity = dir * speed * Time.deltaTime;
            transform.Translate(velocity, Space.World);
        }
    }

    private void HandleCollisions(Vector2 currentPosition)
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(currentPosition, _playerCollider.size, 0);

        foreach (Collider2D hit in hits)
        {
            if (hit == _playerCollider) continue;

            ColliderDistance2D colliderDistance2D = hit.Distance(_playerCollider);

            if (colliderDistance2D.isOverlapped)
            {
                transform.Translate(colliderDistance2D.pointA - colliderDistance2D.pointB, Space.World);
            }
        }
    }

    private void RotatePlayer(Vector2 currentPosition, Vector2 targetPosition, Transform playerDirIndicator)
    {
        Vector2 distance = targetPosition - currentPosition;
        Vector2 newDirection = distance.normalized;
        float angle = Vector2.SignedAngle(_currentDirection, newDirection);

        if (angle <= -1 || angle >= 1)
        {
            _currentDirection = newDirection;
            playerDirIndicator.Rotate(0, 0, angle);
        }
    }

    private void HandleAttack(Vector2 targetPosition)
    {
        _attackIntervalElapsed += Time.deltaTime;

        if (attackInterval > _attackIntervalElapsed) return;
        
        Vector2 currentPosition = _playerAttackPivot.position;
        Vector2 distance = targetPosition - currentPosition;
        Vector2 direction = distance.normalized;
        LayerMask attackableLayer = LayerMask.GetMask("Attackable");
        RaycastHit2D hit = Physics2D.Raycast(currentPosition, direction, attackDistance, attackableLayer);

        if (hit.collider is null) return;

        Attackable attackable = hit.collider.gameObject.GetComponent<Attackable>();
        attackable.Damage(attackDamage);
        _attackIntervalElapsed = 0;
    }
}