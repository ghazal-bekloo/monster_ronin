using UnityEngine;

public enum EnemyType
{
    Reproducible,
    Friend,
    Enemy,
    Boss
}

public class EnemyScript : MonoBehaviour
{
    [Header("Enemy Settings")]
    public EnemyType enemyType;

    public float moveSpeed = 3f;

    [Header("Direction Slice Settings")]
    public float directionMin = 0f;
    public float directionMax = 22.5f;

    [Header("References")]
  
    public game_maneger gameManager;

    void Update()
    {
       

        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
      
        transform.Translate(transform.right * moveSpeed * Time.deltaTime, Space.World);
    }

    public bool IsInSlice(float angle)
    {
        return angle >= directionMin && angle <= directionMax;
    }



    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Door"))
        {
            if (gameManager != null)
                gameManager.LostOneLife();

            Destroy(gameObject);
        }
    }
}
