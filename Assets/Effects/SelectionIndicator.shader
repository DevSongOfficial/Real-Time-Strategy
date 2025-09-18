Shader "URP/SelectionIndicator"
{
    Properties{
        _Color("Color", Color) = (0,1,0,1)
        _Inner("Inner Radius", Range(0,1)) = 0.35
        _Outer("Outer Radius", Range(0,1)) = 0.5
        _Feather("Feather", Range(0,0.2)) = 0.04
        _Speed("Spin Speed", Range(-10,10)) = 2
    }
    SubShader{
        Tags{ "RenderType"="Transparent" "Queue"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass{
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata { float4 vertex:POSITION; float2 uv:TEXCOORD0; };
            struct v2f { float4 pos:SV_POSITION; float2 uv:TEXCOORD0; };

            float4 _Color;
            float _Inner, _Outer, _Feather, _Speed;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                return o;
            }

            float ringMask(float2 uv, float inner, float outer, float feather, float angleOff)
            {
                float2 c = uv - 0.5;
                float r = length(c) * 2; 

                float ang = atan2(c.y, c.x) + _Time.y * _Speed + angleOff;

                float spark = 0.5 + 0.5 * cos(ang * 8);
                float a = smoothstep(inner, inner + feather, r) - smoothstep(outer, outer+feather, r);
                return saturate(a * (0.8 + 0.2 * spark));
            }

            half4 frag(v2f i) : SV_Target 
            {
                float m = ringMask(i.uv, _Inner, _Outer, _Feather, 0);
                return half4(_Color.rgb, _Color.a * m);
            }
            ENDHLSL
        }
    }
}
