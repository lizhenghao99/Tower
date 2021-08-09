Thanks for purchasing my "Floating Particles Pack"!

Setup is very simple, in most cases, you simply need only to drag the prefab into your scene to add the VFX to your project. You will need to import the Unity Standard Asset GlassStainedBumpDistort Shader to effectively use the ragingEmbers VFX:

1) Right-click on empty space in outliner
2) "Import Package" > "Effects"
3) Window will pop up
4) Uncheck the top left box to uncheck everything
5) Find "GlassStainedBumpDistort.shader"
6) Check it's box
7) Click "Import" bottom right


*|IF "PS_RAGINGEMBERS" IS STILL SOLID PINK, MAY NEED TO RE-LINK MATERIAL:

1) Go to BryceMacDonald-FloatingParticlesPack > "Materials"
2) Open "MAT_floating&ragingEmbers-Blur"
3) Switch Shader (right below name of Material in inspector window):
--> "FX" > "Glass" > "Stained BumpDistort"
 


Demo scene is under BryceMacDonald-FloatingParticlesPack > "Scenes". This scene has all VFX placed in front of a camera which has a near-black background to make the VFX stand out, you will need to change the background color to something brighter to see ashFall-Floaty, ashFall-Raining, and preferably for waterBubbles as well.

All VFX prefabs are under BryceMacDonald-FloatingParticlesPack > "Shuriken", this is where you will drag them from to bring them into your scene. They are all 1x scale and have zeroed out rotation and translation, divineAura will look different than the demo video on the store page, this is for consistency and organization: it is still the same VFX. To get the one in the video simply change the Shape to Circle, check the "Emit from Edge" box right under the shape, and set X rotation to 90.



As said on the store page: generic knowledge of the Shuriken Particle System is a plus, as it will allow you to resize, change the density of, and even edit speeds and whatnot of the particle effects. While these VFX are all ready to be dragged and dropped, obviously some changing of the emitter size/shape will need to be done, as well as a changing of the emission rate, for them to fit into your game accurately. Read below for a quick run-through of how to do this.

TO CHANGE VFX DENSITY/EMISSION RATE:
1) Open first tab "Emission" in particle system Inspector
2) Change "Rate over Time" number until you get the density you like
3) The higher the "Particles" number on the "Particle Effect" pop-up in-editor is, the more intensive the VFX will be on your game's performance

TO CHANGE VFX SIZE/SHAPE:
1) Open second tab "Shape" in particle system Inspector
2) Shape drop-down has different options to change shape, rotating is done the same as rotating any other prefab
3) There are a variety of size parameters depending on which shape you have chosen
4) Most of these VFX will be used with boxes, thus they are already set to Shape "Box"
5) Change the Box X, Box Y, and Box Z values to change the size of the box to fit the desired location in your scene



Thank you for reading through the documentation, and I hope things are clear enough! If you have any other questions, feel free to email me at: "cakesanimations1@gmail.com"

Thanks for your purchase and support!







