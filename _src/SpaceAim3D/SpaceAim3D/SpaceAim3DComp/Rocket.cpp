#include "pch.h"
#include "Rocket.h"

void Rocket::Initialize()
{
	m_X = 0.0f;
	m_Y = 0.0f;
	m_Z = 5.0f;
	m_speed = 1.0f;
}

float Rocket::Fly(float distance)
{
	if (m_isEngineRunning) 
	{ 
		// Increase the speed, if the additional engine is running
		m_speed += 0.1f; 
	}
	else
	{
		// Decrease the speed, if the additional engine is not running
		m_speed -= 0.1f;

		// Ensure that the current speed is no lower than 1.0f
		m_speed = max(m_speed, 1.0f);
	}

	// Return the taken distance
	float takenDistance = distance * m_speed;
	m_Z -= takenDistance;
	return takenDistance;
}

void Rocket::Move(float x, float y)
{ 
	m_X += x;
	m_Y += y; 
	m_X = max(m_X, SA3D_MIN_X_COORDINATE);
	m_X = min(m_X, SA3D_MAX_X_COORDINATE);
	m_Y = max(m_Y, SA3D_MIN_Y_COORDINATE);
	m_Y = min(m_Y, SA3D_MAX_Y_COORDINATE);
}
