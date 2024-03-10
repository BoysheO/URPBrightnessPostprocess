using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Postprocess
{
    public class BrightnessPass : ScriptableRenderPass
    {
        private const string PassName = "Brightness";
        private readonly ProfilingSampler m_ProfilingSampler = new(PassName);
        private static readonly int BrightnessStrength = Shader.PropertyToID("_BrightnessStrength");
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");
        private readonly Material _material;

        public BrightnessPass()
        {
            renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
            _material = CoreUtils.CreateEngineMaterial("Shader Graphs/BrightnessShader");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!renderingData.postProcessingEnabled) return;
            if (renderingData.cameraData.isPreviewCamera) return;
            if (renderingData.cameraData.isSceneViewCamera) return;

            var stack = VolumeManager.instance.stack;
            var volumeComponent = stack.GetComponent<BrightnessVolumeComponent>();
            if (!volumeComponent) return;

#if DEBUG
            if (_material == null)
            {
                Debug.LogError("missing material");
                return;
            }
#endif

            CommandBuffer cmd = CommandBufferPool.Get(PassName);
            using (new ProfilingScope(cmd, m_ProfilingSampler))
            {
                var rtH = renderingData.cameraData.renderer.cameraColorTargetHandle;
                var tmp = RenderTexture.GetTemporary(desc: renderingData.cameraData.cameraTargetDescriptor);
                _material.SetFloat(BrightnessStrength, volumeComponent.BrightnessStrength.value);
                cmd.SetGlobalTexture(MainTex, rtH);
                cmd.Blit(rtH, tmp, _material);
                cmd.Blit(tmp, rtH);
                RenderTexture.ReleaseTemporary(tmp);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }
}