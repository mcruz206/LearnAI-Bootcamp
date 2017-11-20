**Modifying Custom Vision API C\# Tutorial**
============================================

In this lab you will take the solution from Lab 1 and modify the code to upload
a new set of images, add tags to it; train the project and obtain the default
prediction endpoint URL for the project. You will then use the endpoint to
programmatically test an image. You can use this open source example as a
template for building your own app for Windows using the Custom Vision AP

 

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
It then performs a classification prediction on a test image that is uploaded.
On opening the project the following code should be displayed from line 35.

 

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Cognitive.CustomVision;

namespace CustomVision.Sample
{
    class Program
    {
        private static List<MemoryStream> hemlockImages;

        private static List<MemoryStream> japaneseCherryImages;

        private static MemoryStream testImage;

        static void Main(string[] args)
        {
            // You can either add your training key here, pass it on the command line, or type it in when the program runs
            string trainingKey = GetTrainingKey("<your key here>", args);

            // Create the Api, passing in a credentials object that contains the training key
            TrainingApiCredentials trainingCredentials = new TrainingApiCredentials(trainingKey);
            TrainingApi trainingApi = new TrainingApi(trainingCredentials);

            // Create a new project
            Console.WriteLine("Creating new project:");
            var project = trainingApi.CreateProject("My New Project");

            // Make two tags in the new project
            var hemlockTag = trainingApi.CreateTag(project.Id, "Hemlock");
            var japaneseCherryTag = trainingApi.CreateTag(project.Id, "Japanese Cherry");

            // Add some images to the tags
            Console.WriteLine("\tUploading images");
            LoadImagesFromDisk();

            // Images can be uploaded one at a time
            foreach (var image in hemlockImages)
            {
                trainingApi.CreateImagesFromData(project.Id, image, new List<string>() { hemlockTag.Id.ToString() });
            }

            // Or uploaded in a single batch 
            trainingApi.CreateImagesFromData(project.Id, japaneseCherryImages, new List<Guid>() { japaneseCherryTag.Id });

            // Now there are images with tags start training the project
            Console.WriteLine("\tTraining");
            var iteration = trainingApi.TrainProject(project.Id);

            // The returned iteration will be in progress, and can be queried periodically to see when it has completed
            while (iteration.Status == "Training")
            {
                Thread.Sleep(1000);

                // Re-query the iteration to get it's updated status
                iteration = trainingApi.GetIteration(project.Id, iteration.Id);
            }

            // The iteration is now trained. Make it the default project endpoint
            iteration.IsDefault = true;
            trainingApi.UpdateIteration(project.Id, iteration.Id, iteration);
            Console.WriteLine("Done!\n");

            // Now there is a trained endpoint, it can be used to make a prediction

            // Get the prediction key, which is used in place of the training key when making predictions
            var account = trainingApi.GetAccountInfo();
            var predictionKey = account.Keys.PredictionKeys.PrimaryKey;

            // Create a prediction endpoint, passing in a prediction credentials object that contains the obtained prediction key
            PredictionEndpointCredentials predictionEndpointCredentials = new PredictionEndpointCredentials(predictionKey);
            PredictionEndpoint endpoint = new PredictionEndpoint(predictionEndpointCredentials);

            // Make a prediction against the new project
            Console.WriteLine("Making a prediction:");
            var result = endpoint.PredictImage(project.Id, testImage);

            // Loop over each prediction and write out the results
            foreach (var c in result.Predictions)
            {
                Console.WriteLine($"\t{c.Tag}: {c.Probability:P1}");
            }
            Console.ReadKey();
        }


        private static string GetTrainingKey(string trainingKey, string[] args)
        {
            if (string.IsNullOrWhiteSpace(trainingKey) || trainingKey.Equals("<your key here>"))
            {
                if (args.Length >= 1)
                {
                    trainingKey = args[0];
                }

                while (string.IsNullOrWhiteSpace(trainingKey) || trainingKey.Length != 32)
                {
                    Console.Write("Enter your training key: ");
                    trainingKey = Console.ReadLine();
                }
                Console.WriteLine();
            }

            return trainingKey;
        }

        private static void LoadImagesFromDisk()
        {
            // this loads the images to be uploaded from disk into memory
            hemlockImages = Directory.GetFiles(@"..\..\..\Images\Hemlock").Select(f => new MemoryStream(File.ReadAllBytes(f))).ToList();
            japaneseCherryImages = Directory.GetFiles(@"..\..\..\Images\Japanese Cherry").Select(f => new MemoryStream(File.ReadAllBytes(f))).ToList();
            testImage = new MemoryStream(File.ReadAllBytes(@"..\..\..\Images\Test\test_image.jpg"));

        }
    }
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

 

 

### Step 2: Change the memory stream definition

In line 46 and 48, change the variable names for the list items to match the
names defined for the image types

 

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
private static List<MemoryStream> AmpImages;

private static List<MemoryStream> GuitarImages
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

 

 

### Step 3: Modify a Custom Vision Service project name

Modify Custom Vision Service project name to “My Second Project”, add the
following change in the code in your **Main()** method after the call to
**LoadImagesFromDisk().**

 

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Create a new project
Console.WriteLine("Creating new project:");
var project = trainingApi.CreateProject("My Second Project");
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

 

### Step 4: Add tags to your project

Create new tags to your project to distinguish between amps and guitars, insert
the following code after the call to **CreateProject(”My Second Project”);**.

 

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Make two tags in the new project
var AmpTag = trainingApi.CreateTag(project.Id, "Amp");
var GuitarTag = trainingApi.CreateTag(project.Id, "Guitar");
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

 

### Step 5: Upload images to the project

To add the images we have in memory to the project, insert the following code
after the call to **CreateTag(project.Id, "Guitar")** method.

 

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Add some images to the tags
Console.WriteLine("\tUploading images");
LoadImagesFromDisk();

// Images can be uploaded one at a time
foreach (var image in AmpImages)
 {
  trainingApi.CreateImagesFromData(project.Id, image, new List<string>() { AmpTag.Id.ToString() });
 }

// Or uploaded in a single batch 
trainingApi.CreateImagesFromData(project.Id, GuitarImages, new List<Guid>() { GuitarTag.Id });
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

 

### Step 6: Redefine the LoadImagesFromDisk() method

There are a new set of images that is located in the following location:
\\Lab\\Starter\\Cognitive-CustomVision-Windows\\Samples\\CustomImages. In this
folder are three folders. The Amp folder contains pictures of amplifiers. The
Guitars folder contains pictures of guitars. These folders are used in the
training of the classification model. The final folder named test contains a
single picture that will be used to perform the prediction.

Modify the code at the end of the Program.cs file on line 145 that makes
reference to the new location of the images that are being used in the
prediction. The code should look as follows:

 

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// this loads the images to be uploaded from disk into memory
AmpImages = Directory.GetFiles(@"..\..\..\CustomImages\Amps").Select(f => new MemoryStream(File.ReadAllBytes(f))).ToList();
GuitarImages = Directory.GetFiles(@"..\..\..\CustomImages\Guitars").Select(f => new MemoryStream(File.ReadAllBytes(f))).ToList();
testImage = new MemoryStream(File.ReadAllBytes(@"..\..\..\CustomImages\Test\Instrument.jpg"));;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

 

### Step 7: Run the example

Build and run the solution. You will be required to input your training API key
into the console app when runing the solution so have this at the ready. The
training and prediction of the images can take 2 mins The prediction results
appear on the console.
