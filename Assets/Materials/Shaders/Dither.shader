Shader "Effects/Dither"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Dither("Dither", 2D) = "white" {}
        _Noise("Noise", 2D) = "white" {}
        _ColorRamp("Color Ramp", 2D) = "white" {}
        _TL("Direction", Vector) = (0.0, 0.0, 0.0, 0.0)
        _BL("Direction", Vector) = (0.0, 0.0, 0.0, 0.0)
        _TR("Direction", Vector) = (0.0, 0.0, 0.0, 0.0)
        _BR("Direction", Vector) = (0.0, 0.0, 0.0, 0.0)
        _Tiling("Tiling", Float) = 192.0
        _PixelSize("Pixel Size", Range(1, 12)) = 3
        _DepthFalloffDistance("Depth Falloff Distance", Range(0.5, 100)) = 20
        _PatternScale("Pattern Scale", Range(1, 8)) = 1
        _Contrast("Contrast", Range(0.5, 2.5)) = 1.15
        _UseColorRamp("Use Color Ramp", Range(0, 1)) = 0
        _DarkColor("Dark Color", Color) = (0, 0, 0, 1)
        _LightColor("Light Color", Color) = (0.95, 0.95, 0.9, 1)
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
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
        #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
        ENDHLSL

        Pass
        {
            Name "Dither"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            TEXTURE2D(_Dither);
            SAMPLER(sampler_Dither);
            TEXTURE2D(_Noise);
            SAMPLER(sampler_Noise);
            TEXTURE2D(_ColorRamp);

            CBUFFER_START(UnityPerMaterial)
                float4 _Dither_TexelSize;
                float4 _Noise_TexelSize;
                float4 _ColorRamp_TexelSize;
                float4 _BL;
                float4 _TL;
                float4 _TR;
                float4 _BR;
                float _Tiling;
                float _PixelSize;
                float _DepthFalloffDistance;
                float _PatternScale;
                float _Contrast;
                float _UseColorRamp;
                float4 _DarkColor;
                float4 _LightColor;
            CBUFFER_END

            half4 SampleSource(float2 uv)
            {
                return SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_PointClamp, uv);
            }

            float2 QuantizeScreenUV(float2 uv, float pixelSize)
            {
                float2 screenPixels = uv * _BlitTexture_TexelSize.zw;
                float2 quantizedPixels = floor(screenPixels / pixelSize) * pixelSize + 0.5 * pixelSize;
                return quantizedPixels * _BlitTexture_TexelSize.xy;
            }

            float GetLinearSceneDepth(float2 uv)
            {
                float rawDepth = SampleSceneDepth(uv);
                return unity_OrthoParams.w == 0 ? LinearEyeDepth(rawDepth, _ZBufferParams) : LinearDepthToEyeDepth(rawDepth);
            }

            float GetDistancePixelSize(float2 uv, float pixelSize)
            {
                float linearDepth = GetLinearSceneDepth(uv);
                float falloff = saturate(linearDepth / max(_DepthFalloffDistance, 0.0001));
                return lerp(pixelSize, 1.0, falloff);
            }

            float SampleScreenDither(float2 uv, float pixelSize, float patternScale)
            {
                float2 screenPixels = uv * _BlitTexture_TexelSize.zw;
                float2 cellIndex = floor(screenPixels / pixelSize);
                float2 patternIndex = floor(cellIndex / patternScale);
                float2 ditherTexel = fmod(patternIndex, _Dither_TexelSize.zw);
                float2 ditherUV = (ditherTexel + 0.5) * _Dither_TexelSize.xy;

                return SAMPLE_TEXTURE2D_LOD(_Dither, sampler_PointClamp, ditherUV, 0).r;
            }

            half3 SamplePalette(half isLit)
            {
                if (_UseColorRamp > 0.5)
                {
                    float rampIndex = isLit > 0.5h ? (_ColorRamp_TexelSize.z - 1.0) : 0.0;
                    float rampU = (rampIndex + 0.5) * _ColorRamp_TexelSize.x;
                    return SAMPLE_TEXTURE2D_LOD(_ColorRamp, sampler_PointClamp, float2(rampU, 0.5), 0).rgb;
                }

                return isLit > 0.5h ? _LightColor.rgb : _DarkColor.rgb;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float2 uv = input.texcoord;
                float pixelSize = GetDistancePixelSize(uv, max(_PixelSize, 1.0));
                float patternScale = max(_PatternScale, 1.0);
                float2 sourceUV = QuantizeScreenUV(uv, pixelSize);
                half3 color = SampleSource(sourceUV).rgb;

                half lum = dot(color, half3(0.299h, 0.587h, 0.114h));
                lum = saturate((lum - 0.5h) * _Contrast + 0.5h);

                half threshold = SampleScreenDither(uv, pixelSize, patternScale);
                half isLit = lum > threshold ? 1.0h : 0.0h;
                half3 output = SamplePalette(isLit);
                return half4(output, 1.0h);
            }
            ENDHLSL
        }
    }
}
