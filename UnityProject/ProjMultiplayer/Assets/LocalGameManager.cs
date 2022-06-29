using Photon.Bolt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LocalGameManager : GlobalEventListener, IPublisher
{
    private HashSet<ISubscriber> subscribers = new HashSet<ISubscriber>();

    public static LocalGameManager Instance { get; private set; }
    public Intensity CurrentColorEffect { get => currentColorEffect; set => currentColorEffect = value; }
    public ShakerEffect CurrentShakerEffect { get => currentShakerEffect; set => currentShakerEffect = value; }

    private Intensity currentColorEffect = Intensity.none;

    private ShakerEffect currentShakerEffect = ShakerEffect.none;

    bool canFinishLevel = false;

    Text canFinishLevelText;
    Text objectivesText;

    private Intensity tempColorEffect;

    float result;

    float[] colorEffectTimerActive;
    float[] colorEffectTimerPause;

    public void Subscribe(ISubscriber subs)
    {
        subscribers.Add(subs);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        //DontDestroyOnLoad(gameObject);


    }

    private void Start()
    {
        StartCoroutine(DelayedStart());
        canFinishLevelText = GameObject.FindGameObjectWithTag("finishLevelText").GetComponent<Text>();
        objectivesText = GameObject.FindGameObjectWithTag("objectivesText").GetComponent<Text>();
        objectivesText.text = 0 + " / " + FindObjectsOfType<BoxSpawner>().Length;
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.5f);

        float timer = 0.0f;

        if (SceneManager.GetActiveScene().name == "wh_1")
        {
            currentColorEffect = Intensity.none;
            currentShakerEffect = ShakerEffect.normal;
            timer = 240.0f;
            colorEffectTimerActive = new float[] { 0f, 0f };
            colorEffectTimerPause = new float[] { 0f, 0f };
        }
        else if (SceneManager.GetActiveScene().name == "wh_2")
        {
            currentColorEffect = Intensity.none;
            currentShakerEffect = ShakerEffect.normal;
            timer = 200.0f;
            colorEffectTimerActive = new float[] { 0f, 0f };
            colorEffectTimerPause = new float[] { 0f, 0f };
        }
        else if (SceneManager.GetActiveScene().name == "wh_3")
        {
            currentColorEffect = Intensity.light;
            currentShakerEffect = ShakerEffect.light;
            timer = 240.0f;
            colorEffectTimerActive = new float[] { 10.0f, 15.0f };
            colorEffectTimerPause = new float[] { 2.0f, 4.0f };
        }
        else if (SceneManager.GetActiveScene().name == "wh_4")
        {
            currentColorEffect = Intensity.normal;
            currentShakerEffect = ShakerEffect.normal;
            timer = 180.0f;
            colorEffectTimerActive = new float[] { 4.0f, 8.0f };
            colorEffectTimerPause = new float[] { 1.0f, 2.0f };
        }

        // PARA TESTES
        //currentColorEffect = Intensity.none;
        //currentShakerEffect = ShakerEffect.none;


        GetComponent<CountdownTimer>().StartTimer(timer);

        if(currentColorEffect != Intensity.none)
        {
            tempColorEffect = currentColorEffect;
            StartCoroutine(AlternateColorEffect());
        }

        NotifySubs();
    }

    IEnumerator AlternateColorEffect()
    {
        yield return new WaitForSeconds(Random.Range(colorEffectTimerActive[0], colorEffectTimerActive[1]));
        ChangeColorEffect(Intensity.none);
        yield return new WaitForSeconds(Random.Range(colorEffectTimerPause[0], colorEffectTimerPause[1]));
        ChangeColorEffect(tempColorEffect);
        StartCoroutine(AlternateColorEffect());
    }

    public void ChangeColorEffect(Intensity intensity)
    {
        currentColorEffect = intensity;
        NotifySubs();
    }

    private void NotifySubs()
    {
        foreach (ISubscriber s in subscribers)
        {
            s.NotifySubscriber();
        }
    }


    public override void OnEvent(canFinishLevelEvent evnt)
    {
        //Text text = GameObject.FindGameObjectWithTag("testText").GetComponent<Text>();
        //text.text =

        canFinishLevel = evnt.canFinish;
        canFinishLevelText.gameObject.SetActive(canFinishLevel);
        canFinishLevelText.text = "Você já pode pressionar \"G\" para finalizar o nível";
        objectivesText.text = evnt.achived + " / " + evnt.total;

        if (canFinishLevel)
            result = evnt.result;
    }





    ///////// Testssss

    private void Update()
    {
        if((Input.GetKeyDown(KeyCode.G) && canFinishLevel) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            var evnt = NextLevelEvent.Create();
            evnt.Send();
        }




        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            RotateDrunk();
        }

        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            RotateLSD();
        }

        if(Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            DrunkNone();
        }
    }

    private void RotateDrunk()
    {
        switch(currentShakerEffect)
        {
            case ShakerEffect.none: DrunkLight(); break;
            case ShakerEffect.light: DrunkNormal(); break;
            case ShakerEffect.normal: DrunkHard(); break;
            case ShakerEffect.hard: DrunkNone(); break;
        }
    }

    private void RotateLSD()
    {
        switch (currentColorEffect)
        {
            case Intensity.none:    LSDLight(); break;
            case Intensity.light:   LSDNormal(); break;
            case Intensity.normal:  LSDHard(); break;
            case Intensity.hard:    LSDNone(); break;
        }
    }


    public void DrunkNone()
    {
        currentShakerEffect = ShakerEffect.none;
        NotifySubs();
    }
    public void DrunkLight()
    {
        currentShakerEffect = ShakerEffect.light;
        NotifySubs();
    }
    public void DrunkNormal()
    {
        currentShakerEffect = ShakerEffect.normal;
        NotifySubs();
    }
    public void DrunkHard()
    {
        currentShakerEffect = ShakerEffect.hard;
        NotifySubs();
    }
    public void LSDNone()
    {
        currentColorEffect = Intensity.none;
        NotifySubs();
    }
    public void LSDLight()
    {
        currentColorEffect = Intensity.light;
        NotifySubs();
    }
    public void LSDNormal()
    {
        currentColorEffect = Intensity.normal;
        NotifySubs();
    }
    public void LSDHard()
    {
        currentColorEffect = Intensity.hard;
        NotifySubs();
    }
}
