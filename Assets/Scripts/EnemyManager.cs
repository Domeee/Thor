using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
  [SerializeField] private float interval = 1.0f;
  [SerializeField] private GameObject enemyPrefab;
  private float _elapsed;
  private BoxCollider2D _collider2D;
  
  /* Like the Awake function, Start is called exactly once in the lifetime of the script. However, Awake is called when the script object is initialised, regardless of whether or not the script is enabled. Start may not be called on the same frame as Awake if the script is not enabled at initialisation time. If variable initialisation can be deferred until the script is enabled, use Start (lazy loading). */
  void Start()
  {
    _collider2D = GetComponentInChildren<BoxCollider2D>();
    PlayerPortalCollisionHandler.OnCollision += RemoveEnemy;
  }


  /* FixedUpdate has the frequency of the physics system; it is called every fixed frame-rate frame. 0.02 seconds (50 calls per second) is the default time between calls. Use Time.fixedDeltaTime to access this value. Use FixedUpdate when using Rigidbody. Set a force to a Rigidbody and it applies each fixed frame. Multiply it with Time.fixedDeltaTime and it applies each second. */
  void FixedUpdate()
  {
    _elapsed += Time.fixedDeltaTime;
    
    if (!(_elapsed >= interval)) return;
    
    Vector2 extents = _collider2D.size / 2f;
    Vector2 position = new Vector2(
      Random.Range( -extents.x, extents.x ),
      Random.Range( -extents.y, extents.y )
    );
    
    position = _collider2D.transform.TransformPoint(position);

    Instantiate(enemyPrefab, position, Quaternion.identity);
    _elapsed = 0;
  }

  private void RemoveEnemy(GameObject enemy)
  {
    Destroy(enemy);
  }
}
