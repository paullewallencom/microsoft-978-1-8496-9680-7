#include "pch.h"
#include "Planet.h"

vector<XMFLOAT3> Planet::m_colors;

Planet::Planet() : Object3D()
{
}

void Planet::Initialize(int z)
{
	m_X = GameHelpers::RandFloat(SA3D_MIN_X_COORDINATE, SA3D_MAX_X_COORDINATE);
	m_Y = GameHelpers::RandFloat(SA3D_MIN_Y_COORDINATE, SA3D_MAX_Y_COORDINATE);
	m_Z = (float)-z;
	m_scale = 10.0f;
	m_rotateSpeedX = m_rotateSpeedY = m_rotateSpeedZ = 1.0f;
}

vector<XMFLOAT3> Planet::GetColors()
{
	if (m_colors.empty())
	{
		m_colors.push_back(GameHelpers::GetColor(170, 50, 0));
		m_colors.push_back(GameHelpers::GetColor(210, 85, 30));
		m_colors.push_back(GameHelpers::GetColor(205, 125, 60));
		m_colors.push_back(GameHelpers::GetColor(250, 135, 50));
		m_colors.push_back(GameHelpers::GetColor(210, 130, 65));
		m_colors.push_back(GameHelpers::GetColor(195, 95, 35));
	}
	return m_colors;
}
