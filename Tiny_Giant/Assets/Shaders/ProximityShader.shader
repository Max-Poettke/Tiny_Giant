Shader "Custom/VertexOpacityShader"
{
    Properties
    {
        _PlacementPos ("Placement Position", Vector) = (0,0,0)
        _MaxDist ("Max Distance", Float) = 5.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float opacity : TEXCOORD0;
            };

            float3 _PlacementPos;
            float _MaxDist;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                float dist = distance(UnityObjectToClipPos(v.vertex), _PlacementPos);
                o.opacity = saturate(1.0 - dist / _MaxDist);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = fixed4(1,1,1,i.opacity); // Example color
                return col;
            }
            ENDCG
        }
    }
}
