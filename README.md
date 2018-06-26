# Photolog
A Windows Forms application. Browse for folders of images, load images to a workspace, caption the images within the workspace, export the image/caption to a pre-formatted Word Document.

There are 3 options `New project`, `Save current project` and `Resume project`.

##### New project
- Browse to a folder on your computer that holds your images. 
- Once a folder is selected all images in that folder will be loaded into the left-hand panel
- Images can be loaded in following format 
    {"*.jpg", "*.jpeg", "*.jpe", "*.jif", "*.jfif", "*.jfi", "*.webp", "*.gif", "*.png", "*.apng", "*.bmp", "*.dib", "*.tiff", "*.tif", "*.svg", "*.svgz", "*.ico", "*.xbm"};

##### Save current project
- you can save your project here. Saving the project will generate an .XML file with the data for your project. It is this file that you need to select when you `Resume project`

##### Resume project
- Load your half-baked project here. You will need to select the .XML file you saved earlier.

##### How the app works
There are two main panels. Left and Right. The Left panel contains all images that will NOT go into the Word document. The right panel contains the images that WILL go into the *Word document*, along with a caption of your chosing. 

All images are loaded into the left-panel at first. You can the swap images between left and right panels. 

