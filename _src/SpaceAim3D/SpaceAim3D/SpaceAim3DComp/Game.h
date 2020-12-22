#pragma once
#include "Constants.h"

// The class representing the single game
class Game
{
public:
	// Initializes a new game
	Game();

	// Methods returning the level number, the score, and the number of rockets
	int GetLevel() { return m_level; }
	int GetScore() { return (int)m_score; }
	int GetRocketsNumber() { return m_rocketsNumber; }

	// Methods used to prepare a new level, by specifying the distance to the target planet, 
	// the speed factor, and the number of asteroids per layer (with the same z coordinate)
	int GetDistanceToPlanet() { return 100 + m_level * 10; }
	float GetSpeedFactor() { return min(SA3D_MIN_SPEED + 0.5f * m_level, SA3D_MAX_SPEED); }
	int GetAsteroidsPerLayer() { return min(max(m_level / 4, 1), SA3D_MAX_ASTEROIDS_PER_LAYER); }

	// Increases the score, depending on taken distance
	void IncreaseScore(float distance);

	// Called when the rocket reaches the target planet
	void ReachPlanet();

	// Called when the rocket crashes
	void CrashWithAsteroid();

	// Restarts the game
	void Restart();

	// Returns the current game state
	SA3D_GAME_STATE GetState() { return m_state; }

	// Methods used to set the current game state, the level number, 
	// the score, and the number of rockets
	void SetState(SA3D_GAME_STATE state) { m_state = state; }
	void SetLevel(int level) { m_level = level; }
	void SetScore(int score) { m_score = (float)score; }
	void SetRocketsNumber(int rockets) { m_rocketsNumber = rockets; }

private:
	// The level numer, the score, the number of rockets, and the current game state
	int m_level;
	float m_score;
	int m_rocketsNumber;
	SA3D_GAME_STATE m_state;

	// The method that checks whether a bonus rocket should be applied
	void AddBonusRocket(float newPoints);
};
