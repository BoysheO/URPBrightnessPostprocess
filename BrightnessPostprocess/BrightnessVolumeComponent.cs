using UnityEngine.Rendering;

namespace Postprocess
{
    public class BrightnessVolumeComponent : VolumeComponent
    {
        public FloatParameter BrightnessStrength = new FloatParameter(1);
    }
}