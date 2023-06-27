using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeController : MonoBehaviour
{
    [SerializeField]
    private Volume volume;
    private Bloom bloom;
    private MotionBlur blur;
    [SerializeField]
    private float intensity;

    private void Update()
    {
        volume.profile.TryGet(out bloom);
        {
            bloom.intensity.value = intensity;
        }
        volume.profile.TryGet(out blur);
        {
            blur.intensity.value = intensity;
        }
    }
}


