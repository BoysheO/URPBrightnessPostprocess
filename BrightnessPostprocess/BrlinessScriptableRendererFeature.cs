using UnityEngine.Rendering.Universal;

namespace Postprocess
{
    public class BrightnessScriptableRendererFeature : ScriptableRendererFeature
    {
        private BrightnessPass _pass;

        public override void Create()
        {
            //do nothing
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            _pass = new BrightnessPass();
            renderer.EnqueuePass(_pass);
        }
    }
}