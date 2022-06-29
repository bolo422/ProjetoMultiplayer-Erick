using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MilkShake;
using UnityEngine.UI;

public enum ShakerEffect { none, light, normal, hard}

public class CameraEffects : MonoBehaviour, ISubscriber
{

    ShakePreset noneShakePreset;
    ShakePreset lightShakePreset;
    ShakePreset normalShakePreset;
    ShakePreset hardShakePreset;
    ShakeInstance shakeInstance;
    Shaker sh;

    ShakerEffect shakerEffect;
    ShakerEffect tempEffect;

    LocalGameManager localGameManager;

    bool shouldUpdate = false;

    private void Awake()
    {
        sh = GetComponent<Shaker>();
        noneShakePreset = Resources.Load<ShakePreset>("ShakePresets/NoneDrunk");
        lightShakePreset = Resources.Load<ShakePreset>("ShakePresets/LightDrunk");
        normalShakePreset = Resources.Load<ShakePreset>("ShakePresets/NormalDrunk");
        hardShakePreset = Resources.Load<ShakePreset>("ShakePresets/HardDrunk");

        localGameManager = LocalGameManager.Instance;
        localGameManager.Subscribe(this);

    }

    private void Start()
    {
        shakeInstance = new ShakeInstance();
        shakerEffect = localGameManager.CurrentShakerEffect;
        drunkEffect();
        //tempEffect = shakerEffect;
        //StartCoroutine(ShakeEffectLoop());
    }

    private void drunkEffect()
    {

        //Debug.LogError("ShakerEffect: " + shakerEffect);
        //if (text != null)
        //    text.text = "ShakerEffect: " + shakerEffect;

        StopAllCoroutines();

        switch (shakerEffect)
        {
            case ShakerEffect.light:
                StartCoroutine(ChangeEffect(lightShakePreset));
                //shakeInstance = sh.Shake(lightShakePreset);
                break;

            case ShakerEffect.normal:
                StartCoroutine(ChangeEffect(normalShakePreset));
                //shakeInstance = sh.Shake(normalShakePreset);
                break;

            case ShakerEffect.hard:
                StartCoroutine(ChangeEffect(hardShakePreset));
                //shakeInstance = sh.Shake(hardShakePreset);
                break;

            case ShakerEffect.none:
                shakeInstance.Stop(2.1f, false);
                break;
        }
    }

    IEnumerator ShakeEffectLoop()
    {
        drunkEffect();
        yield return new WaitForSeconds(10.0f);
        tempEffect = shakerEffect;
        shakerEffect = ShakerEffect.none;
        drunkEffect();
        yield return new WaitForSeconds(2.0f);
        shakerEffect = tempEffect;
        StartCoroutine(ShakeEffectLoop());
    }

    IEnumerator ChangeEffect(ShakePreset preset)
    {
        shakeInstance.Stop(2.0f, false);
        yield return new WaitForSeconds(2.1f);
        shakeInstance = sh.Shake(preset);
    }

    public void NotifySubscriber()
    {
        shakerEffect = localGameManager.CurrentShakerEffect;
        //shouldUpdate = shakerEffect == ShakerEffect.none;
        drunkEffect();



        //tempEffect = shakerEffect;


        //drunkEffect(shakerEffect);

        //StopCoroutine(ShakeEffectLoop());
        //StartCoroutine(ShakeEffectLoop());
    }
}
