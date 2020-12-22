#include "pch.h"
#include "ModelLoader.h"

void ModelLoader::Load(wchar_t *filename, ComPtr<ID3D11Device1> device)
{
	ifstream stream;
	stream.open(filename);
	if (!stream.fail())
	{
		string type;
		char temp[256];
		while (!stream.eof())
		{
			stream >> type;
			if (type.compare("v") == 0)
			{
				// Load data of a vertex
				float x, y, z;
				stream >> x >> y >> z;
				LoadVertex(x, y, z);
			}
			else if (type.compare("f") == 0)
			{
				// Load data of a face
				int f1, f2, f3;
				stream >> f1 >> f2 >> f3;
				LoadFace(f1, f2, f3);
			}
			else
			{
				// Load the whole line and go to the next one
				stream.getline(temp, ARRAYSIZE(temp));
			}
		}

		stream.close();

		m_verticesCount = m_vertices.size();
		m_indicesCount = m_indices.size();
		SetVertexBuffer(device);
		SetIndexBuffer(device);
	}
}

void ModelLoader::LoadVertex(float x, float y, float z)
{
	XMFLOAT3 color(1.0f, 1.0f, 1.0f);
	if (!m_colors.empty())
	{
		int colorIndex = GameHelpers::RandInt(m_colors.size());
		color = m_colors[colorIndex];
	}

	unique_ptr<VertexData> vd(new VertexData());
	vd->SetPosition(x, y, z);
	vd->SetColor(color);
	m_vertices.push_back(*vd);
}

void ModelLoader::LoadFace(int f1, int f2, int f3)
{
	m_indices.push_back(f1-1);
	m_indices.push_back(f3-1);
	m_indices.push_back(f2-1);
}

void ModelLoader::SetVertexBuffer(ComPtr<ID3D11Device1> device)
{
	D3D11_SUBRESOURCE_DATA bufferData = { m_vertices.data(), 0, 0 };
	UINT bytes = sizeof(VertexData) * m_verticesCount;
	CD3D11_BUFFER_DESC bufferDesc = CD3D11_BUFFER_DESC(bytes, D3D11_BIND_VERTEX_BUFFER);
	ThrowIfFailed(device->CreateBuffer(&bufferDesc, &bufferData, &m_vertexBuffer));
}
void ModelLoader::SetIndexBuffer(ComPtr<ID3D11Device1> device)
{
	D3D11_SUBRESOURCE_DATA bufferData = { m_indices.data(), 0, 0 };
	UINT bytes = sizeof(unsigned short) * m_indicesCount;
	CD3D11_BUFFER_DESC bufferDesc = CD3D11_BUFFER_DESC(bytes, D3D11_BIND_INDEX_BUFFER);
	ThrowIfFailed(device->CreateBuffer(&bufferDesc, &bufferData, &m_indexBuffer));
}
