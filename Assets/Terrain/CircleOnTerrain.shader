Shader "Custom/CircleOnTerrain"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color   ("Ring Color", Color) = (1,1,1,1)
        _Center  ("Center (World XZ)", Vector) = (0,0,0,0)
        _Radius  ("Radius", Float) = 1
        _Border  ("Border", Float) = 0.1
    }
    SubShader
    {
        Tags{ "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        Pass
        {
            Name "ForwardUnlit"
            HLSLPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
            float4 _MainTex_ST; 

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _Center;   
                float  _Radius;
                float  _Border;
            CBUFFER_END

            struct Attributes { float4 positionOS : POSITION; float2 uv : TEXCOORD0; };
            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
                float3 positionWS  : TEXCOORD1;
            };

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionWS  = TransformObjectToWorld(v.positionOS.xyz);
                o.positionHCS = TransformWorldToHClip(o.positionWS);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex); 
                return o;
            }

            float4 frag (Varyings i) : SV_Target
            {
                float4 baseCol = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                float  dist = distance(i.positionWS.xz, _Center.xy);
                bool   inRing = (dist > _Radius) && (dist < _Radius + _Border);

                float4 col = inRing ? _Color : baseCol;
                col.a = baseCol.a; 
                return col;
            }
            ENDHLSL
        }
    }
}
