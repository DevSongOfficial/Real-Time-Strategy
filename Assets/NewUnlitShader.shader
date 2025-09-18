Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _FlowTex ("Flow Texture", 2D) = "white" {}
        _UVTex ("UV Texture", 2D) = "white" {}
        _FlowSpeed ("Flow Speed", Vector) = (0, 0, 0, 0)

        [Enum(UnityEngine.Rendering.BlendMode)]
        _SrcFactor("Source Factor", Float) = 5       // 5: BlendMode.SrcAlpha
        [Enum(UnityEngine.Rendering.BlendMode)]
        _DstFactor("Destination Factor", Float) = 10 // 10: BlendMode.OneMinusSrcAlpha
        [Enum(UnityEngine.Rendering.BlendOp)]
        _Opp("Operation", Float) = 0                 // 0: BlendOp.Add
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100


        Blend [_SrcFactor] [_DstFactor]
        BlendOp [_Opp]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };


            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _FlowTex;
            sampler2D _UVTex;
            float4 _FlowSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.zw = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 uv = tex2D(_UVTex, i.uv.xy);
                uv.rg += frac(_Time.y * _FlowSpeed.xy);

                fixed4 flow = tex2D(_FlowTex, uv.rg) * uv.a;
                fixed4 col  = tex2D(_MainTex, i.uv.xy) * (1-uv.a);

                return flow + col;
            }

            ENDCG
        }
    }
}