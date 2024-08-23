using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

// [흑백 효과 사용법]
// 사용하고 싶은 씬의 Global Volume 오브젝트의 Volume 컴포넌트에 ColorAdjustments 추가
// FocusCtrl 함수에서 Focus상태에 돌입 및 해제 시 함수 호출로 흑백 효과 적용 및 해제가 가능함
// grayMulti는 높을 수록 흑백 상태에 걸리는 시간이 줄어듬


public class GrayScaleEffect : MonoBehaviour
{
    public float grayScale = 0.0f;
    public float grayMulti; // grayScale에 곱해지는 배수

    private IEnumerator coroutine;
    private Volume volume;
    private ColorAdjustments colAdj;
    private DepthOfField dof;
    public Camera cam;

    void Start()
    {
        grayMulti = 800.0f;
        cam = Camera.main;
        volume = cam.GetComponent<Volume>();
        volume.profile.TryGet<ColorAdjustments>(out colAdj);
        volume.profile.TryGet<DepthOfField>(out dof);

    }

    public void StartFocus()
    {
        if (colAdj == null) return;
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartEffect();
        StartCoroutine(coroutine);
    }

    public void StopFocus()
    {
        if (colAdj == null) return;
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StopEffect();
        StartCoroutine(coroutine);
    }

    private IEnumerator StartEffect()
    {
        dof.active = true;
        while(grayScale > -100.0f)
        {
            grayScale -= Time.deltaTime * grayMulti;
            colAdj.saturation.value = grayScale;
            yield return null;
        }
        grayScale = -100.0f;
        colAdj.saturation.value = grayScale;
    }

    private IEnumerator StopEffect()
    {
        dof.active = false;
        while (grayScale < 0.0f)
        {
            grayScale += Time.deltaTime * grayMulti;
            colAdj.saturation.value = grayScale;
            yield return null;
        }
        grayScale = 0.0f;
        colAdj.saturation.value = grayScale;
    }

}