#pragma once
#include "Constants.h"

using namespace DirectX;

// The class representing the rocket
class Rocket
{
public:
	// Initializes the rocket by setting a suitable location and speed
	void Initialize();

	// Flies for some distance
	float Fly(float distance);

	// Moves the rocket up, down, left, or right
	void Move(float x, float y);

	// Methods returning the current location of the rocket
	XMVECTOR GetLocation() { return XMVectorSet(m_X, m_Y, m_Z, 0); }
	XMFLOAT3 GetLocationSimple() { return XMFLOAT3(m_X, m_Y, m_Z); }

	// Methods returning current coordinates
	float GetX() { return m_X; }
	float GetY() { return m_Y; }
	float GetZ() { return m_Z; }

	// Methods for starting and stopping the additional engine
	void StartEngine() { m_isEngineRunning = true; }
	void StopEngine() { m_isEngineRunning = false; }

private:
	// X, Y, and Z coordinates of the rocket
	float m_X;
	float m_Y;
	float m_Z;

	// The current speed of the rocket
	float m_speed;

	// A value indicating whether the additional engine is currently running
	bool m_isEngineRunning;
};
