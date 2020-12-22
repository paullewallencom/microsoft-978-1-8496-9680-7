#pragma once

using namespace std;
using namespace DirectX;

// The structure, which stores three matrices - model, view, and projection ones
struct MVPConstantBuffer 
{ 
	XMFLOAT4X4 model; 
	XMFLOAT4X4 view; 
	XMFLOAT4X4 projection; 
};