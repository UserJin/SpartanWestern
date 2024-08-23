using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

// [��� ȿ�� ����]
// ����ϰ� ���� ���� Global Volume ������Ʈ�� Volume ������Ʈ�� ColorAdjustments �߰�
// FocusCtrl �Լ����� Focus���¿� ���� �� ���� �� �Լ� ȣ��� ��� ȿ�� ���� �� ������ ������
// grayMulti�� ���� ���� ��� ���¿� �ɸ��� �ð��� �پ��


public class GrayScaleEffect : MonoBehaviour
{
    public float grayScale = 0.0f;
    public float grayMulti; // grayScale�� �������� ���

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