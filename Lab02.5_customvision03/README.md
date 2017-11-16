**(If time permits)**
=====================

 

**Modifying Custom Vision API C\# Tutorial**
============================================

In this lab you will take the solution from Lab 1 and modify the code to upload
a new set of images, add tags to it; train the project and obtain the default
prediction endpoint URL for the project. You will then use the endpoint to
programmatically test an image. You can use this open source example as a
template for building your own app for Windows using the Custom Vision AP. This
lab deliberately has limited instructions, as it should build on the knowledge
gained in Lab 2.

 

**Prerequisites**
-----------------

###  

### Lab 1 completed

This instructions in this example relies on the fact that the previous lab has
been completed.

 

### Platform requirements

This example has been developed for the .NET Framework using [Visual Studio
2015, Community Edition](https://www.visualstudio.com/downloads/)

 

### Training client library

You may need to install the client library. The easiest way to get the training
client library is to get the
[Microsoft.Cognitive.CustomVision.Training](https://www.nuget.org/packages/Microsoft.Cognitive.CustomVision.Training/)
package from [nuget](ttp://nuget.org).

 

### The Training API key

The training API key allows you to create, manage and train Custom Vision
projects programatically. All operations on the
[website](https://customvision.ai)are exposed through this library, allowing you
to automate all aspects of the Custom Vision Service. You can obtain a key by
creating a project at at the website and finding the key in the setting of the
project that you have created.

<br>**Lab: Modifying a Custom Vision Application**
--------------------------------------------------

 

### Step 1: Create a console application and prepare the training key and the images needed for the example.

 

Start Visual Studio 2017, Community Edition, open the Visual Studio solution
named CustomVision.Sample.sln in location
\\Lab\\Starter\\Cognitive-CustomVision-Windows\\Samples\\CustomVision.Sample.
This code defines a solution for training images using a custom.ai training key.
The images for this particular lab can be found at  \\Lab\\Starter in a folder
called LabImages. Use these images as you see fit. Once used, the solution
performs a classification prediction on a test image that is uploaded. On
opening the project the following code should be displayed from line 35.

 

### Step 2: Change the memory stream definition

Change the variable names for the list items to match the names defined for the
image types

 

### Step 3: Modify a Custom Vision Service project name

Modify Custom Vision Service project name to “My Third Project”, add the
following change in the code in your **Main()** method after the call to
**LoadImagesFromDisk().**

 

### Step 4: Add tags to your project

Create new tags to your project to distinguish between pools and tracks, insert
the following code after the call to **CreateProject(”My Third Project”);**.

 

### Step 5: Upload images to the project

To add the images we have in memory to the project, insert the following code
after the call to **CreateTag(project.Id, "Tracks")** method.

 

### Step 6: Redefine the LoadImagesFromDisk() method

Use the images from the LabImages folder In this folder are three folders. The
Pools folder contains pictures of swimming pools. The Tracks folder contains
pictures of athletics tracks. These folders are used in the training of the
classification model. The final folder named test contains a single picture
named Sport.jpg that will be used to perform the prediction.

Modify the code at the end of the Program.cs file on line 145 that makes
reference to the new location of the images that are being used in the
prediction. The code should look as follows:

 

### Step 7: Run the example

Build and run the solution. You will be required to input your training API key
into the console app when runing the solution so have this at the ready. The
training and prediction of the images can take 2 mins The prediction results
appear on the console.
