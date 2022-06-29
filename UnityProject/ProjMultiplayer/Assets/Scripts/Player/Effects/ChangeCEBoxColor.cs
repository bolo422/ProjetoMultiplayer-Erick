using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public enum Intensity { light, normal, hard, none}


public class ChangeCEBoxColor : MonoBehaviour, ISubscriber
{
    Volume vol;
    ColorAdjustments col;

    List<Color> colors;
    Color color;

    int currentLerp;

    LocalGameManager localGameManager;

    struct IntensityValue
    {
        public const float LIGHT = 0.08f;
        public const float NORMAL = 0.15f;
        public const float HARD = 0.25f;
    }

    private Intensity _intensity;
    public Intensity intensity { get => _intensity; set => _intensity = value; }

    private float currentIntensity;


    private bool changeColorEffect;
    public bool ChangeColorEffect { get => changeColorEffect; set => changeColorEffect = value; }

    private void Awake()
    {
        _intensity = Intensity.none;
        vol = GetComponent<Volume>();
        vol.profile.TryGet(out col);
    }

    private void Start()
    {
        SetupColorList();
        currentLerp = Random.Range(0, colors.Count);

        color = colors[currentLerp==0? colors.Count-1 : currentLerp-1];
        col.colorFilter.value = color;

        changeColorEffect = true;

        localGameManager = LocalGameManager.Instance;
        localGameManager.Subscribe(this);
        intensity = localGameManager.CurrentColorEffect;

        //StartCoroutine(TEST_ChangeIntensity());
        //StartCoroutine(LerpColor());

    }

    private void FixedUpdate()
    {
        if(!changeColorEffect)
        {
            col.active = false;
            return;
        }
        else if(col.active == false)
        {
            col.active = true;
        }


        UpdateIntensity();

        color = Color.Lerp(color, colors[currentLerp], currentIntensity);

        //if (color == colors[currentLerp])
        if(ColorComparate(color, colors[currentLerp]))
        {
            if (currentLerp == colors.Count - 1)
                currentLerp = 0;
            else
                currentLerp++;
        }

        col.colorFilter.value = color;
    }

    private void SetupColorList()
    {
        bool rand = Random.value > 0.5f;
        if(rand)
            colors = new List<Color>{Color.yellow, Color.red, Color.magenta, Color.blue, Color.cyan, Color.green};
        else
            colors = new List<Color>{ Color.green, Color.cyan, Color.blue, Color.magenta, Color.red, Color.yellow};

    }

    private bool ColorComparate(Color c1, Color c2)
    {
        bool red = Mathf.Abs(c1.r - c2.r) < 0.2f;
        bool green = Mathf.Abs(c1.g - c2.g) < 0.2f;
        bool blue = Mathf.Abs(c1.b - c2.b) < 0.2f;

        return red && green && blue;
    }

    private void UpdateIntensity()
    {
        switch(_intensity)
        {
            case Intensity.none: col.active = false;
                break;
            case Intensity.light: currentIntensity = IntensityValue.LIGHT;
                col.hueShift.value = 0.0f;
                break;
            case Intensity.normal: currentIntensity = IntensityValue.NORMAL;
                col.hueShift.value = 00.0f;
                break;
            case Intensity.hard:
                col.hueShift.value = 00.0f;
                //if(GlobalConfigs.UseHardChangeColorEffect)
                currentIntensity = IntensityValue.HARD;
              //else
              //currentIntensity = IntensityValue.NORMAL;
                break;
        }
    }

    IEnumerator TEST_ChangeIntensity()
    {
        intensity = Intensity.light;
        Debug.Log("LIGHT");
        yield return new WaitForSeconds(3.0f);
        intensity = Intensity.normal;
        Debug.Log("NORMAL");
        yield return new WaitForSeconds(3.0f);
        intensity = Intensity.hard;
        Debug.Log("HARD");
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(TEST_ChangeIntensity());
    }

    public void NotifySubscriber()
    {
        _intensity = localGameManager.CurrentColorEffect;
    }

    //IEnumerator LerpColor()
    //{
    //    color = Color.Lerp(color, colors[currentLerp], 2f);

    //    if(color == colors[currentLerp])
    //    {
    //        if (currentLerp == colors.Count - 1)
    //            currentLerp = 0;
    //        else
    //            currentLerp++;
    //    }

    //    col.colorFilter.value = color;
    //    yield return new WaitForSeconds(0.5f);
    //    StartCoroutine(LerpColor());

    //}




    //void Shuffle<T>(List<T> inputList)
    //{
    //    for (int i = 0; i < inputList.Count-1; i++)
    //    {
    //        T temp = inputList[i];
    //        int rand = Random.Range(i, inputList.Count);
    //        inputList[i] = inputList[rand];
    //        inputList[rand] = temp;
    //    }
    //}


}
