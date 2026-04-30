Shader "Custom/AtkisonDither"
{
    Properties
    {
        _Dither ("Dither Pattern", 2D) = "white" {}
        _ColorRamp ("Color Ramp", 2D) = "white" {}
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
        }

        Cull Off
        ZWrite Off
        ZTest Always

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
        ENDHLSL

        Pass
        {
            Name "AtkinsonDither"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            TEXTURE2D(_Dither);
            SAMPLER(sampler_Dither);
            TEXTURE2D(_ColorRamp);
            SAMPLER(sampler_ColorRamp);

            CBUFFER_START(UnityPerMaterial)
                float4 _Dither_TexelSize;
            CBUFFER_END

            half4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                half3 color = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, input.texcoord).rgb;
                half luminance = dot(color, half3(0.299h, 0.587h, 0.114h));

                float2 ditherUV = input.texcoord * _BlitTexture_TexelSize.zw * _Dither_TexelSize.xy;
                half ditherLuminance = SAMPLE_TEXTURE2D(_Dither, sampler_Dither, ditherUV).r;

                half ramp = luminance <= clamp(ditherLuminance, 0.1h, 0.9h) ? 0.1h : 0.9h;
                half3 output = SAMPLE_TEXTURE2D(_ColorRamp, sampler_ColorRamp, float2(ramp, 0.5)).rgb;
                return half4(output, 1.0h);
            }
            ENDHLSL
        }
    }
}
