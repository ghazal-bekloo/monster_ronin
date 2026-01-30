using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeAndTapManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float tapDetectionThreshold = 0.2f;
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private TrailRenderer swipeTrail;

    [Header("References")]
    [SerializeField] private game_maneger gameManager;

    float swipeAngle = 0;

    private Vector2 touchStartPosition;
    private bool isTouching = false;
    private float touchStartTime;


    public AudioSource Miss_the_cut_Sound;

    public ParticleSystem missEffect;

    public AudioSource Kill_the_Enemy_Sound;
    private void Awake()
    {
        Time.timeScale = 1.0f;

    }

    private void Update()
    {
        if (Input.touchCount == 0) return;

        Touch t = Input.GetTouch(0);

        if (t.phase == TouchPhase.Began)
            HandleTouchBegan(t.position);
        else if (t.phase == TouchPhase.Moved)
            HandleTouchMoved(t.position);
        else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
            HandleTouchEnded(t.position);
    }

    private void HandleTouchBegan(Vector2 screenPosition)
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(screenPosition);
        wp.z = 0;

        touchStartPosition = new Vector2(wp.x, wp.y);

        touchStartTime = Time.time;
        isTouching = true;

        if (swipeTrail != null)
        {
            swipeTrail.enabled = false;        
            swipeTrail.Clear();                
            swipeTrail.transform.position = wp; 
            swipeTrail.enabled = true;         
            swipeTrail.emitting = true;        
        }

    }

    private void HandleTouchMoved(Vector2 screenPosition)
    {
        if (!isTouching || swipeTrail == null) return;

        Vector3 wp = Camera.main.ScreenToWorldPoint(screenPosition);
        wp.z = 0;
        swipeTrail.transform.position = wp;

    }


    void PlayMissEffect(Vector3 pos)
    {
        if (missEffect == null) return;

        missEffect.transform.position = pos;
        missEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        missEffect.Play();
    }

    private void HandleTouchEnded(Vector2 screenPosition)
    {
        if (!isTouching) return;
        isTouching = false;


        Vector3 wp = Camera.main.ScreenToWorldPoint(screenPosition);
        Vector2 endPosition = new Vector2(wp.x, wp.y);

        Vector2 delta = endPosition - touchStartPosition;
        float dist = delta.magnitude;


        if (swipeTrail != null)
        {
            swipeTrail.emitting = false;  
            swipeTrail.enabled = false;   
        }


    
        if (dist <= tapDetectionThreshold)
        {
            PerformTapAction(endPosition);
            return;
        }

       
        if (dist < 0.1f) return;

        delta.Normalize();
        swipeAngle = Vector2.SignedAngle(Vector2.right, delta);

        RaycastHit2D hit = Physics2D.Linecast(touchStartPosition, endPosition, enemyLayerMask);
        if (!hit) return;

        EnemyScript es = hit.collider.GetComponent<EnemyScript>();
        if (es == null) return;

        ProcessEnemyHit(hit.collider.gameObject, es, swipeAngle);
    }
    private void PerformTapAction(Vector2 tapPosition)
    {
        Collider2D col = Physics2D.OverlapPoint(tapPosition, enemyLayerMask);
        if (col == null) return;

        EnemyScript enemyScript = col.GetComponent<EnemyScript>();
        if (enemyScript == null) return;

        GameObject enemy = col.gameObject;

        bool sliced = enemyScript.IsInSlice(swipeAngle);

        if (enemyScript.enemyType == EnemyType.Friend)
        {
            gameManager.AddScore(15);
            KillEnemy(enemy);
            return;
        }

      
    }

    private void ProcessEnemyHit(GameObject enemy, EnemyScript enemyScript, float swipeAngle)
    {
        bool sliced = enemyScript.IsInSlice(swipeAngle);

        if (enemyScript.enemyType == EnemyType.Friend)
        {
            gameManager.LostOneLife();
            KillEnemy(enemy);
            return;
        }

        if (enemyScript.enemyType == EnemyType.Enemy )
        {
            if (sliced)
            {
                gameManager.AddScore(15);
                KillEnemy(enemy);
            }
            else
            {
                
                Miss_the_cut_Sound.Play();
                PlayMissEffect(enemy.transform.position);
            }
            return;
        }

        if (enemyScript.enemyType == EnemyType.Reproducible)
        {
            if (sliced)
            {
                gameManager.AddScore(25);
              
                gameManager.Add_Gost_Kids(enemy.transform);
                
                KillEnemy(enemy);
            }
            else
            {
                enemyScript.moveSpeed += 0.5f;
                Miss_the_cut_Sound.Play();
                PlayMissEffect(enemy.transform.position);
            }
            return;
        }

        if (enemyScript.enemyType == EnemyType.Boss)
        {
            Debug.Log("Boss sliced? (Not implemented)");
        }
    }

    private void KillEnemy(GameObject enemy)
    {
        if (enemy != null)
            Kill_the_Enemy_Sound.Play();
            Destroy(enemy);
    }
}
