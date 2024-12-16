# Particle Bake Tool

The Particle Bake Tool is a Unity Editor extension that allows you to bake particle systems into either a texture or a mesh for improved performance and flexibility in your projects. This tool is particularly useful for optimizing complex particle effects by converting them into static or baked assets.

Features:
Bake particle systems into a mesh.
Export baked particle systems as a texture.
Options to apply the material of the original particle system to the baked mesh.
Flexible resolution settings for texture exports.
Intuitive Unity Editor interface for easy use.

Installation:
Download or clone the repository from GitHub.
Add the ParticleBakeTool.cs file to your Unity project's Editor folder.
Open Unity and navigate to Tools > Particle Bake Tool to access the tool.

How to Use:
Open the tool from the Unity menu:
Tools > Particle Bake Tool.
Assign your Particle System in the provided field.
Optionally, assign a Camera to capture the particle effect.
Configure your desired export options:
Export as Mesh: Save the baked particle system as a 3D mesh.
Export as Image: Save the particle system as a 2D texture.
Customize save paths and texture resolution as needed.
Click the Bake Particles button to generate the asset.

Export Options:
Mesh Export:
Bakes the particle system into a mesh and saves it as an .asset file.
Optionally applies the material from the original particle system.
Image Export:
Captures the particle system from the camera's view as a PNG image.
Allows you to set the texture resolution and save path.
Example Use Case:
You can use this tool to:

Optimize complex particle effects by converting them to meshes.
Pre-render particles for use in mobile or performance-critical projects.
Capture particle system visuals for use in UI, promotional materials, or as part of a sprite sheet.

Example : 

![image](https://github.com/user-attachments/assets/48f5668f-2355-4ba7-b540-24e825ae4fa8)

![BakedParticleTexture](https://github.com/user-attachments/assets/5fed38b1-3673-4c15-9733-db2032a3ac4d)


