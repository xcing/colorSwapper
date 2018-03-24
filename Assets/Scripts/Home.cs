using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Home : MonoBehaviour
{

    public bool gameIsRuning;
    public int score;

    //valuable
    public FileData fileData;
    public BestScore bestScoreData;
    private Color32 colorRed;
    private Color32 colorBlue;
    private Color32 colorGreen;
    private int colorGun1;
    private int colorGun2;
    private int colorGun3;
    GameObject bulletPrefab1;
    GameObject bulletPrefab2;
    GameObject bulletPrefab3;
    GameObject bulletEntity1;
    GameObject bulletEntity2;
    GameObject bulletEntity3;
    GameObject enemyPrefab1;
    GameObject enemyPrefab2;
    GameObject enemyPrefab3;
    GameObject enemyEntity1;
    GameObject enemyEntity2;
    GameObject enemyEntity3;
    int randomInt;
    public Setting setting;
    bool soundOn;
    bool adsOn;
    int bulletSpeed;
    public int enemySpeed;
    public int level;
    public int enemyAlive;
    public bool stopReleaseEnemy;
    public GameObject admob;
    public int round;
    //ui
    public Image bg;
    public GameObject gun1;
    public GameObject gun2;
    public GameObject gun3;
    public Image tower1;
    public Image tower2;
    public Image tower3;
    public CanvasGroup bgStartCanvasGroup;
    public CanvasGroup bgLoadCanvasGroup;
    public CanvasGroup noadsButtonCanvasGroup;
    public CanvasGroup bgGameOverCanvasGroup;
    public CanvasGroup bgHelpCanvasGroup;
    public Text scoreValue;
    public Text bestScoreValue;
    public Image soundIcon;
    public AudioClip clash;
    public AudioClip soundBg;
    //public Text scoreValueText;
    public GameObject intro;

    void Awake()
    {
        BillingManager.init();
    }

    public void PurchaseNoAds()
    {
        BillingManager.purchase(BillingManager.NO_ADS_VERSION_PRODUCT_ID);
    }

    void Start()
    {
        round = 0;
        colorRed = new Color32(239, 62, 119, 255);
        colorBlue = new Color32(0, 171, 185, 255);
        colorGreen = new Color32(255, 217, 73, 255);
        setting = fileData.LoadSetting();
        soundOn = setting.soundOn;
        adsOn = setting.adsOn;
        if (soundOn)
        {
            soundIcon.sprite = Resources.Load<Sprite>("soundOnIcon");
        }
        else
        {
            soundIcon.sprite = Resources.Load<Sprite>("soundOffIcon");
        }
        PlaySoundBg();
        //admob.GetComponent<GoogleMobileAdsDemoScript>().RequestBanner();
        //admob.GetComponent<GoogleMobileAdsDemoScript>().RequestInterstitial();


        //replace with your ids
        string MY_BANNERS_AD_UNIT_ID = "ca-app-pub-9781457328779143/6673315110";
        string MY_INTERSTISIALS_AD_UNIT_ID = "ca-app-pub-9781457328779143/8150048315";


        //AndroidAdMobController.instance.Init(MY_BANNERS_AD_UNIT_ID);
        //If yoi whant to use Interstisial ad also, you need to set additional ad unin id for Interstisial as well
        //AndroidAdMobController.instance.SetInterstisialsUnitID(MY_INTERSTISIALS_AD_UNIT_ID);
        if(adsOn){
            AndroidAdMobController.instance.Init(MY_BANNERS_AD_UNIT_ID, MY_INTERSTISIALS_AD_UNIT_ID);
            GoogleMobileAdBanner banner;
            banner = AndroidAdMobController.instance.CreateAdBanner(0, 0, GADBannerSize.SMART_BANNER);

            AndroidAdMobController.instance.OnInterstitialLoaded += OnInterstisialsLoaded;
            AndroidAdMobController.instance.OnInterstitialOpened += OnInterstisialsOpen;
            AndroidAdMobController.instance.OnInterstitialClosed += OnInterstisialsClosed;
        }
        else
        {
            DisablePurchaseButton();
        }
    }

    public void DisablePurchaseButton()
    {
        noadsButtonCanvasGroup.alpha = 0;
        noadsButtonCanvasGroup.blocksRaycasts = false;
    }

    private void OnInterstisialsLoaded()
    {
        //ad loaded, strting ad
        
    }

    private void OnInterstisialsOpen()
    {
        //pausing the game
    }

    private void OnInterstisialsClosed()
    {
        //un-pausing the game
    }

    void GameStart()
    {
        for (int i = tower1.transform.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(tower1.transform.GetChild(i).gameObject);
        }
        for (int j = tower2.transform.childCount - 1; j >= 0; j--)
        {
            GameObject.Destroy(tower2.transform.GetChild(j).gameObject);
        }
        for (int k = tower3.transform.childCount - 1; k >= 0; k--)
        {
            GameObject.Destroy(tower3.transform.GetChild(k).gameObject);
        }
        gameIsRuning = true;

        bgStartCanvasGroup.alpha = 0;
        bgStartCanvasGroup.blocksRaycasts = false;
        bgGameOverCanvasGroup.alpha = 0;
        bgGameOverCanvasGroup.blocksRaycasts = false;

        round += 1;
        score = 0;
        //scoreValueText.text = "0";
        bulletSpeed = 16;
        enemySpeed = 4;
        level = 1;
        enemyAlive = 0;

        if (fileData.LoadBestScore() != null)
        {
            bestScoreData = fileData.LoadBestScore();
        }
        else
        {
            bestScoreData = new BestScore();
            bestScoreData.bestScore = 0;
            fileData.SaveBestScore(bestScoreData);
        }
        colorGun1 = 1;
        colorGun2 = 2;
        colorGun3 = 3;
        stopReleaseEnemy = false;
        //bg.color = new Color32(71, 71, 67, 255);
        tower1.sprite = Resources.Load<Sprite>("switch_pink");
        tower2.sprite = Resources.Load<Sprite>("switch_blue");
        tower3.sprite = Resources.Load<Sprite>("switch_yellow");

        gun1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Pink_Gun_Spritesheet_0");
        gun1.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Pink_Gun_Spritesheet_0");
        gun2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Blue_Gun_Spritesheet_0");
        gun2.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Blue_Gun_Spritesheet_0");
        gun3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Yellow_Gun_Spritesheet_0");
        gun3.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Yellow_Gun_Spritesheet_0");

        bulletPrefab1 = (GameObject)Resources.Load("Bullet", typeof(GameObject));
        bulletPrefab2 = (GameObject)Resources.Load("Bullet", typeof(GameObject));
        bulletPrefab3 = (GameObject)Resources.Load("Bullet", typeof(GameObject));
        enemyPrefab1 = (GameObject)Resources.Load("Enemy", typeof(GameObject));
        enemyPrefab2 = (GameObject)Resources.Load("Enemy", typeof(GameObject));
        enemyPrefab3 = (GameObject)Resources.Load("Enemy", typeof(GameObject));

        InvokeRepeating("Shotgun", 0, 0.7f);
        InvokeRepeating("RepleaseEnemy", 0, 1.0f);
    }

    void Update()
    {

    }

    public void Shotgun()
    {
        bulletPrefab1.gameObject.GetComponent<Bullet>().color = colorGun1;
        bulletPrefab1.gameObject.GetComponent<Bullet>().speed = bulletSpeed;
        bulletPrefab1.gameObject.GetComponent<Bullet>().pastBullet = false;
        bulletPrefab1.gameObject.GetComponent<Bullet>().SetData();
        bulletEntity1 = Instantiate(bulletPrefab1, new Vector3(1, 1, 1), Quaternion.identity) as GameObject;
        bulletEntity1.transform.parent = tower1.transform;
        bulletEntity1.transform.position = tower1.transform.position;
        bulletEntity1.transform.Translate(0, 286, 1);

        bulletPrefab2.gameObject.GetComponent<Bullet>().color = colorGun2;
        bulletPrefab2.gameObject.GetComponent<Bullet>().speed = bulletSpeed;
        bulletPrefab2.gameObject.GetComponent<Bullet>().pastBullet = false;
        bulletPrefab2.gameObject.GetComponent<Bullet>().SetData();
        bulletEntity2 = Instantiate(bulletPrefab2, new Vector3(1, 1, 1), Quaternion.identity) as GameObject;
        bulletEntity2.transform.parent = tower2.transform;
        bulletEntity2.transform.position = tower2.transform.position;
        bulletEntity2.transform.Translate(0, 286, 1);

        bulletPrefab3.gameObject.GetComponent<Bullet>().color = colorGun3;
        bulletPrefab3.gameObject.GetComponent<Bullet>().speed = bulletSpeed;
        bulletPrefab3.gameObject.GetComponent<Bullet>().pastBullet = false;
        bulletPrefab3.gameObject.GetComponent<Bullet>().SetData();
        bulletEntity3 = Instantiate(bulletPrefab3, new Vector3(1, 1, 1), Quaternion.identity) as GameObject;
        bulletEntity3.transform.parent = tower3.transform;
        bulletEntity3.transform.position = tower3.transform.position;
        bulletEntity3.transform.Translate(0, 286, 1);
    }

    public void RepleaseEnemy()
    {
        if (!stopReleaseEnemy)
        {
            randomInt = Random.Range(1, 3);
            if (randomInt == 1)
            {
                enemyPrefab1.gameObject.GetComponent<Enemy>().color = Random.Range(1, 4);
                enemyPrefab1.gameObject.GetComponent<Enemy>().speed = enemySpeed;
                enemyPrefab1.gameObject.GetComponent<Enemy>().SetData();
                enemyEntity1 = Instantiate(enemyPrefab1, new Vector3(1, 1, 1), Quaternion.identity) as GameObject;
                enemyEntity1.transform.parent = tower1.transform;
                enemyEntity1.transform.position = tower1.transform.position;
                enemyEntity1.transform.Translate(0, 1768, 1);
                enemyAlive++;
                if(enemyAlive == level){
                    enemyAlive = 0;
                    return;
                }
            }
            randomInt = Random.Range(1, 3);
            if (randomInt == 1)
            {
                enemyPrefab2.gameObject.GetComponent<Enemy>().color = Random.Range(1, 4);
                enemyPrefab2.gameObject.GetComponent<Enemy>().speed = enemySpeed;
                enemyPrefab2.gameObject.GetComponent<Enemy>().SetData();
                enemyEntity2 = Instantiate(enemyPrefab2, new Vector3(1, 1, 1), Quaternion.identity) as GameObject;
                enemyEntity2.transform.parent = tower2.transform;
                enemyEntity2.transform.position = tower2.transform.position;
                enemyEntity2.transform.Translate(0, 1768, 1);
                enemyAlive++;
                if (enemyAlive == level)
                {
                    enemyAlive = 0;
                    return;
                }
            }
            randomInt = Random.Range(1, 3);
            if (randomInt == 1)
            {
                enemyPrefab3.gameObject.GetComponent<Enemy>().color = Random.Range(1, 4);
                enemyPrefab3.gameObject.GetComponent<Enemy>().speed = enemySpeed;
                enemyPrefab3.gameObject.GetComponent<Enemy>().SetData();
                enemyEntity3 = Instantiate(enemyPrefab3, new Vector3(1, 1, 1), Quaternion.identity) as GameObject;
                enemyEntity3.transform.parent = tower3.transform;
                enemyEntity3.transform.position = tower3.transform.position;
                enemyEntity3.transform.Translate(0, 1768, 1);
                enemyAlive++;
                if (enemyAlive == level)
                {
                    enemyAlive = 0;
                    return;
                }
            }

        }
        else
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                stopReleaseEnemy = false;
                if (enemySpeed == 2)
                {
                    //bg.color = new Color32(88, 78, 96, 255);
                }
                else if (enemySpeed == 3)
                {
                    //bg.color = new Color32(69, 42, 1, 255);
                }
            }
        }
    }

    public void GameOver()
    {
        gameIsRuning = false;
        CancelInvoke();
        if (score == 0)
            scoreValue.text = "0";
        else
            scoreValue.text = score.ToString("#,#");
        if (score > bestScoreData.bestScore)
        {
            bestScoreData.bestScore = score;
            fileData.SaveBestScore(bestScoreData);
        }
        if (bestScoreData.bestScore == 0)
            bestScoreValue.text = "0";
        else
            bestScoreValue.text = bestScoreData.bestScore.ToString("#,#");

        if (round % 3 == 0 && adsOn)
        {
           // admob.GetComponent<GoogleMobileAdsDemoScript>().ShowInterstitial();
            AndroidAdMobController.instance.StartInterstitialAd();
            AndroidAdMobController.instance.ShowInterstitialAd();
        }

        bgGameOverCanvasGroup.alpha = 1;
        bgGameOverCanvasGroup.blocksRaycasts = true;
    }

    public void PlayButton()
    {
        Destroy(intro);
        GameStart();
    }

    public void ShareButton()
    {
        StartCoroutine(ShareScreen());
    }

    IEnumerator ShareScreen()
    {
        bgLoadCanvasGroup.alpha = 1;
        bgLoadCanvasGroup.blocksRaycasts = true;
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();
        yield return new WaitForSeconds(4);
        bgLoadCanvasGroup.alpha = 0;
        bgLoadCanvasGroup.blocksRaycasts = false;

        AndroidSocialGate.StartShareIntent("Share", "This is my score. Try to Beat me! https://play.google.com/store/apps/details?id=com.Dreamup.Swapper", texture);
    }

    public void RateButton()
    {
        AndroidNativeUtility.OpenAppRatingPage("market://details?id=com.Dreamup.Swapper");
    }

    public void SoundButton()
    {
        if (soundOn)
        {
            soundOn = false;
            soundIcon.sprite = Resources.Load<Sprite>("soundOffIcon");
        }
        else
        {
            soundOn = true;
            soundIcon.sprite = Resources.Load<Sprite>("soundOnIcon");
        }
        setting.soundOn = soundOn;
        fileData.SaveSetting(setting);
        PlaySoundBg();
    }

    public void HelpButton()
    {
        bgHelpCanvasGroup.alpha = 1;
        bgHelpCanvasGroup.blocksRaycasts = true;
    }

    public void BackFromHelpButton()
    {
        bgHelpCanvasGroup.alpha = 0;
        bgHelpCanvasGroup.blocksRaycasts = false;
    }

    public void PlaySoundBg()
    {
        if (soundOn)
        {
            GetComponent<AudioSource>().Play();
        }
        else
        {
            GetComponent<AudioSource>().Stop();
        }
    }

    public void PlayClash()
    {
        if (soundOn)
        {
            GetComponent<AudioSource>().PlayOneShot(clash);
        }
    }

    public void SwapColorGun(int position)
    {
        if (position == 1)
        {
            colorGun1 = SwapColorSequence(colorGun1, tower1, gun1);
        }
        else if (position == 2)
        {
            colorGun2 = SwapColorSequence(colorGun2, tower2, gun2);
        }
        else if (position == 3)
        {
            colorGun3 = SwapColorSequence(colorGun3, tower3, gun3);
        }
    }

    public int SwapColorSequence(int color, Image tower, GameObject gun)
    {
        if (color == 1)
        {
            tower.sprite = Resources.Load<Sprite>("switch_blue");
            gun.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Blue_Gun_Spritesheet_0");
            gun.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Blue_Gun_Spritesheet_0");
            return 2;
        }
        else if (color == 2)
        {
            tower.sprite = Resources.Load<Sprite>("switch_yellow");
            gun.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Yellow_Gun_Spritesheet_0");
            gun.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Yellow_Gun_Spritesheet_0");
            return 3;
        }
        else if (color == 3)
        {
            tower.sprite = Resources.Load<Sprite>("switch_pink");
            gun.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Pink_Gun_Spritesheet_0");
            gun.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Pink_Gun_Spritesheet_0");
            return 1;
        }
        else
        {
            tower.sprite = Resources.Load<Sprite>("switch_pink");
            gun.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Pink_Gun_Spritesheet_0");
            gun.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Pink_Gun_Spritesheet_0");
            return 1;
        }
    }

    public void linktoplayonloop()
    {
        Application.OpenURL("http://www.playonloop.com/2013-music-loops/mecha-world/");
    }
}
