#pragma once

using namespace DirectX;

// The class used for detecting collisions between objects.
class CollisionDetector
{
public:
	// Returns a value indicating whether there is a collision 
	// between the point and the sphere with a particular radius
	bool static IsHit(XMFLOAT3 point, XMFLOAT3 sphere, float radius);
};
