Shader "OmNomCook/OutlineText"
{
	Properties{
		[PerRendererData] _MainTex("Font Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[Toggle(OUTLINE_ON)]_Outline("Outline", float) = 1
		_OutlineColor("OutlineColor", Color) = (1,1,1,1)
		_OutlineWidth("Outline Width", float) = 1
		_Drop("Drop", float) = 1

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
	}


	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Stencil
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask[_ColorMask]

	//Up Right
	Pass
	{
			Name "Outline"
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile_local _ UNITY_UI_CLIP_RECT
			#pragma multi_compile_local _ UNITY_UI_ALPHACLIP

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
				fixed4 color : COLOR;
				float2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _MainTex;
			fixed4 _Color;
			fixed4 _TextureSampleAdd;
			float4 _ClipRect;
			float4 _MainTex_ST;
			half4 _OutlineColor;
			half _OutlineWidth;

			v2f vert(appdata_t v)
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				OUT.worldPosition = v.vertex + _OutlineWidth;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

				OUT.color = v.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * _OutlineColor;

				#ifdef UNITY_UI_CLIP_RECT
				color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
				#endif

				#ifdef UNITY_UI_ALPHACLIP
				clip(color.a - 0.001);
				#endif

				return color * IN.color;
			}
		ENDCG
		}

	//Down Left & Down Right
	/*
		//Down Left		
		Pass
		{
			Name "Outline1"
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile_local _ UNITY_UI_CLIP_RECT
			#pragma multi_compile_local _ UNITY_UI_ALPHACLIP

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
				fixed4 color : COLOR;
				float2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _MainTex;
			fixed4 _Color;
			fixed4 _TextureSampleAdd;
			float4 _ClipRect;
			float4 _MainTex_ST;
			half4 _OutlineColor;
			half _OutlineWidth;

			v2f vert(appdata_t v)
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				OUT.worldPosition = v.vertex - _OutlineWidth;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

				OUT.color = v.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * _OutlineColor;

				#ifdef UNITY_UI_CLIP_RECT
				color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
				#endif

				#ifdef UNITY_UI_ALPHACLIP
				clip(color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
//Down Right
		Pass
		{
			Name "Outline1"
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile_local _ UNITY_UI_CLIP_RECT
			#pragma multi_compile_local _ UNITY_UI_ALPHACLIP

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
				fixed4 color : COLOR;
				float2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _MainTex;
			fixed4 _Color;
			fixed4 _TextureSampleAdd;
			float4 _ClipRect;
			float4 _MainTex_ST;
			half4 _OutlineColor;
			half _OutlineWidth;

			v2f vert(appdata_t v)
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				v.vertex.x += _OutlineWidth;
				v.vertex.y -= _OutlineWidth;

				OUT.worldPosition = v.vertex;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

				OUT.color = v.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * _OutlineColor;

				#ifdef UNITY_UI_CLIP_RECT
				color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
				#endif

				#ifdef UNITY_UI_ALPHACLIP
				clip(color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
*/

	//Up Left
	Pass
	{
		Name "Outline1"
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 2.0

		#include "UnityCG.cginc"
		#include "UnityUI.cginc"

		#pragma multi_compile_local _ UNITY_UI_CLIP_RECT
		#pragma multi_compile_local _ UNITY_UI_ALPHACLIP

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
			fixed4 color : COLOR;
			float2 texcoord  : TEXCOORD0;
			float4 worldPosition : TEXCOORD1;
			UNITY_VERTEX_OUTPUT_STEREO
		};

		sampler2D _MainTex;
		fixed4 _Color;
		fixed4 _TextureSampleAdd;
		float4 _ClipRect;
		float4 _MainTex_ST;
		half4 _OutlineColor;
		half _OutlineWidth;

		v2f vert(appdata_t v)
		{
			v2f OUT;
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
			v.vertex.x -= _OutlineWidth;
			v.vertex.y += _OutlineWidth;

			OUT.worldPosition = v.vertex;
			OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

			OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

			OUT.color = v.color * _Color;
			return OUT;
		}

		fixed4 frag(v2f IN) : SV_Target
		{
			half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * _OutlineColor;

			#ifdef UNITY_UI_CLIP_RECT
			color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
			#endif

			#ifdef UNITY_UI_ALPHACLIP
			clip(color.a - 0.001);
			#endif

			return color * IN.color;
		}
	ENDCG
	}

	//Left & Right
	/*
//Left
		Pass
		{
					Name "OutlineH"
				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma target 2.0

					#include "UnityCG.cginc"
					#include "UnityUI.cginc"

					#pragma multi_compile_local _ UNITY_UI_CLIP_RECT
					#pragma multi_compile_local _ UNITY_UI_ALPHACLIP

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
						fixed4 color : COLOR;
						float2 texcoord  : TEXCOORD0;
						float4 worldPosition : TEXCOORD1;
						UNITY_VERTEX_OUTPUT_STEREO
					};

					sampler2D _MainTex;
					fixed4 _Color;
					fixed4 _TextureSampleAdd;
					float4 _ClipRect;
					float4 _MainTex_ST;
					half4 _OutlineColor;
					half _OutlineWidth;

					v2f vert(appdata_t v)
					{
						v2f OUT;
						UNITY_SETUP_INSTANCE_ID(v);
						UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
						v.vertex.x -= _OutlineWidth;

						OUT.worldPosition = v.vertex;
						OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

						OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

						OUT.color = v.color * _Color;
						return OUT;
					}

					fixed4 frag(v2f IN) : SV_Target
					{
						half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * _OutlineColor;

						#ifdef UNITY_UI_CLIP_RECT
						color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
						#endif

						#ifdef UNITY_UI_ALPHACLIP
						clip(color.a - 0.001);
						#endif

						return color;
					}
				ENDCG
		}
//Right
		Pass
		{
					Name "OutlineH"
				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma target 2.0

					#include "UnityCG.cginc"
					#include "UnityUI.cginc"

					#pragma multi_compile_local _ UNITY_UI_CLIP_RECT
					#pragma multi_compile_local _ UNITY_UI_ALPHACLIP

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
						fixed4 color : COLOR;
						float2 texcoord  : TEXCOORD0;
						float4 worldPosition : TEXCOORD1;
						UNITY_VERTEX_OUTPUT_STEREO
					};

					sampler2D _MainTex;
					fixed4 _Color;
					fixed4 _TextureSampleAdd;
					float4 _ClipRect;
					float4 _MainTex_ST;
					half4 _OutlineColor;
					half _OutlineWidth;

					v2f vert(appdata_t v)
					{
						v2f OUT;
						UNITY_SETUP_INSTANCE_ID(v);
						UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
						v.vertex.x += _OutlineWidth;

						OUT.worldPosition = v.vertex;
						OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

						OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

						OUT.color = v.color * _Color;
						return OUT;
					}

					fixed4 frag(v2f IN) : SV_Target
					{
						half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * _OutlineColor;

						#ifdef UNITY_UI_CLIP_RECT
						color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
						#endif

						#ifdef UNITY_UI_ALPHACLIP
						clip(color.a - 0.001);
						#endif

						return color;
					}
				ENDCG
		}

			*/


	//Drop Left
	Pass
	{
		Name "Drop-H"
	CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 2.0

		#include "UnityCG.cginc"
		#include "UnityUI.cginc"

		#pragma multi_compile_local _ UNITY_UI_CLIP_RECT
		#pragma multi_compile_local _ UNITY_UI_ALPHACLIP

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
			fixed4 color : COLOR;
			float2 texcoord  : TEXCOORD0;
			float4 worldPosition : TEXCOORD1;
			UNITY_VERTEX_OUTPUT_STEREO
		};

		sampler2D _MainTex;
		fixed4 _Color;
		fixed4 _TextureSampleAdd;
		float4 _ClipRect;
		float4 _MainTex_ST;
		half4 _OutlineColor;
		half _OutlineWidth;
		half _Drop;

		v2f vert(appdata_t v)
		{
			v2f OUT;
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
			v.vertex.y += _Drop;
			v.vertex.x -= _OutlineWidth;

			OUT.worldPosition = v.vertex;
			OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

			OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

			OUT.color = v.color * _Color;
			return OUT;
		}

		fixed4 frag(v2f IN) : SV_Target
		{
			half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * _OutlineColor;

			#ifdef UNITY_UI_CLIP_RECT
			color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
			#endif

			#ifdef UNITY_UI_ALPHACLIP
			clip(color.a - 0.001);
			#endif

			return color * IN.color;
		}
	ENDCG
	}

	//Drop Right
	Pass
	{
		Name "Drop+H"
	CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 2.0

		#include "UnityCG.cginc"
		#include "UnityUI.cginc"

		#pragma multi_compile_local _ UNITY_UI_CLIP_RECT
		#pragma multi_compile_local _ UNITY_UI_ALPHACLIP

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
			fixed4 color : COLOR;
			float2 texcoord  : TEXCOORD0;
			float4 worldPosition : TEXCOORD1;
			UNITY_VERTEX_OUTPUT_STEREO
		};

		sampler2D _MainTex;
		fixed4 _Color;
		fixed4 _TextureSampleAdd;
		float4 _ClipRect;
		float4 _MainTex_ST;
		half4 _OutlineColor;
		half _OutlineWidth;
		half _Drop;

		v2f vert(appdata_t v)
		{
			v2f OUT;
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
			v.vertex.y += _Drop;
			v.vertex.x += _OutlineWidth;

			OUT.worldPosition = v.vertex;
			OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

			OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

			OUT.color = v.color * _Color;
			return OUT;
		}

		fixed4 frag(v2f IN) : SV_Target
		{
			half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * _OutlineColor;

			#ifdef UNITY_UI_CLIP_RECT
			color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
			#endif

			#ifdef UNITY_UI_ALPHACLIP
			clip(color.a - 0.001);
			#endif

			return color * IN.color;
		}
	ENDCG
	}

	//Drop
	/*Pass
	{
			Name "Drop"
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile_local _ UNITY_UI_CLIP_RECT
			#pragma multi_compile_local _ UNITY_UI_ALPHACLIP

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
				fixed4 color : COLOR;
				float2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _MainTex;
			fixed4 _Color;
			fixed4 _TextureSampleAdd;
			float4 _ClipRect;
			float4 _MainTex_ST;
			half4 _OutlineColor;
			half _Drop;

			v2f vert(appdata_t v)
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				v.vertex.y += _Drop;

				OUT.worldPosition = v.vertex;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

				OUT.color = v.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * _OutlineColor;

				#ifdef UNITY_UI_CLIP_RECT
				color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
				#endif

				#ifdef UNITY_UI_ALPHACLIP
				clip(color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
*/
	
	//Front
	Pass
	{
			Name "Front"
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile_local _ UNITY_UI_CLIP_RECT
			#pragma multi_compile_local _ UNITY_UI_ALPHACLIP

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
				fixed4 color : COLOR;
				float2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _MainTex;
			fixed4 _Color;
			fixed4 _TextureSampleAdd;
			float4 _ClipRect;
			float4 _MainTex_ST;
			half4 _OutlineColor;

			v2f vert(appdata_t v)
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				OUT.worldPosition = v.vertex;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

				OUT.color = v.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
				/*half4 outline = (tex2D(_MainTex, IN.texcoord + 0.03) + _TextureSampleAdd) * _OutlineColor;
				outline *= 1 - color.a;
				color.rgb *= color.a;
				color += outline;*/

				#ifdef UNITY_UI_CLIP_RECT
				color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
				#endif

				#ifdef UNITY_UI_ALPHACLIP
				clip(color.a - 0.001);
				#endif

				return color;
			}
				ENDCG
		}
	}
	FallBack "UI/Default Font"
}