# Using Portable Class Libraries to Simplify App Development with Cognitive Services

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

## Navigating the Labs

This workshop has been broken down into five sections:
- [1_Setup](./1_Setup.md): Here we'll get everything set up for you to perform these labs - Azure, a Data Science Virtual Machine, and Keys you'll need throughout the workshop
- [2_ImageProcessor](./2_ImageProcessor.md): You'll learn about portable class libraries, and how to build an image processor from service helpers
- [3_TestApp](./3_TestApp.md): We'll finish building our application and test it out, also storing our insights in `ImageInsights.json`
- [4_TestCLI](./4_TestCLI.md) (optional): Here we'll load our images into Cosmos DB and Azure Storage using a console application. You should complete this lab if you want to learn more about Cosmos DB, and if you plan on completing **lab01.2-luis_and_search**
- [5_Challenge_and_Closing](./5_Challenge_and_Closing.md): If you get through all the labs, try this challenge. You will also find a summary of what you've done and where to learn more.



### Continue to [1_Setup](./1_Setup.md)


