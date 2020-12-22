#include "pch.h"
#include "Game.h"

Game::Game() 
{ 
	Restart(); 
}

void Game::IncreaseScore(float distance)
{
	float newPoints = distance * m_level;
	AddBonusRocket(newPoints);
	m_score += newPoints;
}

void Game::ReachPlanet()
{
	int newPoints = SA3D_REACH_PLANET_BONUS_BASE * m_level;
	AddBonusRocket((float)newPoints);
	m_score += newPoints;
	m_level++;
}

void Game::CrashWithAsteroid()
{
	if (m_rocketsNumber > 0) 
	{ 
		m_rocketsNumber--; 
	}
}

void Game::Restart()
{
	m_level = 1;
	m_score = 0;
	m_rocketsNumber = SA3D_ROCKETS_NUMBER;
	m_state = SA3D_STATE_STARTING;
}

void Game::AddBonusRocket(float newPoints)
{
	float newScore = m_score + newPoints;
	if ((int)m_score % SA3D_NEW_ROCKET_BONUS > (int)newScore % SA3D_NEW_ROCKET_BONUS)
	{
		m_rocketsNumber++;
	}
}