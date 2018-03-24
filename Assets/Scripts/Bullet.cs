using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    public int speed;
    public int color;
    public bool pastBullet;

    void Start()
    {
        SetData();
    }

    void Update()
    {
        if (GameObject.Find("HomeScript").GetComponent<Home>().gameIsRuning)
        {
            this.transform.Translate(0, speed, 0);
            if (this.transform.position.y >= 800)
            {
                Destroy(this.gameObject);
            }
        }

    }

    public void SetData()
    {
        if (color == 1)
        {
            this.GetComponent<Image>().color = new Color32(239, 62, 119, 255);
        }
        else if (color == 2)
        {
            this.GetComponent<Image>().color = new Color32(0, 171, 185, 255);
        }
        else if (color == 3)
        {
            this.GetComponent<Image>().color = new Color32(255, 217, 73, 255);
        }
        else
        {
            this.GetComponent<Image>().color = new Color32(239, 62, 119, 255);
        }
    }



    void OnTriggerEnter2D(Collider2D enemy)
    {
        if (enemy.gameObject.name == "Enemy(Clone)")
        {
            if (enemy.gameObject.GetComponent<Enemy>().color == color)
            {
                GameObject.Find("HomeScript").GetComponent<Home>().PlayClash();
                GameObject.Find("HomeScript").GetComponent<Home>().score += 15;
                //GameObject.Find("HomeScript").GetComponent<Home>().scoreValueText.text = GameObject.Find("HomeScript").GetComponent<Home>().score.ToString();
                enemy.GetComponent<Animator>().Play("break");
                enemy.GetComponent<BoxCollider2D>().enabled = false;
                enemy.gameObject.GetComponent<Enemy>().isDead = true;
                if (GameObject.Find("HomeScript").GetComponent<Home>().score == 450)
                {
                    GameObject.Find("HomeScript").GetComponent<Home>().stopReleaseEnemy = true;
                    GameObject.Find("HomeScript").GetComponent<Home>().level = 2;
                }
                if (GameObject.Find("HomeScript").GetComponent<Home>().score == 1500)
                {
                    GameObject.Find("HomeScript").GetComponent<Home>().stopReleaseEnemy = true;
                    GameObject.Find("HomeScript").GetComponent<Home>().enemySpeed = 8;
                }
            }
            if (!pastBullet)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
