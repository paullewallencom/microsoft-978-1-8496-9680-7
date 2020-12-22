#pragma once

// A range for values of X and Y coordinates
#define SA3D_MIN_X_COORDINATE -25.0f
#define SA3D_MAX_X_COORDINATE 25.0f
#define SA3D_MIN_Y_COORDINATE -25.0f
#define SA3D_MAX_Y_COORDINATE 25.0f

// A minimum and maximum rotation speed for asteroids
#define SA3D_ASTEROID_ROTATE_SPEED_MIN -5.0f
#define SA3D_ASTEROID_ROTATE_SPEED_MAX 5.0f

// Paths to model files with data of the planet and the asteroid
#define SA3D_PLANET_MODEL_FILE L"Assets\\planet.obj"
#define SA3D_ASTEROID_MODEL_FILE L"Assets\\asteroid.obj"

// A number of points, which the user should got, to receive a bonus rocket
#define SA3D_NEW_ROCKET_BONUS 100000

// A number of bonus points for reaching the planet (it will be multiplied by the level number)
#define SA3D_REACH_PLANET_BONUS_BASE 1000

// A default number of rockets
#define SA3D_ROCKETS_NUMBER 3

// A minimum and maximum rocket speed
#define SA3D_MIN_SPEED 1.0f
#define SA3D_MAX_SPEED 50.0f

// A maximum number of asteroids per layer
#define SA3D_MAX_ASTEROIDS_PER_LAYER 10

// A distance filter, which makes it possible to simplify calculations and rendering,
// by skipping asteroids placed too far from the rocket
#define SA3D_ASTEROIDS_DISTANCE_FILTER 50.0f

// A path to the texture presenting the rocket display, as well as its width
#define SA3D_DISPLAY_TEXTURE_FILE L"Assets\\display.dds"
#define SA3D_DISPLAY_TEXTURE_WIDTH 1366

// A path to the sprite font used with the rocket display
#define SA3D_DISPLAY_FONT_FILE L"Assets\\display.spritefont"

// Margins (bottom and side) for the rocket display
#define SA3D_DISPLAY_MARGIN_BOTTOM 115.0f
#define SA3D_DISPLAY_MARGIN_SIDE 175.0f

// Maximum supported screen height and width
#define SA3D_MAX_SCREEN_HEIGHT 768.0f
#define SA3D_MAX_SCREEN_WIDTH 1280.0f

// A path and extension of the default file with translations
#define SA3D_LOCALIZED_STRINGS_PATH L"Assets\\LocalizedStrings."
#define SA3D_LOCALIZED_STRINGS_EXTENSION L"txt"

// Settings of the countdown feature - a delay, a default value, and a path to the sprite font file
#define SA3D_COUNTDOWN_DELAY 1.0f
#define SA3D_COUNTDOWN_NUMBER 3
#define SA3D_COUNTDOWN_FONT_FILE L"Assets\\countdown.spritefont"

// Settings of menus - paths to textures and the sprite font file
#define SA3D_MENU_BACKGROUND_FILE L"Assets\\menu-background.dds"
#define SA3D_MENU_ITEM_BACKGROUND_FILE L"Assets\\menu-item.dds"
#define SA3D_MENU_ITEM_FONT_FILE L"Assets\\menu.spritefont"

// A path to the file, where the current game state is saved
#define SA3D_SAVE_FILE L"save.txt"

// A length of the string with the current game state data
#define SA3D_SAVE_FILE_SIZE 20

// A path to the .wav file with a crash sound
#define SA3D_CRASH_SOUND_FILE L"Assets\\crash-sound.wav"

// Possible game states
enum SA3D_GAME_STATE
{
	SA3D_STATE_STARTING,
	SA3D_STATE_PLAYING,
	SA3D_STATE_RESULT,
	SA3D_STATE_PAUSE
};

// Possible actions (related to the managed part)
enum SA3D_ACTION
{
	SA3D_ACTION_NONE,
	SA3D_ACTION_BACK_TO_MENU,
	SA3D_ACTION_SEND_RESULT,
	SA3D_ACTION_RESTART
};

// Available menu items
enum SA3D_MENU_ITEM
{
	SA3D_MENU_NONE,
	SA3D_MENU_PAUSE_RESUME,
	SA3D_MENU_PAUSE_RESTART,
	SA3D_MENU_PAUSE_BACK,
	SA3D_MENU_RESULT_RESTART,
	SA3D_MENU_RESULT_SEND,
	SA3D_MENU_RESULT_BACK
};
