Shader "Example/URPUnlitShaderBasic"
{
    Properties
    { }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
             
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"            

            struct Attributes
            {
                float4 vertex   : POSITION;
                float2 uv       : TEXCOORD0;
            };

            struct Varyings
            {
                float4 vertex   : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float2 uv       : TEXCOORD1;
            };            

            Varyings vert(Attributes i)
            {
                Varyings o;
                o.worldPos = mul (unity_ObjectToWorld, i.vertex);
                o.vertex = mul(UNITY_MATRIX_MVP, i.vertex);
                o.uv = i.uv;
                return o;
            }

            real SampleShadow(TEXTURE2D_SHADOW_PARAM(ShadowMap, sampler_ShadowMap), float4 shadowCoord)
            {
                real attenuation = real(SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, shadowCoord.xyz));
                return BEYOND_SHADOW_FAR(shadowCoord) ? 1.0 : attenuation;
            }
            
            half MainLightShadow(float4 shadowCoord)
            {
                return SampleShadow(TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_LinearClampCompare), shadowCoord);
            }
            
            half ShadowAtten(float3 worldPosition)
            {
                float4 shadowCoord = mul(_MainLightWorldToShadow[0], float4(worldPosition, 1.0));
                return MainLightShadow(shadowCoord);
            }

            half4 frag(Varyings i) : SV_Target
            {
                return ShadowAtten(i.worldPos);
            }
            ENDHLSL
        }
    }
}