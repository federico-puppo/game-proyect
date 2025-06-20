using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

public class SelectiveColorPassFeature : ScriptableRendererFeature
{
    class SelectiveColorPass : ScriptableRenderPass
    {
        private FilteringSettings filteringSettings;
        private List<ShaderTagId> shaderTagIds = new List<ShaderTagId>
        {
            new ShaderTagId("UniversalForward"),
            new ShaderTagId("SRPDefaultUnlit"),
            new ShaderTagId("UniversalForwardOnly")
        };

        private string profilerTag = "Selective Color Pass";

        public SelectiveColorPass(LayerMask layerMask)
        {
            // Renderiza objetos transparentes y opacos
            filteringSettings = new FilteringSettings(RenderQueueRange.all, layerMask);
            renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var cmd = CommandBufferPool.Get(profilerTag);
            using (new ProfilingScope(cmd, new ProfilingSampler(profilerTag)))
            {
                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();

                var drawSettings = CreateDrawingSettings(shaderTagIds, ref renderingData, SortingCriteria.CommonOpaque);
                context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref filteringSettings);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    [System.Serializable]
    public class SelectiveColorSettings
    {
        public LayerMask colorLayer;
    }

    public SelectiveColorSettings settings = new SelectiveColorSettings();
    private SelectiveColorPass pass;

    public override void Create()
    {
        pass = new SelectiveColorPass(settings.colorLayer);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pass);
    }
}
