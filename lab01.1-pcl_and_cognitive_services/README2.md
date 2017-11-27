# lab01.1-pcl_and_cognitive_services - Using Portable Class Libraries to Simplify App Development with Cognitive Services

This hands-on lab guides you through creating an intelligent Windows (UWP) application from end-to-end using Cognitive Services (Computer Vision API, Emotion API, Face API). We will focus on the ImageProcessing portable class library (PCL), discussing its contents and how to use it in your own applications. 


## Objectives 
In this workshop, you will:
- Learn about some of the various Cognitive Services APIs
- Understand how to configure your apps to call Cognitive Services
- Build an application that calls various Cognitive Services APIs (specifically Computer Vision, Face, Emotion) in .NET applications

While there is a focus on Cognitive Services, you will also leverage the following technologies:

- Visual Studio 2017, Community Edition
- Windows 10 SDK (UWP)
- Cosmos DB
- Azure Storage
- Data Science Virtual Machine

 
## Prerequisites

This workshop is meant for an AI Developer on Azure. Since this is a half-day workshop, there are certain things you need before you arrive.

Firstly, you should have experience with Visual Studio. We will be using it for everything we are building in the workshop, so you should be familiar with [how to use it](https://docs.microsoft.com/en-us/visualstudio/ide/visual-studio-ide) to create applications. Additionally, this is not a class where we teach you how to code or develop applications. We assume you some familiarity with C# (you can learn [here](https://mva.microsoft.com/en-us/training-courses/c-fundamentals-for-absolute-beginners-16169?l=Lvld4EQIC_2706218949)), but you do not know how to implement solutions with Cognitive Services. 

Secondly, you should have experience with the portal and be able to create resources (and spend money) on Azure. 

## Introduction

We're going to build an end-to-end application that allows you to pull in your own pictures, use Cognitive Services to find objects and people in the images, figure out how those people look like they are feeling, and store all of that data in a NoSQL Store (Cosmos DB). In a continuation of this lab, `lab01.2-luis_and_search`, we will use that NoSQL Store to populate an Azure Search index, and then build a Bot Framework bot using LUIS to allow easy, targeted querying.

## Architecture

We will build a simple C# application that allows you to ingest pictures from your local drive, then invoke several different Cognitive Services to gather data on those images:

- [Computer Vision](https://www.microsoft.com/cognitive-services/en-us/computer-vision-api): We will call this service to analyze the image and obtain tags and a description.
- [Face](https://www.microsoft.com/cognitive-services/en-us/face-api): We will call this service to determine if there are faces in the image, and if there are, we will store the UniqueFaceId and FaceRectangle location.
- [Emotion](https://www.microsoft.com/cognitive-services/en-us/emotion-api): We use this to pull emotion scores from each face in the image.

Once we have this data, we process it to pull out the details we need, and store it all into [Cosmos DB](https://azure.microsoft.com/en-us/services/cosmos-db/), our [NoSQL](https://en.wikipedia.org/wiki/NoSQL) [PaaS](https://azure.microsoft.com/en-us/overview/what-is-paas/) offering.

In the continuation of this lab, `lab01.2-luis_and_search`, we'll build an [Azure Search](https://azure.microsoft.com/en-us/services/search/) Index (Azure Search is our PaaS offering for faceted, fault-tolerant search - think Elastic Search without the management overhead) on top of Cosmos DB. We'll show you how to query your data, and then build a [Bot Framework](https://dev.botframework.com/) bot to query it. Finally, we'll extend this bot with [LUIS](https://www.microsoft.com/cognitive-services/en-us/language-understanding-intelligent-service-luis) to automatically derive intent from your queries and use those to direct your searches intelligently. 

![Architecture Diagram](./resources/assets/AI_Immersion_Arch.png)

> This lab was modified from this [Cognitive Services Tutorial](https://github.com/noodlefrenzy/CognitiveServicesTutorial).

## Navigating the GitHub ##

There are several directories in the [resources](./resources) folder:

- **assets**: This contains all of the images for the lab manual. You can ignore this folder.
- **code**: In here, there are several directories that we will use:
	- **Starting-ImageProcessing** and **Finished-ImageProcessing**: There is a folder for starting, which you should use if you are going through the labs, but there is also a finished folder if you get stuck or run out of time. Each folder contains a solution (.sln) that has several different projects for the workshop, let's take a high level look at them:
		- **ProcessingLibrary**: This is a Portable Class Library (PCL) containing helper classes for accessing the various Cognitive Services related to Vision, and some "Insights" classes for encapsulating the results.
		- **ImageStorageLibrary**: Since Cosmos DB does not (yet) support UWP, this is a non-portable library for accessing Blob Storage and Cosmos DB.
		- **TestApp**: A UWP application that allows you to load your images and call the various cognitive services on them, then explore the results. Useful for experimentation and exploration of your images.
		- **TestCLI**: A Console application allowing you to call the various cognitive services and then upload the images and data to Azure. Images are uploaded to Blob Storage, and the various metadata (tags, captions, faces) are uploaded to Cosmos DB.

		Both _TestApp_ and _TestCLI_ contain a `settings.json` file containing the various keys and endpoints needed for accessing the Cognitive Services and Azure. They start blank, so once you provision your resources, we will grab your service keys and set up your storage account and Cosmos DB instance.
		
### Labs: Setting up your Azure Account, Setting up your DSVM, and Collecting the Keys
Please complete the labs in [1_Setup](./1_Setup.md).

## Cognitive Services

Cognitive Services can be used to infuse your apps, websites and bots with algorithms to see, hear, speak, understand and interpret your user needs through natural methods of communication. 

There are five main categories for the available Cognitive Services:
- **Vision**: Image-processing algorithms to identify, caption and moderate your pictures
- **Knowledge**: Map complex information and data in order to solve tasks such as intelligent recommendations and semantic search
- **Language**: Allow your apps to process natural language with pre-built scripts, evaluate sentiment and learn how to recognize what users want
- **Speech**: Convert spoken audio into text, use voice for verification, or add speaker recognition to your app
- **Search**: Add Bing Search APIs to your apps and harness the ability to comb billions of webpages, images, videos, and news with a single API call

You can browse all of the specific APIs in the [Services Directory](https://azure.microsoft.com/en-us/services/cognitive-services/directory/). 

As you may recall, the application we'll be building today will use [Computer Vision](https://www.microsoft.com/cognitive-services/en-us/computer-vision-api) to grab tags and a description, [Face](https://www.microsoft.com/cognitive-services/en-us/face-api) to grab faces and their details from each image, and [Emotion](https://www.microsoft.com/cognitive-services/en-us/emotion-api) to pull emotion scores from each face in the image.

Let's talk about how we're going to call these Cognitive Services in our application.

### **Image Processing Library** ###

Under resources>code>Starting-ImageProcessing, you'll find the `Processing Library`. This is a [Portable Class Library (PCL)](https://docs.microsoft.com/en-us/dotnet/standard/cross-platform/cross-platform-development-with-the-portable-class-library), which helps in building cross-platform apps and libraries quickly and easily. It serves as a wrapper around several services. This specific PCL contains various helper classes for accessing the various Cognitive Services related to Vision, and several "Insights" classes to encapsulate the results. Later, we'll create an image processor class that will be responsible for wrapping an image and exposing several methods and properties that act as a bridge to the Cognitive Services. 

After creating the image processor, you should be able to pick up this portable class library and drop it in your other projects that involve Cognitive Services (some modification may be required). 


**Service Helpers**

Service helpers exist to make your life easier when you're developing your app. One of the key things that service helpers do is provide the ability to detect when the API calls return a call-rate-exceeded error and automatically retry the call (after some delay). They also help with bringing in methods, handling exceptions and handling the keys.

You can find additional service helpers for some of the other Cognitive Services within the [Intelligent Kiosk sample application](https://github.com/Microsoft/Cognitive-Samples-IntelligentKiosk/tree/master/Kiosk/ServiceHelpers). Utilizing these resources makes it easy to add and remove the service helpers in your future projects as needed.


**The "Insights" classes**

Take a look at each of the "Insights" classes:
- If you look at `FaceInsights.cs`, you can see the items we ultimately want from the Face and Emotion APIs: `UniqueFaceID`, `FaceRectangle`, `TopEmotion`, `Gender`, and `Age`
- You can see that in `VisionInsights.cs`, we're calling for `Caption` and `Tags` from the images. 
- Finally, in `ImageInsights.cs`, we're creating our complete Image Insights for each image, with the `ImageId`, `FaceInsights` and `VisionInsights`.

Overall, the "Insights" group only the pieces of information we want from the Cognitive Services.

Now let's take a step back for a minute. It isn't quite as simple as creating "Insights" classes and copying over some methsods/error handling from service helpers. We still have to call the API and process the images somewhere. For the purpose of this lab, we are going to walk through creating `ImageProcessor.cs`, but in future projects, feel free to add this class to your PCL and start from there (it may need modification depending what Cognitive Services you are calling and what you are processing - images, text, voice, etc.).

### Lab: Creating `ImageProcessor.cs`

Please complete the lab in [2_ImageProcessor](./2_ImageProcessor.md).


### Lab: Building and exploring the TestApp

Please complete the labs in [3_TestApps](./3_TestApp.md).

## (optional) Exploring Cosmos DB

Azure Cosmos DB is our resilient NoSQL PaaS solution and is incredibly useful for storing loosely structured data like we have with our image metadata results. There are other possible choices (Azure Table Storage, SQL Server), but Cosmos DB gives us the flexibility to evolve our schema freely (like adding data for new services), query it easily, and can be quickly integrated into Azure Search (which we'll do in `lab01.2-luis_and_search`).

Cosmos DB is not a focus of this workshop, but if you're interested in what's going on - here are some highlights from the code we will be using:
- Navigate to the `DocumentDBHelper.cs` class in the `ImageStorageLibrary`. Many of the implementations we are using can be found in the [Getting Started guide](https://docs.microsoft.com/en-us/azure/cosmos-db/documentdb-get-started).
- Go to `TestCLI`'s `Util.cs` and review  the `ImageMetadata` class. This is where we turn the `ImageInsights` we retrieve from Cognitive Services into appropriate Metadata to be stored into Cosmos DB.
- Finally, look in `Program.cs` and notice in `ProcessDirectoryAsync`. First, we check if the image and metadata have already been uploaded - we can use `DocumentDBHelper` to find the document by ID and to return `null` if the document doesn't exist. Next, if we've set `forceUpdate` or the image hasn't been processed before, we'll call the Cognitive Services using `ImageProcessor` from the `ImageProcessingLibrary` and retrieve the `ImageInsights`, which we add to our current `ImageMetadata`. 
- Once all of that is complete, we can store our image - first the actual image into Blob Storage using our `BlobStorageHelper` instance, and then the `ImageMetadata` into Cosmos DB using our `DocumentDBHelper` instance. If the document already existed (based on our previous check), we should update the existing document. Otherwise, we should be creating a new one.

### Lab (optional): Cosmos DB and TestCLI
If there is time, and you are planning on completing the next workshop (lab01.2-luis_and_search), you may want to complete [4_Cosmos](./4_Cosmos.md). 

#### Finish early? Try this ####

What if we needed to port our application to another language? Modify your code to call the [Translator API](https://azure.microsoft.com/en-us/services/cognitive-services/translator-text-api/) on the caption and tags you get back from the Vision service.

Look into the _Image Processing Library_ at the _Service Helpers_. You can copy one of these and use it to invoke the [Translator API](https://docs.microsofttranslator.com/text-translate.html). Now you can hook this into the `ImageProcessor.cs`. Try adding translated versions to your `ImageInsights` class, and then wire it through to the DocuementDB `ImageMetadata` class. 


## Lab Completion

In this lab we covered creating an intelligent bot from end-to-end using the Microsoft Bot Framework, Azure Search and several Cognitive Services.

You should have learned:
- What the various Cognitive Services APIs are
- How to configure your apps to call Cognitive Services
- How to build an application that calls various Cognitive Services APIs (specifically Computer Vision, Face, Emotion and LUIS) in .NET applications



## Appendix ##

### Further resources ###

- [Cognitive Services](https://www.microsoft.com/cognitive-services)
- [Cosmos DB](https://docs.microsoft.com/en-us/azure/cosmos-db/)
- [Azure Search](https://azure.microsoft.com/en-us/services/search/)
- [Bot Developer Portal](http://dev.botframework.com)

