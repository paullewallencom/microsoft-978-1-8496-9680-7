#include "pch.h"
#include "CollisionDetector.h"

bool CollisionDetector::IsHit(XMFLOAT3 point, XMFLOAT3 sphere, float radius)
{
	float distance = sqrt((point.x - sphere.x) * (point.x - sphere.x) 
		+ (point.y - sphere.y) * (point.y - sphere.y) 
		+ (point.z - sphere.z) * (point.z - sphere.z));
	return distance <= radius;
}
