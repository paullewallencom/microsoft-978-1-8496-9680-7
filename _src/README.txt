+==========================================================================+
|                          Space Aim 3D source code                        |
+==========================================================================+
|  The Space Aim 3D game is created as an example to the "Windows Phone 8  |
|    Game Development" book published by Packt Publishing Ltd. in 2013     |
+==========================================================================+
|             Instruction how to run the project, version 1.0              |
+--------------------------------------------------------------------------+
|   Author: Marcin Jamro (marcin@jamro.biz) | Website: http://jamro.biz    |
+==========================================================================+

Let's start
----------------------------------------------------------------------------
At the beginning, you should launch the Microsoft Visual Studio Express 2012 
for Windows Phone and open the "SpaceAim3D" project. 

If there is a message "load failed" next to the "SpaceAim3D" project, 
you can choose the "Reload Project" option from the context menu. 

You should also ensure that "SpaceAim3D" (not "SpaceAim3DComp") is chosen 
as the startup project. In such a case, its name will be bold. If it is not,
you can set it as the startup project by choosing the "Set as StartUp Project" 
option from the context menu.

Adding DirectXTK
----------------------------------------------------------------------------
The game uses the DirectXTK library, which is not included in this .zip file.
For this reason, you need to download the DirectXTK library and add it to 
the project, exactly in the same way as described at the beginning 
of Chapter 7 (2D User Interface in 3D Game). You should not forget to add 
a reference in the "SpaceAim3DComp" project, as well as configure 
the additional include directory.

"External" Directory in the Native Part
----------------------------------------------------------------------------
You should copy six files (from the "XAudio2 audio file playback sample" - 
http://code.msdn.microsoft.com/Basic-Audio-Sample-9a5bb0b7) to the 
"External" directory in the "SpaceAim3DComp" project, as described 
in Chapter 11 (Improving Game Experience). It is worth mentioning that you 
need to include these files in the project, by choosing a suitable option 
from the context menu. If it is necessary, you should also modify them, 
as suggested in the chapter. Now, you should be able to successfully rebuild 
the "SpaceAim3DComp" project.

Configuring the Web Service
----------------------------------------------------------------------------
Please do not close the Microsoft Visual Studio Express 2012 for Windows 
Phone and launch the Microsoft Visual Studio Express 2012 for Web 
(with administrator priviliges). Then, you should load 
the "SpaceAim3D.WebService" project and deploy it locally. In case of 
any problems, please refer to Chapter 9 (Exchanging Data via Web Services) 
and its subchapter named "Web Service Deployment".

Now, let's come back to the Microsoft Visual Studio Express 2012 for Windows 
Phone and add a service reference in a way described in subchapter 
"Web service usage" inside Chapter 9 (Exchanging Data via Web Services).
You should not forget to add a rule in the Windows Firewall, to be able
to connect to the web service.

Installing Libraries using NuGet Package Manager
----------------------------------------------------------------------------
The managed part of the project uses three libraries that can be downloaded 
using the NuGet package manager, namely "Facebook", "linqtotwitter", 
and "WPtoolkit". A way how you can install their current versions 
is described in Chapter 8 (Maps, Geolocation, and Augmented Reality) 
and Chapter 10 (Social Networks, Feeds, Settings, and Local Rank). 

Adding GART library
----------------------------------------------------------------------------
The application is equipped with the augmented reality feature,
which uses the GART library (Geo Augmented Reality Toolkit). Thus, you need 
to add it to the project, as described in Chapter 8 (Maps, Geolocation, 
and Augmented Reality).

Background music
----------------------------------------------------------------------------
The downloaded version of the project do not contain the .mp3 file played 
as the background music. For this reason, you need to copy the .mp3 file, 
which you want to listen to while playing the game, to the "Assets" directory 
inside the SpaceAim3D project (SpaceAim3D\SpaceAim3D\SpaceAim3D\Assets).

Map credentials
---------------
Please refer to Chapter 12 (Game Publishing) to learn how to obtain the 
Map service ApplicationID and Map service AuthenticationToken values. 
Then, you should copy them as values of MAP_APPLICATION_ID 
and MAP_AUTHENTICATION_TOKEN constants inside the MapViewModel class 
(SpaceAim3D\SpaceAim3D\SpaceAim3D\ViewModels\MapViewModel.cs).

Facebook and Twitter 
--------------------
A description how to create Facebook and Twitter application, as well as 
get necessary data to support them in the game, are availalble 
in Chapter 10 (Social Networks, Feeds, Settings, and Local Rank). 
Thus, let's copy necessary information as values of FB_APP_ID, 
TW_CONSUMERKEY, and TW_CONSUMERSECRET constants inside the WebViewModel 
class (SpaceAim3D\SpaceAim3D\SpaceAim3D\ViewModels\WebViewModel.cs).

That is all :-)
----------------------------------------------------------------------------
Now, you can rebuild the project and run it in the emulator or on the phone.

If you receive "error LNK1104: cannot open file '[path]\SpaceAim3D\
Debug\SpaceAim3DComp\SpaceAim3DComp.winmd'" while rebuilding the project, 
just restart the IDE and try to build it once again.