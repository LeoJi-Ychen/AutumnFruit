Shader "UI/BrightnessGlow"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        // 亮度控制（>1 变亮，<1 变暗）
        _Brightness ("Brightness", Float) = 1
        
        // 对比度
        _Contrast ("Contrast", Float) = 1
        
        // 饱和度
        _Saturation ("Saturation", Float) = 1
        
        // 外发光
        _GlowColor ("Glow Color", Color) = (1,1,0.5,1)
        _GlowIntensity ("Glow Intensity", Float) = 0
        _GlowSize ("Glow Size", Range(0,0.1)) = 0.02
        
        // 必需：Stencil 支持 UI Mask
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        
        _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        // UI Mask 支持
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "DEFAULT"
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_TexelSize;
            
            float _Brightness;
            float _Contrast;
            float _Saturation;
            
            fixed4 _GlowColor;
            float _GlowIntensity;
            float _GlowSize;

            // 饱和度调整函数
            fixed3 ApplySaturation(fixed3 color, float saturation)
            {
                fixed luminance = dot(color, fixed3(0.299, 0.587, 0.114));
                return lerp(fixed3(luminance, luminance, luminance), color, saturation);
            }

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                OUT.texcoord = v.texcoord;
                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                // 基础颜色采样
                half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
                
                // 外发光计算（采样周围像素）
                if (_GlowIntensity > 0 && _GlowSize > 0)
                {
                    float2 offset = _MainTex_TexelSize.xy * _GlowSize;
                    float glowAlpha = 0;
                    
                    // 8方向采样
                    glowAlpha += tex2D(_MainTex, IN.texcoord + float2(-offset.x, -offset.y)).a;
                    glowAlpha += tex2D(_MainTex, IN.texcoord + float2(0, -offset.y)).a;
                    glowAlpha += tex2D(_MainTex, IN.texcoord + float2(offset.x, -offset.y)).a;
                    glowAlpha += tex2D(_MainTex, IN.texcoord + float2(-offset.x, 0)).a;
                    glowAlpha += tex2D(_MainTex, IN.texcoord + float2(offset.x, 0)).a;
                    glowAlpha += tex2D(_MainTex, IN.texcoord + float2(-offset.x, offset.y)).a;
                    glowAlpha += tex2D(_MainTex, IN.texcoord + float2(0, offset.y)).a;
                    glowAlpha += tex2D(_MainTex, IN.texcoord + float2(offset.x, offset.y)).a;
                    glowAlpha /= 8;
                    
                    // 叠加发光
                    fixed3 glow = _GlowColor.rgb * glowAlpha * _GlowIntensity;
                    color.rgb += glow;
                }
                
                // 应用饱和度
                color.rgb = ApplySaturation(color.rgb, _Saturation);
                
                // 应用亮度（可以超过1）
                color.rgb *= _Brightness;
                
                // 应用对比度
                color.rgb = (color.rgb - 0.5) * _Contrast + 0.5;
                
                // UI Clip 支持
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                
                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                return color;
            }
            ENDCG
        }
    }
}
