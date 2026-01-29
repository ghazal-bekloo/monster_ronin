using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class game_maneger : MonoBehaviour
{
    public int Score = 0;

    public int life = 4;

    public TextMeshProUGUI Score_Text;


    public GameObject Enemy_Prifabs;


    public GameObject Enemy_Prifabs1;



    public GameObject Win_Panel;

    public GameObject Lost_panel;

    [Header("Lifs")]
    public GameObject Life1;
    public GameObject Life2;
    public GameObject Life3;
    public GameObject Life4;


    public void LostOneLife()
    {
        life -= 1;

        // حذف یکی از جانها UI
        if (life == 3)
            Life4.SetActive(false);
        else if (life == 2)
            Life3.SetActive(false);
        else if (life == 1)
            Life2.SetActive(false);
        else if (life == 0)
            Life1.SetActive(false);

      

        if (life == 0)
        {
            You_lost();
        }
    }


    public void AddScore( int Ammont)
    {
        Score += Ammont;
        Score_Text.text = "Score : " + Score;

        if (Score >= 150  ) {
        You_win();
        }
    }

    public void You_lost()
    {

        Lost_panel.SetActive(true);
        Time.timeScale = 0;
    }

    public void reset_Game()
    {
        SceneManager.LoadScene(0);
    }
    public void You_win()
    {

        Win_Panel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Add_Gost_Kids(Transform gost_transform)
    {

        Debug.Log("test");
        gost_transform.position = new Vector3(gost_transform.position.x-6
            , gost_transform.position.y, gost_transform.position.z) ;

      
        Instantiate(Enemy_Prifabs , gost_transform ) ;

        GameObject enemy = Instantiate(Enemy_Prifabs, gost_transform.position, Quaternion.identity);
        enemy.GetComponent<EnemyScript>().gameManager = this;



        gost_transform.position = new Vector3(gost_transform.position.x-1, gost_transform.position.y+0.5f , gost_transform.position.z);


        GameObject enemy1 = Instantiate(Enemy_Prifabs1, gost_transform.position, Quaternion.identity);
        enemy1.GetComponent<EnemyScript>().gameManager = this;
    }



}
