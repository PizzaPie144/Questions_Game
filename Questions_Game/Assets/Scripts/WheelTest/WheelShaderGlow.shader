Shader "Wheel/WheelGlow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BorderColor("Border Color", Color) = (0,0,0,1)
        _ColorTint ("Color Tint", Color) = (1,1,1,0)
        _Tolerance("Tolerance", float) = 0.1
        _Glow ("Glow", float) = 2
        _AlphaIntensity_Fade_1("AlpaInteneisity_1",float) = 0
        _AlphaIntensity_Fade_2("AlpaInteneisity_2",float) = 0

        _TintRGBA_Color_1 ("Tint 1",Color) = (0,0,0,0)
        _TintRGBA_Color_2 ("Tint 2",Color) = (0,0,0,0)

        _OperationBlend_Fade_1 ("blend", float) = 0
        _SpriteFade("fade",float) =0


        
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "ForceNoShadowCasting" = "True"}
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off  		
			Lighting OFF

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            fixed4 _BorderColor;
            fixed4 _TargetColor;
            fixed4 _ColorTint;
            float _Tolerance;
            float _Glow;
            float _AlphaIntensity_Fade_1;
            float _AlphaIntensity_Fade_2;

            float4 _TintRGBA_Color_1;
            float4 _TintRGBA_Color_2;

            float _OperationBlend_Fade_1;
            float _SpriteFade;


            float4 TintRGBA(float4 txt, float4 color)
            {
                float3 tint = dot(txt.rgb, float3(.222, .707, .071));
                tint.rgb *= color.rgb;
                txt.rgb = lerp(txt.rgb,tint.rgb,color.a);
                return txt;
            }

            float4 OperationBlend(float4 origin, float4 overlay, float blend)
            {
                float4 o = origin; 
                o.a = overlay.a + origin.a * (1 - overlay.a);
                o.rgb = (overlay.rgb * overlay.a + origin.rgb * origin.a * (1 - overlay.a)) / (o.a+0.0000001);
                o.a = saturate(o.a);
                o = lerp(origin, o, blend);
                return o;
            }

            float4 AlphaIntensity(float4 txt,float fade)
            {
                if (txt.a < 1) txt.a = lerp(0, txt.a, fade);
                return txt;
            }


            fixed4 frag (v2f i) : SV_Target
            {
                //fixed4 col = tex2D(_MainTex, i.uv);
                //float2 v = i.uv;
                //float4 a = abs(col - _TargetColor);

                //if(a.x + a.y + a.z + a.w < _Tolerance)
                //{
                //    col = col * _ColorTint ;
                //}
                
                float4 _MainTex_1 = tex2D(_MainTex, i.uv);
                float4 AlphaIntensity_1 = AlphaIntensity(_MainTex_1,_AlphaIntensity_Fade_1);
                float4 TintRGBA_1 = TintRGBA(AlphaIntensity_1,_TintRGBA_Color_1);
                float4 _MainTex_2 = tex2D(_MainTex, i.uv);
                float4 AlphaIntensity_2 = AlphaIntensity(_MainTex_2,_AlphaIntensity_Fade_2);
                float4 TintRGBA_2 = TintRGBA(AlphaIntensity_2,_TintRGBA_Color_2);
                float4 OperationBlend_1 = OperationBlend(TintRGBA_2, TintRGBA_1, _OperationBlend_Fade_1); 
                float4 FinalResult = OperationBlend_1;
                FinalResult.rgb *= _MainTex_1.rgb;

                FinalResult.a = FinalResult.a * _SpriteFade * _MainTex_1.a;
                return FinalResult;

            }
            ENDCG
        }

        GrabPass{ "_FlatTexture"}

        Pass{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            

            struct v2f
            {
                float2 grabPos : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.grabPos = ComputeGrabScreenPos(o.pos);
                return o;
            }

            sampler2D _FlatTexture;
            fixed4 _BorderColor;
            fixed4 _TargetColor;
            float _Glow;

            fixed4 frag (v2f i) : SV_Target
            {

                fixed4 col = tex2D(_FlatTexture, i.grabPos);

                return col;
                //borders;

            }
            ENDCG
        }
    }
}
