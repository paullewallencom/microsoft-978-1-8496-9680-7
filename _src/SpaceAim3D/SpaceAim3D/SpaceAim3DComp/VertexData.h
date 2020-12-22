#pragma once

using namespace DirectX;

// The structure representing data of the single vertex, i.e. its position and color
struct VertexData
{
public:
	XMFLOAT3 position;
	XMFLOAT3 color;
	void SetPosition(float x, float y, float z) { position = XMFLOAT3(x, y, z); }
	void SetColor(float r, float g, float b) { color = XMFLOAT3(r, g, b); }
	void SetColor(XMFLOAT3 c) { color = c; }
};