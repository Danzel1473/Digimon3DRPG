Shader "Custom/ToonTerrainShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ToonRamp ("Toon Ramp", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness ("Outline Thickness", Float) = 0.02
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        // First Pass: Toon Shading
        Pass
        {
            Name "ToonShading"
            Tags { "LightMode"="ForwardBase" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            sampler2D _ToonRamp;
            float4 _MainTex_ST;
            float4 _Color;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
                float NdotL = dot(i.worldNormal, worldLightDir);
                float toonShade = tex2D(_ToonRamp, float2(NdotL, 0.5)).r;

                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                col.rgb *= toonShade;

                return col;
            }
            ENDCG
        }

        // Second Pass: Outline
        Pass
        {
            Name "Outline"
            Tags { "LightMode"="Always" }

            Cull Front
            ZWrite On
            ZTest LEqual
            ColorMask RGB

            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vertOutline
            #pragma fragment fragOutline

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            float _OutlineThickness;
            float4 _OutlineColor;

            v2f vertOutline (appdata_t v)
            {
                // Extrude along normals
                v2f o;
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.vertex = UnityObjectToClipPos(v.vertex + float4(worldNormal * _OutlineThickness, 0));
                o.color = _OutlineColor;
                return o;
            }

            fixed4 fragOutline (v2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}