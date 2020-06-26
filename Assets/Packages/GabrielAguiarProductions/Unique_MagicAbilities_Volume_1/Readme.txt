Unique Magic Abilities Volume 1 - Readme


This package contains:

- 20 Magic Abilities;
- 4 Projectiles Prefabs;
- 4 Hits/Impacts Prefabs;
- PC Demo Scene;
- Mobile Demo Scene;
- Customizable Shaders;
- Particle System Controller Script (control size, speed, color, lights, trails, enable/disable vfxs, etc);
- Projectile Script (control fire rate, accuracy, fire point, etc);
- Comet Script (control start point, end point, radius, delay, rate of fire, quantity and waves of comets);
- Extra Textures for customization.




DEMO SCENE - SHORTCUTS:

Mouse 1 - Spawn Ability
D - Next Ability
A - Previous Ability
C - Change Camera
Z - Zoom In
X - Zoom Out
1 - Enable/Disable Camera Shake




PARTICLE SYSTEM CONTROLLER SCRIPT - DESCRIPTION:

Options:
'Size' - Multiplies Particle Systems and Trails sizes.
'Speed' - Multiplies Particle Systems and Trails speeds.
'Loop' - Enable/Disable Particle Systems loop.
'Lights' - Enable/Disable Particle Systems lights.
'Trails' - Enable/Disable Particle Systems trails.
'Changes Color' - Enable/Disable changing color of Particle Systems and Trails speeds.
'New Max Color' - New maximum color.
'New Min Color' - New minimum color.
'Particle Systems' - The Particle Systems and Trails the prefab contains. Can be filled automatically with 'Fill Lists' button, or manually.
'Active Particle Systems' - Choose which Particle Systems and Trails are active. Can be filled automatically with 'Fill Lists' button, or manually.
'Fill Lists' - Finds and adds Particle Systems and Trails, of the parent and childs of current gameobject, to 'Particle Systems' and 'Active Particle Systems' lists.
'Empty Lists' - Emptys 'Particle Systems' and 'Active Particle Systems' lists.
'Apply' - It will apply the changes you made (Size, Speed, Loop, Lights Enabled/Disabled, Trails Enabled/Disabled, Change Color) to the particle systems in 'Particle Systems' that ARE active in the 'Active Particle Systems' list. It will also save the original settings in a folder called 'Original Settings' inside the folder of the vfx prefab.
'Reset' - Resets the Particle Systems and Trails to the original settings which are saved in a folder called 'Original Settings' inside the folder of the vfx prefab.

Workflow:
1) Add script to any VFX prefab;
2) Press 'Fill Lists' to automatically find and add Particle Systems and Trails to lists;
3) Make your changes (Size, Speed, Loop Enabled/Disabled, Lights Enabled/Disabled, Trails Enabled/Disabled, Change Color, Enable/Disable Particle Systems with 'Active Particle Systems' lists);
4) Press 'Aplly';
5) Script saves original settings and applies changes;
6) That's it, enjoy. 
PS: You can always press 'Reset' to go back to original settings.

Warnings:
1) Don't change the name of the VFX after you have pressed 'Apply'. Otherwise 'Reset' will not work since it wouldn't be able to find the original settings.
2) You can change the name of the VFX BUT you must go to the respective 'Original Settings' folder and copy paste the exact same name of the VFX.





SPAWN COMETS SCRIPT - DESCRIPTION:

Options:
'Comet' - The projectile to spawn.
'Start Point' - The spawn point from where the projectiles are going to spawn.
'End Point' - The destination of the projectiles.
'Delay' - Initial delay before it spawns the projectiles.
'Rate Of Fire' - The rate at which it spawns the projectiles in Quantity.
'Radius' - The radius for the projectiles to spawn.
'Quantity' - The number of projectiles to spawn.
'Waves' - How many times it repeats the process.

Workflow:
1) Add the Projectile you want to the 'Comet' in the inspector.
2) Assign the 'Start Point', which in this case it's a child called "CometSpawn" of the respective Magic Ability that has the 'Spawn Comets Script' attached.
3) Assign the 'End Point', which in this case it's the a Circle of the respective Magic Abilites that has the 'SpawnCometsScript' attached.
4) Choose the area of effect in the 'Radius'. It's a radius for the Start Point and End Point positions. Zero means it will spawn exactly in the 'Start Point' position and go the 'End Point' position (Speed is controlled in the 'Projectile Move Script' attached to the projectile).
5) In the 'Delay', choose how much time it will take for the first projectile to spawn. This is also the delay between each wave.
6) Select the 'Rate Of Fire' at wich the projectiles are going to spawn. Meaning that a rate of fire of 1 second will spawn a projectile each second, according to the amount assigned in the 'Quantity'.
7) Assign the amount of projectiles to spawn in the 'Quantity'.
8) Choose how many times this will repeat in the 'Waves'. 

Notes:
This are all just examples on how you can use this effects. Only simple code suggestions.





PROJECTILE MOVE SCRIPT - DESCRIPTION:

Options:
'Speed' - The speed of the projectile.
'Accuracy' - The accuracy the projectile has. Goes from 0 to 100. For example, 100% of accuracy means it goes exactly where we aiming at.
'Fire Rate' - The fire rate of the projectile. For example, 1 means it will fire 1 projectile each second.
'Muzzle Prefab' - The effect to spawn each time we fire the projectile.
'Hit Prefab' - The effect to spawn when hitting something.
'Shot SFX' - The sound it makes when firing a projectile.
'Hit SFX' - The sound for when hitting something.
'Trails' - It's the 'Particle Systems' or 'Trail Renderers' that we want to dettach when hitting something. If not added it will destroy the trail of particles or the trail renderer as soon as it it's something.

Workflow:
1) Choose the 'Speed' of the projectile.
2) Choose the 'Accuracy' it will have. For example, 100% of accuracy means it goes exactly where we aiming at.
3) Choose the 'Fire Rate' . For example, 1 means it will fire 1 projectile each second.
4) Assign the respective Muzzle Flash you want to the 'Muzzle Prefab'.
5) Assign the respective Hit effect you want to the 'Hit Prefab'.
6) If you have SFX, you can assign them to the 'Shot SFX' for when shooting and the 'Hit SFX' for when hitting something.
7) In the 'Trails' you can add the trail renderers, and the respective particles that leave a trail, preventing from being immediatly destryoed on collision.

Notes:
This are all just examples on how you can use this effects. Only simple code suggestions.





CONTACTS:

Feel free to contact me via links bellow in case you have any doubts. 

Twitter: @GabrielAguiProd

Facebook: facebook.com/gabrielaguiarprod/

YouTube: youtube.com/c/gabrielaguiarprod



Thank you for purchasing the Unique Magic Abilities Volume 1 package.
Unique Magic Abilities Volume 01 is created by Gabriel Aguiar



Any feedback on the Asset Store is very Welcome!