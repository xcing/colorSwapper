using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    public int speed;
    public int color;
    public bool isDead;

	void Start () {
        SetData();
	}
	
	void Update () {
        if (GameObject.Find("HomeScript").GetComponent<Home>().gameIsRuning)
        {
            this.transform.Translate(0, -speed, 0);
            if (this.transform.position.y <= -700)
            {
                Destroy(this.gameObject);
            }
            if(isDead){
                StartCoroutine(DestroyEnemy());
            }
        }
	}

    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(this.gameObject);
    }

    public void SetData()
    {
        isDead = false;
        if (color == 1)
        {
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Bar/Bar_Pink_Spritesheet_0");
            this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Bar/Bar_Pink_Spritesheet_0");
        }
        else if (color == 2)
        {
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Bar/Bar_Blue_Spritesheet_0");
            this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Bar/Bar_Blue_Spritesheet_0");
        }
        else if (color == 3)
        {
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Bar/Bar_Yellow_Spritesheet_0");
            this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Bar/Bar_Yellow_Spritesheet_0");
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Bar/Bar_Pink_Spritesheet_0");
            this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Bar/Bar_Pink_Spritesheet_0");
        }
    }

    void OnTriggerEnter2D(Collider2D tower)
    {
        if (tower.gameObject.name == "Gun1" || tower.gameObject.name == "Gun2" || tower.gameObject.name == "Gun3")
        {
            GameObject.Find("HomeScript").GetComponent<Home>().GameOver();
        }
    }
}
