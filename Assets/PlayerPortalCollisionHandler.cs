using System;
using UnityEngine;

public class PlayerPortalCollisionHandler : MonoBehaviour
{
  public static event Action<GameObject> OnCollision; 

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (!other.CompareTag("Enemy")) return;
    
    OnCollision?.Invoke(other.gameObject);
  }
}
