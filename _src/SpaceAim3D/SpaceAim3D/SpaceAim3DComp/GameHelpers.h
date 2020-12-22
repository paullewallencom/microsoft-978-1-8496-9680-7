#pragma once
#include <string>

using namespace std;
using namespace DirectX;

// The class containing auxiliary methods for the native part of the game
class GameHelpers
{
public:
	// Returns a random integer value smaller than the given one
	int static RandInt(int max);

	// Returns a floating-point number in range <0.0, 1.0)
	float static RandFloat();

	// Returns a floating-point number in range <minValue, maxValue)
	float static RandFloat(float minValue, float maxValue);

	// Returns the XMFLOAT3 instance based on R, G, and B parts representing a color
	XMFLOAT3 static GetColor(byte r, byte g, byte b);

	// Writes a floating-point value to the debug output
	void static Debug(float value);

	// Writes three floating-point values to the debug output
	void static Debug(float x, float y, float z);
};
