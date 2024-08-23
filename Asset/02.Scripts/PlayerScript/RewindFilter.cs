using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RewindFilter : MonoBehaviour
{
    [SerializeField]
    [ColorUsage(true, true)]
    Color color; // 필터에 적용되는 색 Default(183, 104, 58)
    [SerializeField]
    float satValue; // 필터에 적용되는 채도(낮으면 회색) Default(-40)
    
    public Volume volume;
    public Bloom bloom;
    public FilmGrain fg;
    public LiftGammaGain lgg;
    public ColorAdjustments colorAdjustments;
    public ChromaticAberration ca;
    public Camera cam;
    public Vignette vg;
    public DepthOfField df;
    private void Start()
    {
        cam = Camera.main;
        satValue = -60f;
        color = new Color(255/255f,190/255f,135/255f);
        volume = cam.GetComponent<Volume>();
        volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
        volume.profile.TryGet<LiftGammaGain>(out lgg);
        volume.profile.TryGet<FilmGrain>(out fg);
        volume.profile.TryGet<ChromaticAberration>(out ca);
        volume.profile.TryGet<Vignette>(out vg);
    }

    public void StartRewindEffect()
    {
        colorAdjustments.colorFilter.value = color;
        colorAdjustments.saturation.value = satValue;
        lgg.active = false;
        fg.active = true;
        ca.active = true;
        vg.active = true;
    }

    public void StopRewindEffect()
    {
        colorAdjustments.colorFilter.value = Color.white;
        colorAdjustments.saturation.value = 0.0f;
        lgg.active = true;
        fg.active = false;
        ca.active = false;
        vg.active = false;
    }
}
