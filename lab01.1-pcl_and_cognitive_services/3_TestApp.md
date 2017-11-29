## 3_TestApp
Estimated Time: 10-15 minutes

### Lab: Building and exploring the TestApp

We've spent some time looking at the `ImageProcessingLibrary`, but you will also find a UWP application (`TestApp`) that allows you to load your images and call the various Cognitive Services on them, then explore the results. It is useful for experimentation and exploration of your images. This app is built on top of the `ImageProcessingLibrary` project, which is also used  by the TestCLI project to analyze the images. 

You'll want to "Build" the solution (right click on `ImageProcessing.sln` and select "Build Solution"). You also may have to reload the TestApp project, which you can do by right-clicking on it and selecting "Reload project". 

Before running the app, make sure to enter the Cognitive Services API keys in the `settings.json` file under the `TestApp` project. Once you do that, run the app, point it to the [sample_images](./resources/sample_images) folder (or any folder with images) via the `Select Folder` button, and it should generate results like the following, showing all the images it processed, along with a breakdown of unique faces, emotions and tags that also act as filters on the image collection.

![UWP Test App](./resources/assets/UWPTestApp.JPG)

Once the app processes a given directory it will cache the results in a `ImageInsights.json` file in that same folder, allowing you to look at that folder results again without having to call the various APIs. Open the file and examine the results. Are they structured how you expected? Discuss with a neighbor. 

### Continue to [4_TestCLI](./4_TestCLI.md) (optional) or [5_Challenge_and_Closing](./5_Challenge_and_Closing.md)




Back to [README](./0_readme.md)