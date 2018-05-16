float4x4 modelMatrix;
float4x4 viewMatrix;
float4x4 projectionMatrix;

float4x4 lightSpaceMatrix;

float3 lightLocation;
float3 lightColor;
float lightAmbient;

float time;

texture albedoMap;
sampler2D textureSampler = sampler_state {
    Texture = (albedoMap);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

Texture2D shadowMap;

SamplerComparisonState cmpSampler : register(s1);

struct VertexShaderInput
{
	float3 vertexPosition : POSITION0;
	float2 vertexTexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 vertexPosition : POSITION0;
	float3 pixelPosition : POSITION1;
	float2 pixelTexCoord : TEXCOORD;
};
	

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	
	VertexShaderOutput output;
	
	output.vertexPosition = mul(mul(float4(input.vertexPosition, 1.0f), modelMatrix), viewMatrix);
	output.pixelPosition = output.vertexPosition.xyz;
	output.vertexPosition = mul(output.vertexPosition, projectionMatrix);
	
	output.pixelTexCoord = input.vertexTexCoord;
	
	return output;
	
}

float3 PixelShaderFunction(VertexShaderOutput input) : COLOR
{
	
	float3 textureColor = tex2D(textureSampler, input.pixelTexCoord).xyz;
	
	float3 fdx = ddx(input.pixelPosition);
	float3 fdy = ddy(input.pixelPosition);
 	float3 normal = normalize(cross(fdy, fdx));
	
	float3 lightDirection = normalize(lightLocation - input.pixelPosition);
	
	float4 shadowCoords = mul(float4(input.pixelPosition, 1.0f), lightSpaceMatrix);
	shadowCoords.xyz /= shadowCoords.w;
	shadowCoords.z -= 0.002f;
	
	shadowCoords.w = clamp((length(input.pixelPosition) - 40.0f) * 0.1f, 0.0f, 1.0f);
	shadowCoords.xy = shadowCoords.xy * 0.5f + 0.5f;
	shadowCoords.y = 1.0f - shadowCoords.y;
	float shadowFactor = shadowMap.SampleCmpLevelZero(cmpSampler, shadowCoords.xy, shadowCoords.z) + shadowCoords.w;
	
	if (shadowCoords.w > 0.999f)
		shadowFactor = 1.0f;

	// Specular lighting not necessary for now
	float3 ambient = textureColor.xyz * lightAmbient;
	float3 diffuse = max((dot(normal, lightDirection) * lightColor) * shadowFactor, ambient) * textureColor;
	
	float3 outColor = diffuse + ambient;
	
	return outColor;
	
}
	
	

technique Main
{
	pass Pass0
	{
		VertexShader = compile vs_5_0 VertexShaderFunction();
		PixelShader = compile ps_5_0 PixelShaderFunction();
	}
}