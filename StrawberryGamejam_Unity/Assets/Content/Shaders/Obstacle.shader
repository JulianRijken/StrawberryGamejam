// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Obstacle"
{
	Properties
	{
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[ASEBegin]_EdgeWith("EdgeWith", Float) = 0.47
		_FillAngle("FillAngle", Range( 0 , 360)) = 0
		_Rotation("Rotation", Float) = 2
		_MainTex("MainTex", 2D) = "white" {}
		[ASEEnd]_ExtraEdgeWith("ExtraEdgeWith", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }

		Cull Off
		HLSLINCLUDE
		#pragma target 2.0
		
		#pragma prefer_hlslcc gles
		#pragma exclude_renderers d3d11_9x 

		ENDHLSL

		
		Pass
		{
			Name "Sprite Lit"
			Tags { "LightMode"="Universal2D" }
			
			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZTest LEqual
			ZWrite Off
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM

			#define ASE_SRP_VERSION 110000


			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_0
			#pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_1
			#pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_2
			#pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_3

			#define _SURFACE_TYPE_TRANSPARENT 1
			#define SHADERPASS_SPRITELIT

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"
			
			#if USE_SHAPE_LIGHT_TYPE_0
			SHAPE_LIGHT(0)
			#endif

			#if USE_SHAPE_LIGHT_TYPE_1
			SHAPE_LIGHT(1)
			#endif

			#if USE_SHAPE_LIGHT_TYPE_2
			SHAPE_LIGHT(2)
			#endif

			#if USE_SHAPE_LIGHT_TYPE_3
			SHAPE_LIGHT(3)
			#endif

			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/CombinedShapeLightShared.hlsl"

			

			sampler2D _MainTex;
			CBUFFER_START( UnityPerMaterial )
			float4 _MainTex_ST;
			float _EdgeWith;
			float _ExtraEdgeWith;
			float _FillAngle;
			float _Rotation;
			CBUFFER_END


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 color : COLOR;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 texCoord0 : TEXCOORD0;
				float4 color : TEXCOORD1;
				float4 screenPosition : TEXCOORD2;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			#if ETC1_EXTERNAL_ALPHA
				TEXTURE2D(_AlphaTex); SAMPLER(sampler_AlphaTex);
				float _EnableAlphaTexture;
			#endif

			
			VertexOutput vert ( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.normal = v.normal;
				v.tangent.xyz = v.tangent.xyz;

				VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);

				o.texCoord0 = v.uv0;
				o.color = v.color;
				o.clipPos = vertexInput.positionCS;
				o.screenPosition = ComputeScreenPos( o.clipPos, _ProjectionParams.x );
				return o;
			}

			half4 frag ( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 uv_MainTex = IN.texCoord0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float2 appendResult11_g17 = (float2(1.0 , 1.0));
				float temp_output_17_0_g17 = length( ( (IN.texCoord0.xy*2.0 + -1.0) / appendResult11_g17 ) );
				float temp_output_131_0 = ( ( _EdgeWith * 2.0 ) + _ExtraEdgeWith );
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float clampResult127 = clamp( ( 1.0 - ( temp_output_131_0 / ase_objectScale.x ) ) , 0.0 , 1.0 );
				float clampResult128 = clamp( ( 1.0 - ( temp_output_131_0 / ase_objectScale.y ) ) , 0.0 , 1.0 );
				float2 appendResult11_g16 = (float2(clampResult127 , clampResult128));
				float temp_output_17_0_g16 = length( ( (IN.texCoord0.xy*2.0 + -1.0) / appendResult11_g16 ) );
				float cos10 = cos( ( ( ( _Rotation + 180.0 ) - ( _FillAngle / 2.0 ) ) * ( PI / 180.0 ) ) );
				float sin10 = sin( ( ( ( _Rotation + 180.0 ) - ( _FillAngle / 2.0 ) ) * ( PI / 180.0 ) ) );
				float2 rotator10 = mul( IN.texCoord0.xy - float2( 0.5,0.5 ) , float2x2( cos10 , -sin10 , sin10 , cos10 )) + float2( 0.5,0.5 );
				float2 break17 = (rotator10*float2( 2,2 ) + float2( -1,-1 ));
				float4 appendResult50 = (float4(tex2D( _MainTex, uv_MainTex ).rgb , ( ( saturate( ( ( 1.0 - temp_output_17_0_g17 ) / fwidth( temp_output_17_0_g17 ) ) ) - ( saturate( ( ( 1.0 - temp_output_17_0_g16 ) / fwidth( temp_output_17_0_g16 ) ) ) * 1.0 ) ) * floor( ( ( _FillAngle / 360.0 ) + (0.0 + (atan2( break17.x , break17.y ) - ( PI * -1.0 )) * (1.0 - 0.0) / (PI - ( PI * -1.0 ))) ) ) )));
				
				float4 Color = appendResult50;
				float4 Mask = float4(1,1,1,1);
				float3 Normal = float3( 0, 0, 1 );

				#if ETC1_EXTERNAL_ALPHA
					float4 alpha = SAMPLE_TEXTURE2D(_AlphaTex, sampler_AlphaTex, IN.texCoord0.xy);
					Color.a = lerp ( Color.a, alpha.r, _EnableAlphaTexture);
				#endif
				
				Color *= IN.color;
			#if ASE_SRP_VERSION >= 120000
				SurfaceData2D surfaceData;
				InitializeSurfaceData(Color.rgb, Color.a, Mask, surfaceData);
				InputData2D inputData;
				InitializeInputData(IN.texCoord0.xy, half2(IN.screenPosition.xy / IN.screenPosition.w), inputData);
				return CombinedShapeLightShared(surfaceData, inputData);
			#else
				return CombinedShapeLightShared( Color, Mask, IN.screenPosition.xy / IN.screenPosition.w );
			#endif
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "Sprite Normal"
			Tags { "LightMode"="NormalsRendering" }
			
			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZTest LEqual
			ZWrite Off
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM

			#define ASE_SRP_VERSION 110000


			#pragma vertex vert
			#pragma fragment frag

			#define _SURFACE_TYPE_TRANSPARENT 1
			#define SHADERPASS_SPRITENORMAL

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/NormalsRenderingShared.hlsl"
			
			

			sampler2D _MainTex;
			CBUFFER_START( UnityPerMaterial )
			float4 _MainTex_ST;
			float _EdgeWith;
			float _ExtraEdgeWith;
			float _FillAngle;
			float _Rotation;
			CBUFFER_END


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 color : COLOR;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 texCoord0 : TEXCOORD0;
				float4 color : TEXCOORD1;
				float3 normalWS : TEXCOORD2;
				float4 tangentWS : TEXCOORD3;
				float3 bitangentWS : TEXCOORD4;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			
			VertexOutput vert ( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.normal = v.normal;
				v.tangent.xyz = v.tangent.xyz;

				VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);

				o.texCoord0 = v.uv0;
				o.color = v.color;
				o.clipPos = vertexInput.positionCS;

				float3 normalWS = TransformObjectToWorldNormal( v.normal );
				o.normalWS = NormalizeNormalPerVertex( normalWS );
				float4 tangentWS = float4( TransformObjectToWorldDir( v.tangent.xyz ), v.tangent.w );
				o.tangentWS = normalize( tangentWS );
				o.bitangentWS = cross( normalWS, tangentWS.xyz ) * tangentWS.w;
				return o;
			}

			half4 frag ( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 uv_MainTex = IN.texCoord0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float2 appendResult11_g17 = (float2(1.0 , 1.0));
				float temp_output_17_0_g17 = length( ( (IN.texCoord0.xy*2.0 + -1.0) / appendResult11_g17 ) );
				float temp_output_131_0 = ( ( _EdgeWith * 2.0 ) + _ExtraEdgeWith );
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float clampResult127 = clamp( ( 1.0 - ( temp_output_131_0 / ase_objectScale.x ) ) , 0.0 , 1.0 );
				float clampResult128 = clamp( ( 1.0 - ( temp_output_131_0 / ase_objectScale.y ) ) , 0.0 , 1.0 );
				float2 appendResult11_g16 = (float2(clampResult127 , clampResult128));
				float temp_output_17_0_g16 = length( ( (IN.texCoord0.xy*2.0 + -1.0) / appendResult11_g16 ) );
				float cos10 = cos( ( ( ( _Rotation + 180.0 ) - ( _FillAngle / 2.0 ) ) * ( PI / 180.0 ) ) );
				float sin10 = sin( ( ( ( _Rotation + 180.0 ) - ( _FillAngle / 2.0 ) ) * ( PI / 180.0 ) ) );
				float2 rotator10 = mul( IN.texCoord0.xy - float2( 0.5,0.5 ) , float2x2( cos10 , -sin10 , sin10 , cos10 )) + float2( 0.5,0.5 );
				float2 break17 = (rotator10*float2( 2,2 ) + float2( -1,-1 ));
				float4 appendResult50 = (float4(tex2D( _MainTex, uv_MainTex ).rgb , ( ( saturate( ( ( 1.0 - temp_output_17_0_g17 ) / fwidth( temp_output_17_0_g17 ) ) ) - ( saturate( ( ( 1.0 - temp_output_17_0_g16 ) / fwidth( temp_output_17_0_g16 ) ) ) * 1.0 ) ) * floor( ( ( _FillAngle / 360.0 ) + (0.0 + (atan2( break17.x , break17.y ) - ( PI * -1.0 )) * (1.0 - 0.0) / (PI - ( PI * -1.0 ))) ) ) )));
				
				float4 Color = appendResult50;
				float3 Normal = float3( 0, 0, 1 );
				
				Color *= IN.color;

				return NormalsRenderingShared( Color, Normal, IN.tangentWS.xyz, IN.bitangentWS, IN.normalWS);
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "Sprite Forward"
			Tags { "LightMode"="UniversalForward" }

			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZTest LEqual
			ZWrite Off
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM

			#define ASE_SRP_VERSION 110000


			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA

			#define _SURFACE_TYPE_TRANSPARENT 1
			#define SHADERPASS_SPRITEFORWARD

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

			

			sampler2D _MainTex;
			CBUFFER_START( UnityPerMaterial )
			float4 _MainTex_ST;
			float _EdgeWith;
			float _ExtraEdgeWith;
			float _FillAngle;
			float _Rotation;
			CBUFFER_END


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 color : COLOR;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 texCoord0 : TEXCOORD0;
				float4 color : TEXCOORD1;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			#if ETC1_EXTERNAL_ALPHA
				TEXTURE2D( _AlphaTex ); SAMPLER( sampler_AlphaTex );
				float _EnableAlphaTexture;
			#endif

			
			VertexOutput vert( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.normal = v.normal;

				VertexPositionInputs vertexInput = GetVertexPositionInputs( v.vertex.xyz );

				o.texCoord0 = v.uv0;
				o.color = v.color;
				o.clipPos = vertexInput.positionCS;

				return o;
			}

			half4 frag( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 uv_MainTex = IN.texCoord0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float2 appendResult11_g17 = (float2(1.0 , 1.0));
				float temp_output_17_0_g17 = length( ( (IN.texCoord0.xy*2.0 + -1.0) / appendResult11_g17 ) );
				float temp_output_131_0 = ( ( _EdgeWith * 2.0 ) + _ExtraEdgeWith );
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				float clampResult127 = clamp( ( 1.0 - ( temp_output_131_0 / ase_objectScale.x ) ) , 0.0 , 1.0 );
				float clampResult128 = clamp( ( 1.0 - ( temp_output_131_0 / ase_objectScale.y ) ) , 0.0 , 1.0 );
				float2 appendResult11_g16 = (float2(clampResult127 , clampResult128));
				float temp_output_17_0_g16 = length( ( (IN.texCoord0.xy*2.0 + -1.0) / appendResult11_g16 ) );
				float cos10 = cos( ( ( ( _Rotation + 180.0 ) - ( _FillAngle / 2.0 ) ) * ( PI / 180.0 ) ) );
				float sin10 = sin( ( ( ( _Rotation + 180.0 ) - ( _FillAngle / 2.0 ) ) * ( PI / 180.0 ) ) );
				float2 rotator10 = mul( IN.texCoord0.xy - float2( 0.5,0.5 ) , float2x2( cos10 , -sin10 , sin10 , cos10 )) + float2( 0.5,0.5 );
				float2 break17 = (rotator10*float2( 2,2 ) + float2( -1,-1 ));
				float4 appendResult50 = (float4(tex2D( _MainTex, uv_MainTex ).rgb , ( ( saturate( ( ( 1.0 - temp_output_17_0_g17 ) / fwidth( temp_output_17_0_g17 ) ) ) - ( saturate( ( ( 1.0 - temp_output_17_0_g16 ) / fwidth( temp_output_17_0_g16 ) ) ) * 1.0 ) ) * floor( ( ( _FillAngle / 360.0 ) + (0.0 + (atan2( break17.x , break17.y ) - ( PI * -1.0 )) * (1.0 - 0.0) / (PI - ( PI * -1.0 ))) ) ) )));
				
				float4 Color = appendResult50;

				#if ETC1_EXTERNAL_ALPHA
					float4 alpha = SAMPLE_TEXTURE2D( _AlphaTex, sampler_AlphaTex, IN.texCoord0.xy );
					Color.a = lerp( Color.a, alpha.r, _EnableAlphaTexture );
				#endif

				Color *= IN.color;

				return Color;
			}

			ENDHLSL
		}
		
	}
	CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=18933
1920;0;1920;1019;1560.398;593.6788;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;122;-3838.189,176.0936;Inherit;False;4305.497;1051.335;Comment;22;119;29;99;102;11;100;10;13;14;12;20;17;22;21;18;104;19;28;30;123;130;129;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;119;-3207.665,791.4631;Inherit;False;202;185;Half The Fill Angle;1;118;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;130;-3045.683,654.5223;Inherit;False;Constant;_RotationOffset;RotationOffset;4;0;Create;True;0;0;0;False;0;False;180;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-3785.189,241.0936;Inherit;False;Property;_FillAngle;FillAngle;1;0;Create;True;0;0;0;False;0;False;0;0.5;0;360;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;123;-3009.779,541.0636;Inherit;False;Property;_Rotation;Rotation;2;0;Create;True;0;0;0;False;0;False;2;180;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;118;-3157.665,841.4631;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;129;-2735.683,635.5223;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;99;-2839.889,1029.229;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;121;-2259.874,-685.1027;Inherit;False;2552.739;701.2682;Get Edge;16;35;94;70;71;76;75;69;31;32;34;127;128;132;131;133;134;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;124;-2639.342,836.4696;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;102;-2649.084,1022.323;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;180;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-1925.132,-362.7009;Inherit;False;Property;_EdgeWith;EdgeWith;0;0;Create;True;0;0;0;False;0;False;0.47;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;11;-2434.774,508.462;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;100;-2378.787,890.3722;Inherit;False;2;2;0;FLOAT;1801;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;-1697.317,-348.9182;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;132;-1750.799,-190.6783;Inherit;False;Property;_ExtraEdgeWith;ExtraEdgeWith;4;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;14;-1856.035,835.7239;Inherit;False;Constant;_Vector2;Vector 2;0;0;Create;True;0;0;0;False;0;False;-1,-1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RotatorNode;10;-2138.598,628.223;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;13;-1855.036,701.7238;Inherit;False;Constant;_Vector1;Vector 1;0;0;Create;True;0;0;0;False;0;False;2,2;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.ObjectScaleNode;70;-1510.426,-189.0444;Inherit;False;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;131;-1544.799,-322.6784;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;12;-1650.491,632.9779;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;2.53,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;76;-1249.988,-133.4051;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;71;-1256.401,-349.1281;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;75;-1099.506,-132.5005;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;69;-1096.222,-343.7512;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;17;-1374.316,632.9003;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;22;-799.1736,848.6693;Inherit;False;Constant;_Float0;Float 0;0;0;Create;True;0;0;0;False;0;False;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;20;-793.1739,716.6719;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;128;-902.3019,-153.8912;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-581.1751,759.6719;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;127;-904.3019,-337.8913;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ATan2OpNode;18;-1136.099,634.7903;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;19;-414.7009,638.9255;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;31;-683.8432,-303.9809;Inherit;True;Ellipse;-1;;16;3ba94b7b3cfd5f447befde8107c04d52;0;3;2;FLOAT2;0,0;False;7;FLOAT;0;False;9;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;104;-291.5394,258.5625;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;360;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;134;-365.3978,-184.6788;Inherit;False;Constant;_Float2;Float 2;5;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-17.46966,449.0269;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;32;-518.6674,-628.6028;Inherit;True;Ellipse;-1;;17;3ba94b7b3cfd5f447befde8107c04d52;0;3;2;FLOAT2;0,0;False;7;FLOAT;1;False;9;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;133;-216.3978,-311.6788;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;30;317.1595,445.6603;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;34;58.16473,-433.9749;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;638.675,144.8825;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;39;572.0798,-289.2863;Inherit;True;Property;_MainTex;MainTex;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;50;1094.519,-109.4709;Inherit;False;COLOR;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;2;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;12;New Amplify Shader;199187dac283dbe4a8cb1ea611d70c58;True;Sprite Normal;0;1;Sprite Normal;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;2;5;False;-1;10;False;-1;3;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=NormalsRendering;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;3;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;12;New Amplify Shader;199187dac283dbe4a8cb1ea611d70c58;True;Sprite Forward;0;2;Sprite Forward;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;2;5;False;-1;10;False;-1;3;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;1387.055,-121.249;Float;False;True;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;12;Obstacle;199187dac283dbe4a8cb1ea611d70c58;True;Sprite Lit;0;0;Sprite Lit;6;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;2;5;False;-1;10;False;-1;3;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=Universal2D;False;False;0;Hidden/InternalErrorShader;0;0;Standard;1;Vertex Position;1;0;0;3;True;True;True;False;;False;0
WireConnection;118;0;29;0
WireConnection;129;0;123;0
WireConnection;129;1;130;0
WireConnection;124;0;129;0
WireConnection;124;1;118;0
WireConnection;102;0;99;0
WireConnection;100;0;124;0
WireConnection;100;1;102;0
WireConnection;94;0;35;0
WireConnection;10;0;11;0
WireConnection;10;2;100;0
WireConnection;131;0;94;0
WireConnection;131;1;132;0
WireConnection;12;0;10;0
WireConnection;12;1;13;0
WireConnection;12;2;14;0
WireConnection;76;0;131;0
WireConnection;76;1;70;2
WireConnection;71;0;131;0
WireConnection;71;1;70;1
WireConnection;75;0;76;0
WireConnection;69;0;71;0
WireConnection;17;0;12;0
WireConnection;128;0;75;0
WireConnection;21;0;20;0
WireConnection;21;1;22;0
WireConnection;127;0;69;0
WireConnection;18;0;17;0
WireConnection;18;1;17;1
WireConnection;19;0;18;0
WireConnection;19;1;21;0
WireConnection;19;2;20;0
WireConnection;31;7;127;0
WireConnection;31;9;128;0
WireConnection;104;0;29;0
WireConnection;28;0;104;0
WireConnection;28;1;19;0
WireConnection;133;0;31;0
WireConnection;133;1;134;0
WireConnection;30;0;28;0
WireConnection;34;0;32;0
WireConnection;34;1;133;0
WireConnection;37;0;34;0
WireConnection;37;1;30;0
WireConnection;50;0;39;0
WireConnection;50;3;37;0
WireConnection;1;1;50;0
ASEEND*/
//CHKSM=2F0495A7DF47122D82C4B76F65F39F94DB318553