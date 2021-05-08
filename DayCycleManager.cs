using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{
    [Range(0, 1)]
    public float TimeOfDay;
    public float DayDuration = 30f;

    public AnimationCurve SunCurve;
    public AnimationCurve MoonCurve;
    public AnimationCurve ScyboxCurve;

    public Material DayScybox;
    public Material NightScybox;

    public ParticleSystem Stars;

    public Light Sun;
    public Light Moon;

    private float SunIntensity;
    private float MoonIntensity;

    // Start is called before the first frame update
    private void Start()
    {
        SunIntensity = Sun.intensity;
        MoonIntensity = Moon.intensity;
    }

    // Update is called once per frame
    private void Update()
    {
        TimeOfDay += Time.deltaTime / DayDuration;
        if (TimeOfDay >= 1) TimeOfDay -= 1;

        RenderSettings.skybox.Lerp(NightScybox, DayScybox, ScyboxCurve.Evaluate(TimeOfDay));
        RenderSettings.sun = ScyboxCurve.Evaluate(TimeOfDay) > 0.1 ? Sun : Moon;
        DynamicGI.UpdateEnvironment();

        var mainModule = Stars.main;
        mainModule.startColor = new Color(1, 1, 1, 1 - ScyboxCurve.Evaluate(TimeOfDay));

        Sun.transform.localRotation = Quaternion.Euler(TimeOfDay * 360f, 180, 0);
        Moon.transform.localRotation = Quaternion.Euler(TimeOfDay * 360f + 180f, 180, 0);

        Sun.intensity = SunIntensity * SunCurve.Evaluate(TimeOfDay);
        Moon.intensity = MoonIntensity * MoonCurve.Evaluate(TimeOfDay);
    }
}
