using UnityEngine;

public class Attackable : MonoBehaviour
{
  [SerializeField] private int hitPoints = 20;
  private int _damage;
  
  /* Update is called every frame, if the MonoBehaviour is enabled. */
  void Update()
  {
    if (_damage >= hitPoints) Destroy(gameObject);
  }

  public void Damage(int damage)
  {
    _damage += damage;
  }
}
