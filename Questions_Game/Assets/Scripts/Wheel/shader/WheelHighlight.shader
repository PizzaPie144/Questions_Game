Shader "Wheel/WheelHighlight"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BorderColor("Border Color", Color) = (0,0,0,1)
        _ColorTint ("Color Tint", Color) = (1,1,1,0)
        
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
            

            fixed4 frag (v2f i) : SV_Target
            {

                fixed4 col = tex2D(_MainTex, i.uv);

                float2 v = i.uv;

                //high light
                float4 a = abs(col - _TargetColor);

                if(a.x + a.y + a.z + a.w < 0.1)
                {
                    col = col * _ColorTint ;
                }

                return col;
                //borders;

            }
            ENDCG
        }

        //GrabPass{}

        //Pass{

        //    Blend SrcAlpha OneMinusSrcAlpha
        //    CGPROGRAM
        //    #pragma vertex vert
        //    #pragma fragment frag

        //    #include "UnityCG.cginc"

        //     sampler2D _GrabTexture : register(s0);
        //    struct appdata
        //    {
        //        float4 vertex : POSITION;
        //        float2 uv : TEXCOORD0;
        //    };

        //    struct v2f
        //    {
        //        float2 uv : TEXCOORD0;
        //        float4 vertex : SV_POSITION;
        //    };

        //    v2f vert (appdata v)
        //    {
        //        v2f o;
        //        o.vertex = UnityObjectToClipPos(v.vertex);
        //        o.uv = v.uv;
        //        return o;
        //    }

        //    sampler2D _MainTex;
        //    fixed4 _BorderColor;
        //    fixed4 _TargetColor;
            
        //    float normpdf(float x, float sigma)
        //    {
        //        return 0.39894*exp(-0.5*x*x / (sigma*sigma)) / sigma;
        //    }

        //    half4 blur(sampler2D tex, float2 uv,float blurAmount) {
        //        half4 col = tex2D(tex, uv);
        //        const int mSize = 3;
        //        const int iter = (mSize - 1) / 2;
        //        for (int i = -iter; i <= iter; ++i) {
        //            for (int j = -iter; j <= iter; ++j) {
        //                col += tex2D(tex, float2(uv.x + i * blurAmount, uv.y + j * blurAmount)) * normpdf(float(i), 7);
        //            }
        //        }
        //        return col/mSize;
        //    }

        //    fixed4 frag (v2f i) : SV_Target
        //    {
        //        fixed4 col = tex2D(_GrabTexture, i.uv);
        //        col = blur(_GrabTexture, i.uv, 0.1);
        //        return col;

        //    }
        //    ENDCG
        //}
    }
}
