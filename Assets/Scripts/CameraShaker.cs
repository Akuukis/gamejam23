using Cinemachine;
using UnityEngine;

[System.Serializable]
public class ShakeSettings
{
    public float shakeDuration = 0.5f;
    public float shakeAmplitude = 1.2f;
    public float shakeFrequency = 2.0f;
}

public class CameraShaker : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public ShakeSettings defaultShakeSettings;
    public ShakeSettings highDamageShakeSettings;

    private ShakeSettings currentShakeSettings;
    private float shakeElapsedTime = 0f;

    private void Update()
    {
        if (shakeElapsedTime > 0)
        {
            // Generate random shake offset for the camera
            float shakeOffsetX = Random.Range(-1f, 1f) * currentShakeSettings.shakeAmplitude;
            float shakeOffsetY = Random.Range(-1f, 1f) * currentShakeSettings.shakeAmplitude;

            // Set the camera's noise values for shake effect
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = shakeOffsetX;
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = shakeOffsetY;

            // Reduce the shake elapsed time
            shakeElapsedTime -= Time.deltaTime;
        }
        else
        {
            // If shake duration is over, reset the camera noise values
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0f;
        }
    }

    public void ShakeCamera(bool isHighDamage)
    {
        if (isHighDamage)
        {
            // Start the camera shake with high damage shake settings
            currentShakeSettings = highDamageShakeSettings;
        }
        else
        {
            // Start the camera shake with default shake settings
            currentShakeSettings = defaultShakeSettings;
        }

        // Set the shake duration
        shakeElapsedTime = currentShakeSettings.shakeDuration;
    }
}