Shader "Custom/GlowMain"
{
    Properties
    {
        _BaseColor("BaseColor", Color) = (1, 1, 1, 1)
        _MainTex("Base (RGB)", 2D) = "white" {}
        _RimColor("Rim Color", Color) = (1, 1, 1, 1)
        _RimPower("Rim Power", Range(1, 4.0)) = 1.0

    }
    SubShader
    {

        Tags{ "RenderType" = "Opaque" }

        CGPROGRAM
        #pragma surface surf Lambert

        struct Input {
            float4 color : Color;
            float2 uv_MainTex;
            float3 viewDir;
        };

        float4 _BaseColor, _RimColor;
        float _RimPower;
        sampler2D _MainTex;

        void surf(Input IN, inout SurfaceOutput o)
        {
            IN.color = _BaseColor;
            half4 color = tex2D(_MainTex, IN.uv_MainTex);
            half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));

            o.Albedo = color.rgb * IN.color;
            if (color[0] != 1 || color[1] != 1 || color[2] != 1) 
            {
                o.Emission = _BaseColor;
            }
            else 
            {
                o.Emission = _RimColor.rgb * pow(rim, _RimPower / 2.0);
            }
        }
        ENDCG
    }
    FallBack "Diffuse"
}