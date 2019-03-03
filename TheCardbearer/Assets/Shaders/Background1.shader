Shader "Lit/Diffuse With Shadows"
{
    Properties
    {
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            Tags {"LightMode"="ForwardBase"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            // compile shader into multiple variants, with and without shadows
            // (we don't care about any lightmaps yet, so skip these variants)
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            // shadow helper functions and macros
            #include "AutoLight.cginc"

            struct v2f
            {
                float2 uv : TEXCOORD0;
                SHADOW_COORDS(1) // put shadows data into TEXCOORD1
                fixed3 diff : COLOR0;
                fixed3 ambient : COLOR1;
                float4 pos : SV_POSITION;
            };
			
            v2f vert (appdata_base v)
            {
                v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
                
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                
				half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                
				o.diff = nl * _LightColor0;
				
                o.ambient = ShadeSH9(half4(worldNormal,1));
				
				o.diff.rgb += ShadeSH9(half4(worldNormal,1));
				
                // compute shadows data
                TRANSFER_SHADOW(o)
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
                fixed shadow = SHADOW_ATTENUATION(i);
                // darken light's illumination with shadow, keep ambient intact
                fixed3 lighting = i.diff * shadow + i.ambient;
                col.rgb *= lighting;
				
				
				
				float2 c = i.uv;
                c = c / 2;
				
				fixed4 output = fixed4(0.2, 0.2, 0.3, 0);	// dark background
				
				
				float2 ring = ( (2.*c) - float2(1, 1) );
				
				float len = 10.*length(ring) + 5.*_Time;
				
				float func = (0.4 - (len % 2)/4) - ((i.uv.x + i.uv.y)/4);
				
				if (func % 0.1 > 0 && func % 0.1 < (sin(_Time*0.75).y + 3)/60)
				{
					output = output + fixed4(0, 0.2, 0, 0);		// green spiral
				}
				
				
				float eq1 = (i.uv.x + i.uv.y + sin(_Time*0.2).y + 10);
				float eq2 = (sin(_Time).y + 1.5)/20;
				
				float eq3 = (i.uv.x - i.uv.y + sin((_Time*0.2) + 3.14/2).y + 10);
				
				if (eq1 % 0.4 < eq2 && eq1 % 0.4 > eq2 - 0.01)
				{
					output = output + fixed4(0, 0.2, 0.2, 0);	// blue line
				}
				
				if (eq3 % 0.3 < eq2 && eq3 % 0.3 > eq2 - 0.01)
				{
					output = output + fixed4(0.2, 0.2, 0, 0);	// orange line
				}
				
				
                return output * col;
            }
            ENDCG
        }

        // shadow casting support
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}


