Shader "Custom/VisualSettings"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Brightness ("Brightness", Range(-1, 1)) = 0
        _Contrast ("Contrast", Range(0, 2)) = 1
        _Saturation ("Saturation", Range(-1, 1)) = 0
    }
    SubShader
    {
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Brightness;
            float _Contrast;
            float _Saturation;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                half4 color = tex2D(_MainTex, i.uv);

                // Apply brightness (multiplying instead of adding)
                color.rgb *= (1 + _Brightness);

                // Apply contrast (only modify RGB, not alpha)
                color.rgb = (color.rgb - 0.5) * _Contrast + 0.5;

                // Apply saturation (only modify RGB, not alpha)
                half3 luminance = dot(color.rgb, half3(0.299, 0.587, 0.114));
                color.rgb = lerp(luminance.xxx, color.rgb, 1 + _Saturation);

                // Clamp RGB values to the range [0, 1]
                color.rgb = saturate(color.rgb);

                return color;
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}
