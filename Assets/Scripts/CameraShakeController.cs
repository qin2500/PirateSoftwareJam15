using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeController : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin perlinNoise;
    // Start is called before the first frame update
    void Start()
    {
        virtualCamera= GetComponent<CinemachineVirtualCamera>();
        perlinNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlinNoise.m_AmplitudeGain = 0f;
    }

    public void shakeCamera(float intensity, float duration)
    {
        perlinNoise.m_AmplitudeGain= intensity;
        StartCoroutine(stopShake(duration));

    }

    private IEnumerator stopShake(float duration)
    {
        yield return new WaitForSeconds(duration);
        perlinNoise.m_AmplitudeGain = 0f;
    }
}
