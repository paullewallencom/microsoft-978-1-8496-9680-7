#include "pch.h"
#include "Asteroid.h"

vector<XMFLOAT3> Asteroid::m_colors;

Asteroid::Asteroid() : Object3D()
{
}

void Asteroid::Initialize(int z)
{
	m_X = GameHelpers::RandFloat(SA3D_MIN_X_COORDINATE, SA3D_MAX_X_COORDINATE);
	m_Y = GameHelpers::RandFloat(SA3D_MIN_Y_COORDINATE, SA3D_MAX_Y_COORDINATE);
	m_Z = (float)-z;
	m_rotateSpeedX = GameHelpers::RandFloat(SA3D_ASTEROID_ROTATE_SPEED_MIN, SA3D_ASTEROID_ROTATE_SPEED_MAX);
	m_rotateSpeedY = GameHelpers::RandFloat(SA3D_ASTEROID_ROTATE_SPEED_MIN, SA3D_ASTEROID_ROTATE_SPEED_MAX);
	m_rotateSpeedZ = GameHelpers::RandFloat(SA3D_ASTEROID_ROTATE_SPEED_MIN, SA3D_ASTEROID_ROTATE_SPEED_MAX);
}

vector<XMFLOAT3> Asteroid::GetColors()
{
	if (m_colors.empty())
	{
		m_colors.push_back(GameHelpers::GetColor(250, 230, 125));
		m_colors.push_back(GameHelpers::GetColor(220, 160, 50));
		m_colors.push_back(GameHelpers::GetColor(150, 70, 15));
	}
	return m_colors;
}
