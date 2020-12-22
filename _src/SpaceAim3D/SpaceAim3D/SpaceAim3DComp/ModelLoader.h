#pragma once
#include <fstream>
#include <string>
#include <vector>
#include "DirectXHelper.h"
#include "VertexData.h"
#include "GameHelpers.h"

using namespace std;
using namespace DirectX;
using namespace DX;
using namespace Microsoft::WRL;

// The class used for loading models
class ModelLoader
{
public:
	// Loads data of vertices and indices from the .obj file
	void Load(wchar_t *filename, ComPtr<ID3D11Device1> device);

	// Sets available colors for vertices
	void SetColors(vector<XMFLOAT3> colors) { m_colors = colors; }

	// Returns a number of indices
	int GetIndicesCount() { return m_indicesCount; }

	// Methods returning vertex and index buffers
	ComPtr<ID3D11Buffer> GetVertexBuffer() { return m_vertexBuffer; }
	ComPtr<ID3D11Buffer> GetIndexBuffer() { return m_indexBuffer; }

private:
	// Methods creating vertex and index buffers
	void SetVertexBuffer(ComPtr<ID3D11Device1> device);
	void SetIndexBuffer(ComPtr<ID3D11Device1> device);

	// Load vertex and face data
	void LoadVertex(float x, float y, float z);
	void LoadFace(int f1, int f2, int f3);

	// Fields storing vertices, indices, as well as their numbers
	vector<VertexData> m_vertices;
	vector<unsigned short> m_indices;
	int m_verticesCount;
	int m_indicesCount;

	// Available colors for vertices
	vector<XMFLOAT3> m_colors;

	// Vertex and index buffers
	ComPtr<ID3D11Buffer> m_vertexBuffer;
	ComPtr<ID3D11Buffer> m_indexBuffer;
};
