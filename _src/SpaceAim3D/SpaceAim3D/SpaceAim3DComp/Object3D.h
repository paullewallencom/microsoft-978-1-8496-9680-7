#pragma once
#include "MVPConstantBuffer.h"
#include "VertexData.h"

using namespace std;
using namespace Microsoft::WRL;

// The class representing an object that can be placed in the game world
class Object3D
{
public:
	// Sets default settings for the object
	Object3D();

	// Set vertex and index buffers for the object, as well as a number of indices
	virtual void SetBuffers(ComPtr<ID3D11Buffer> vertexBuffer, ComPtr<ID3D11Buffer> indexBuffer, UINT indexCount);

	// Initializes the object
	// It is a purely virtual method
	virtual void Initialize(int z) = 0;

	// Updates the object
	virtual void Update(XMMATRIX *viewMatrix, float timeTotal);

	// Updates the projection matrix for the object
	virtual void UpdateProjectionMatrix(XMMATRIX *projectionMatrix);

	// Renders the object
	virtual void Render(ComPtr<ID3D11DeviceContext1> deviceContext, ComPtr<ID3D11Buffer> constantBuffer);

	// Methods returning coordinates, scale, and location of the object
	float GetX() { return m_X; }
	float GetY() { return m_Y; }
	float GetZ() { return m_Z; }
	float GetScale() { return m_scale; }
	XMFLOAT3 GetLocation()  { return XMFLOAT3(m_X, m_Y, m_Z); }

protected:
	// X, Y, and Z coordinates of the object
	float m_X;
	float m_Y;
	float m_Z;

	// Rotation speeds and scale of the object
	float m_rotateSpeedX;
	float m_rotateSpeedY;
	float m_rotateSpeedZ;
	float m_scale;

	// Vertex and index buffers
	ComPtr<ID3D11Buffer> m_vertexBuffer;
	ComPtr<ID3D11Buffer> m_indexBuffer;

	// Constant buffer
	MVPConstantBuffer m_constantBuffer;

	// A number of indices
	UINT m_indexCount;
};
