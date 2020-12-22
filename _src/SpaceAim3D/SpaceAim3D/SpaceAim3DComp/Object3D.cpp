#include "pch.h"
#include "Object3D.h"

Object3D::Object3D() : 
	m_indexCount(0),
	m_rotateSpeedX(0.0f),
	m_rotateSpeedY(0.0f), 
	m_rotateSpeedZ(0.0f), 
	m_scale(1.0f) { }

void Object3D::SetBuffers(ComPtr<ID3D11Buffer> vertexBuffer, ComPtr<ID3D11Buffer> indexBuffer, UINT indexCount)
{
	m_vertexBuffer = vertexBuffer;
	m_indexBuffer = indexBuffer;
	m_indexCount = indexCount;
}

void Object3D::Update(XMMATRIX *viewMatrix, float timeTotal)
{
	// Scale
	XMMATRIX scaleMatrix = XMMatrixScaling(m_scale, m_scale, m_scale);

	// Rotation
	float rotateX = m_rotateSpeedX == 0 ? 0 : timeTotal * XM_PIDIV4 / m_rotateSpeedX;
	float rotateY = m_rotateSpeedY == 0 ? 0 : timeTotal * XM_PIDIV4 / m_rotateSpeedY;
	float rotateZ = m_rotateSpeedZ == 0 ? 0 : timeTotal * XM_PIDIV4 / m_rotateSpeedZ;
	XMMATRIX rotationMatrix = XMMatrixRotationRollPitchYaw(rotateX, rotateY, rotateZ);

	// Translation
	XMMATRIX translationMatrix = XMMatrixTranslation(m_X, m_Y, m_Z);

	// Update the model and view matrices
	XMMATRIX scaleRotationMatrix = XMMatrixMultiply(scaleMatrix, rotationMatrix);
	XMMATRIX modelMatrix = XMMatrixMultiplyTranspose(scaleRotationMatrix, translationMatrix);
	XMStoreFloat4x4(&m_constantBuffer.model, modelMatrix);
	XMStoreFloat4x4(&m_constantBuffer.view, *viewMatrix);
}

void Object3D::UpdateProjectionMatrix(XMMATRIX *projectionMatrix)
{
	XMStoreFloat4x4(&m_constantBuffer.projection, *projectionMatrix);
}

void Object3D::Render(ComPtr<ID3D11DeviceContext1> deviceContext, ComPtr<ID3D11Buffer> constantBuffer)
{
	deviceContext->UpdateSubresource(constantBuffer.Get(), 0, NULL, &m_constantBuffer, 0, 0);
	deviceContext->DrawIndexed(m_indexCount, 0, 0);
}
