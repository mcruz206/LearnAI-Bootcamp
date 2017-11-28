# lab01.2-luis_and_search - Developing Intelligent Applications with LUIS and Azure Search

This hands-on lab guides you through creating an intelligent bot from end-to-end using the Microsoft Bot Framework, Azure Search, and Microsoft's Language Understanding Intelligent Service (LUIS). 


## Objectives
In this workshop, you will:
- Understand how to implement Azure Search features to provide a positive search experience inside applications
- Configure an Azure Search service to extend your data to enable full-text, language-aware search
- Build, train and publish a LUIS model to help your bot communicate effectively
- Build an intelligent bot using Microsoft Bot Framework that leverages LUIS and Azure Search


While there is a focus on LUIS and Azure Search, you will also leverage the following technologies:

- Data Science Virtual Machine (DSVM)
- Windows 10 SDK (UWP)
- CosmosDB
- Azure Storage
- Visual Studio


## Prerequisites

This workshop is meant for an AI Developer on Azure. Since this is a short workshop, there are certain things you need before you arrive.

Firstly, you should have experience with Visual Studio. We will be using it for everything we are building in the workshop, so you should be familiar with [how to use it](https://docs.microsoft.com/en-us/visualstudio/ide/visual-studio-ide) to create applications. Additionally, this is not a class where we teach you how to code or develop applications. We assume you know how to code in C# (you can learn [here](https://mva.microsoft.com/en-us/training-courses/c-fundamentals-for-absolute-beginners-16169?l=Lvld4EQIC_2706218949)), but you do not know how to implement advanced Search and NLP (natural language processing) solutions. 

Secondly, you should have some experience developing bots with Microsoft's Bot Framework. We won't spend a lot of time discussing how to design them or how dialogs work. If you are not familiar with the Bot Framework, you should take [this Microsoft Virtual Academy course](https://mva.microsoft.com/en-us/training-courses/creating-bots-in-the-microsoft-bot-framework-using-c-17590#!) prior to attending the workshop.

Thirdly, you should have experience with the portal and be able to create resources (and spend money) on Azure. We will not be providing Azure passes for this workshop.

## Introduction

We're going to build an end-to-end scenario that allows you to pull in your own pictures, use Cognitive Services to find objects and people in the images, figure out how those people are feeling, and store all of that data into a NoSQL Store (DocumentDB). We'll use that NoSQL Store to populate an Azure Search index, and then build a Bot Framework bot using LUIS to allow easy, targeted querying.

> Note: This lab is a continuation of `lab01.1-pcl_and_cognitive_services`; we will skip pulling in pictures, using Cognitive Services to determine information about the images, and storing the data in DocumentDB. The only thing we will use from that lab is DocumentDB to populate our search index. If you have completed `lab01.1-pcl_and_cognitive_services`, you can optionally use your DocumentDB connection string instead of the one provided.

## Architecture

In `lab01.1-pcl_and_cognitive_services`, we built a simple C# application that allows you to ingest pictures from your local drive, then invoke several different Cognitive Services to gather data on those images:

- [Computer Vision](https://www.microsoft.com/cognitive-services/en-us/computer-vision-api): We use this to grab tags and a description
- [Face](https://www.microsoft.com/cognitive-services/en-us/face-api): We use this to grab faces and their details from each image
- [Emotion](https://www.microsoft.com/cognitive-services/en-us/emotion-api): We use this to pull emotion scores from each face in the image

Once we had this data, we processed it and stored all the information needed in [DocumentDB](https://azure.microsoft.com/en-us/services/documentdb/), our [NoSQL](https://en.wikipedia.org/wiki/NoSQL) [PaaS](https://azure.microsoft.com/en-us/overview/what-is-paas/) offering.

Now that we have it in DocumentDB, we'll build an [Azure Search](https://azure.microsoft.com/en-us/services/search/) Index on top of it (Azure Search is our PaaS offering for faceted, fault-tolerant search - think Elastic Search without the management overhead). We'll show you how to query your data, and then build a [Bot Framework](https://dev.botframework.com/) bot to query it. Finally, we'll extend this bot with [LUIS](https://www.microsoft.com/cognitive-services/en-us/language-understanding-intelligent-service-luis) to automatically derive intent from your queries and use those to direct your searches intelligently. 

![Architecture Diagram](./resources/assets/AI_Immersion_Arch.png)

> This lab was modified from this [Cognitive Services Tutorial](https://github.com/noodlefrenzy/CognitiveServicesTutorial).

## Navigating the GitHub ##

There are several directories in the [resources](./resources) folder:

- **assets**: This contains all of the images for the lab manual. You can ignore this folder.
- **code**: In here, there are several directories that we will use:
	- **LUIS**: Here you will find the LUIS model for the PictureBot. You will create your own, but if you fall behind or want to test out a different LUIS model, you can use the .json file to import this LUIS app.
	- **Models**: These classes will be used when we add search to our PictureBot.
	- **Finished-PictureBot**: Here there is the finished PictureBot.sln that is for the latter sections of the workshop, where we integrate LUIS and our Search Index into the Bot Framework. If you fall behind or get stuck, you can use this.

> You need Visual Studio to run these labs, but if you have already deployed a Windows Data Science Virtual Machine for one of the workshops, you could use that.

## Collecting the Keys

Over the course of this lab, we will collect various keys. It is recommended that you save all of them in a text file, so you can easily access them throughout the workshop.

>_Keys_
>- LUIS API:
>- Cosmos DB Connection String:
>- Azure Search Name:
>- Azure Search Key:
>- Bot Framework App Name:
>- Bot Framework App ID:
>- Bot Framework App Password:


## Navigating the Labs

This workshop has been broken down into five sections:
- [1_AzureSearch](./1_AzureSearch.md): Here you will learn about Azure Search and create an index
- [2_LUIS](./2_LUIS.md): We'll build a LUIS model to enhance the language understanding of our bot (which you'll build in the next lab)
- [3_Bot](./3_Bot.md): Learn how to put everything together using the Bot Framework
- [4_Bot_Enhancements](./4_Bot_Enhancements.md): We'll finish by enhancing our bot with regular expressions and publishing it with the Bot Connector
- [5_Challenge_and_Closing](./4_Challenge_and_Closing.md): If you get through all the labs, try this challenge. You will also find a summary of what you've done and where to learn more.



### Continue to [1_AzureSearch](./1_AzureSearch.md)


