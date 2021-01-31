Shader "Wheel/WheelHighlight"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BorderColor("Border Color", Color) = (0,0,0,1)
        _ColorTint ("Color Tint", Color) = (1,1,1,0)
        _Tolerance("Tolerance", float) = 0.1
        
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

            fixed4 frag (v2f i) : SV_Target
            {

                fixed4 col = tex2D(_MainTex, i.uv);

                float2 v = i.uv;

                //high light
                float4 a = abs(col - _TargetColor);

                if(a.x + a.y + a.z + a.w < _Tolerance)
                {
                    col = col * _ColorTint ;
                }

                return col;
                //borders;

            }
            ENDCG
        }
    }
}
