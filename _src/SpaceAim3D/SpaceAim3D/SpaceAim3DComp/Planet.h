#pragma once
#include <vector>
#include "Object3D.h"
#include "GameHelpers.h"
#include "Constants.h"

using namespace std;

// The class representing the target planet
class Planet : public Object3D
{
public:
	// Initializes a new instance
	Planet();

	// Initializes the asteroid by setting a location (X, Y, Z coordinates) and rotation speeds
	void Initialize(int z);

	// Returns colors for vertices
	vector<XMFLOAT3> static GetColors();

private:
	// Colors for vertices
	vector<XMFLOAT3> static m_colors;
};
