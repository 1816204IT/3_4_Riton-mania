Shader "Custom/RotationMask"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _OverTex("Base Texture (RGB)", 2D) = "white" {}
        _MaskTex("Mask Texture (RGB)", 2D) = "white" {}
        _RotationSpeed("Rotation Speed", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0
        
        struct Input
        {
            float2 uv_MaskTex;
            float2 uv_OverTex;
        };


        float _RotationSpeed;
        fixed4 _Color;
        sampler2D _OverTex;
        sampler2D _MaskTex;

        #define ANGLE (_Time.z * _RotationSpeed)

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 maskColor = tex2D(_MaskTex, IN.uv_MaskTex);
            clip(maskColor.r - 0.5); // do not draw if mask.r is less than 0.5
            
            fixed2 center = fixed2(0.5, 0.5);
            float2x2 rotate = float2x2(cos(ANGLE), -sin(ANGLE), sin(ANGLE), cos(ANGLE));
            fixed2 uv_OverTex = mul(rotate, IN.uv_OverTex - center) + center;
            fixed4 overColor = tex2D(_OverTex, uv_OverTex);


            fixed4 color = maskColor * overColor * _Color;
            o.Emission = color.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
