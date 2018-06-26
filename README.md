# Photolog
A Windows Forms application. Browse for folders of images, load images to a workspace, caption the images within the workspace, export the image/caption to a pre-formatted Word Document.

There are 3 options `New project`, `Save current project` and `Resume project`.

##### New project
- Browse to a folder on your computer that holds your images. 
- Once a folder is selected all images in that folder will be loaded into the left-hand panel
- Images can be loaded in following format 
    {"*.jpg", "*.jpeg", "*.jpe", "*.jif", "*.jfif", "*.jfi", "*.webp", "*.gif", "*.png", "*.apng", "*.bmp", "*.dib", "*.tiff", "*.tif", "*.svg", "*.svgz", "*.ico", "*.xbm"};

##### Save current project
- you can save your project here. Saving the project will generate an .XML file with the data for your project. It is this file that you need to select when you `Resume project`. It is safe to save your .XML file into the same folder as your images.

##### Resume project
- Load your half-baked project here. You will need to select the .XML file you saved earlier.

##### How the app works
There are two main panels. Left and Right. The Left panel contains all images that will **NOT** be `Published` in the Word document. The right panel contains the images that **WILL** be `Published` the Word document, along with a `caption` of your chosing. 

All images are loaded into the left-panel at first. You can the swap images between left and right panels. To get a better view of the images you can `double-click` them.  

Once images are in the Right-panel they can also be moved up and down. Do **NOT** label the Images with numbers. They will be automatically numbered when `Published` to the Word Document.

It must be noted. The app works by simply moving a pointer the complete filepath around from one side to the other. If you move your images out of the original folder the you will get an error. If you change the filenames you will get an error.

