// UVスクロール
// プレイシーンの背景に使用している六角形の画像に使用しています。
Shader "Custom/MoveMask"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _OverTex("Base Texture (RGB)", 2D) = "white" {}
        _MaskTex("Mask Texture (RGB)", 2D) = "white" {}
        _MoveSpeed("MoveSpeed", Float) = 1.0
    }
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent"
        }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard alpha:fade
        #pragma target 3.0

        struct Input
        {
            float2 uv_MaskTex;
            float2 uv_OverTex;
        };

        float _MoveSpeed;
        fixed4 _Color;
        sampler2D _OverTex;
        sampler2D _MaskTex;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // Mask画像のUVを回転させる
            fixed2 center = fixed2(0.5, 0.5);
            float2x2 rotate = float2x2(cos(3.14 / 4), -sin(3.14 / 4), sin(3.14 / 4), cos(3.14 / 4));
            fixed2 uv_MaskTex = mul(rotate, IN.uv_MaskTex - center) + center;

            // Mask画像のUVを移動させる 
            uv_MaskTex.x += _MoveSpeed * ((_Time.y % 2.0) - 1.0);
            uv_MaskTex.y += _MoveSpeed * ((_Time.y % 2.0) - 1.0);

            // 画像の色を取得
            fixed4 overColor = tex2D(_OverTex, IN.uv_OverTex);
            float grayScale = overColor.r * 0.3 + overColor.g * 0.6 + overColor.b * 0.1;
            o.Alpha = (overColor.a == 0) ? 0 : 1;

            // Mask画像の色を取得
            fixed4 maskColor = tex2D(_MaskTex, uv_MaskTex);

            // Maskの白部分の場合
            if (maskColor.r > 0.01)
            {
                o.Emission = overColor.rgb * _Color;
            }
            // Maskの黒部分の場合}
            else
            {
                o.Emission = fixed3(0.1, 0.1, 0.1);
            }
        }
        ENDCG
    }
        FallBack "Diffuse"
}
