## 1_Setup:
Estimated Time: 30-40 minutes

### Lab: Setting up your Azure Account

You may activate an Azure free trial at [https://azure.microsoft.com/en-us/free/](https://azure.microsoft.com/en-us/free/).  

If you have been given an Azure Pass to complete this lab, you may go to [http://www.microsoftazurepass.com/](http://www.microsoftazurepass.com/) to activate it.  Please follow the instructions at [https://www.microsoftazurepass.com/howto](https://www.microsoftazurepass.com/howto), which document the activation process.  A Microsoft account may have **one free trial** on Azure and one Azure Pass associated with it, so if you have already activated an Azure Pass on your Microsoft account, you will need to use the free trial or use another Microsoft account.

### Lab: Setting up your Data Science Virtual Machine

After creating an Azure account, you may access the [Azure portal](https://portal.azure.com). From the portal, [create a Resource Group for this lab](https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-group-portal). Detailed information about the Data Science Virtual Machine can be [found online](https://docs.microsoft.com/en-us/azure/machine-learning/data-science-virtual-machine/overview), but we will just go over what's needed for this workshop. In your Resource Group, deploy and connect to a Data Science Virtual Machine for Windows (2016), with a size of D4S_V3 (this is only available on certain regions, try "West US 2" or "East US"). All other defaults are fine.
>We are creating a VM and not doing it locally, because many of you will not have the ability to change your machine to "Developer Mode" which we need to develop UWP (Windows) apps.

Once you're connected, there are several things you need to do to set up the DSVM for the workshop:

1. Navigate to [this repository](https://github.com/Azure/learnAI-Bootcamp) in Firefox, and download it as a zip file. Extract all the files for this lab to your Desktop.
2. Open "ImageProcessing.sln" which is under resources>code>Starting-ImageProcessing. It may take a while for Visual Studio to open for the first time, and you will have to log in.
3. Once it's open, you will be prompted to install the SDK for Windows 10 App Development (UWP). Follow the prompts to install it (you'll have to close Visual Studio). If you aren't prompted, right click on TestApp and select "Reload project", then you will be prompted.
4. While it's installing, there are a few tasks you can complete: 
	- Type in the Cortana search bar "For developers", select "For developers settings", and change the settings to "Developer Mode".
	- Type in the Cortana search bar "gpedit.msc" and push enter. Enable the following policy: Computer Configuration>Windows Settings>Security Settings>Local Policies>Security Options>User Account Control: Admin Approval Mode for the Built-in Administrator account
    - In the Cortana search bar, type "gpupdate", and click "gpupdate" to force the local security policy to refresh immediately
	- Start the Collecting the Keys lab (right below this one) 
5. Once the install is complete and you have changed your developer settings and the User Account Control policy, reboot your DSVM. 
> Note: Be sure to turn off your DSVM after the workshop so you don't get charged.


### Lab: Collecting the Keys ###

Over the course of this lab, we will collect Cognitive Services keys and storage keys. You should save all of them in a text file so you can easily access them in future labs.

- _Cognitive Services Keys_
  - Computer Vision API:
  - Face API:
  - Emotion API:

- _Storage Keys_
  - Azure Blob Storage Connection String:
  - Cosmos DB URI:
  - Cosmos DB key:

**Getting Cognitive Services API Keys**

Within the Portal, we'll first create keys for the Cognitive Services we'll be using. We'll primarily be using different APIs under the [Computer Vision](https://www.microsoft.com/cognitive-services/en-us/computer-vision-api) Cognitive Service, so let's create an API key for that first.

In the Portal, hit **Create a resource** and then enter **cognitive** or **computer vision** in the search box and choose **Computer Vision API**:

![Creating a Cognitive Service Key](./resources/assets/new-cognitive-services.PNG)

This will lead you to fill out a few details for the API endpoint you'll be creating, choosing the API you're interested in and where you'd like your endpoint to reside (**eastus** or **westus** are good options), as well as what pricing plan you'd like. We'll be using **S1** so that we have the throughput we need for the tutorial. Use the same Resource Group that you used to create your DSVM. We'll be using this same resource group below for our Blob Storage and Cosmos DB as well. _Pin to dashboard_ so that you can easily find it. Since the Computer Vision API stores images internally at Microsoft (in a secure fashion), to help improve future Cognitive Services Vision offerings, you'll need to check the box that states you're ok with this before you can create the resource.

**Modifying `settings.json`, part one**

Once you have created your new API subscription, you can grab the keys from the appropriate section of the blade and add them to your _TestApp's_ and _TestCLI's_ `settings.json` file.

![Cognitive API Key](./resources/assets/cognitive-keys.PNG)

>Note: there are two keys for each of the Cognitive Services APIs you will create. Either one will work. You can read more about multiple keys [here](https://blogs.msdn.microsoft.com/mast/2013/11/06/why-does-an-azure-storage-account-have-two-access-keys/).

We'll also be using other APIs within the Cognitive Services family, so take this opportunity to create API keys for the **Emotion** and **Face** APIs as well. They are created in the same fashion as above (search for **cognitive**, **face**, or **emotion**), except you'll need to use the **S0** pricing for the Emotion API and Face API. Make sure to select _Pin to Dashboard_, and then add those keys to your `settings.json` files.



**Setting up Storage**

We'll be using two different stores in Azure for this project - one for storing the raw images, and the other for storing the results of our Cognitive Service calls. Azure Blob Storage is made for storing large amounts of data in a format that looks similar to a file-system, and it is a great choice for storing data like images. Azure Cosmos DB is our resilient NoSQL PaaS solution and is incredibly useful for storing loosely structured data like we have with our image metadata results. There are other possible choices (Azure Table Storage, SQL Server), but Cosmos DB gives us the flexibility to evolve our schema freely (like adding data for new services), query it easily, and integrate quickly into Azure Search.

_Azure Blob Storage_

Detailed "Getting Started" instructions can be [found online](https://docs.microsoft.com/en-us/azure/storage/storage-dotnet-how-to-use-blobs), but let's just go over what you need for this lab.

Within the Azure Portal, click **Create a resource->Storage->Storage Account**

![New Azure Storage](./resources/assets/create-blob-storage.PNG)

Once you click it, you'll be presented with the fields above to fill out. 

- Choose your storage account name (lowercase letters and numbers), 
- set _Account kind_ to _Blob storage_, 
- set _Replication_ to _Locally-Redundant storage (LRS)_ (this is just to save money), 
- use the same Resource Group as above, and 
- set _Location_ to _West US_.  (The list of Azure services that are available in each region is at [https://azure.microsoft.com/en-us/regions/services/](https://azure.microsoft.com/en-us/regions/services/)). _Pin to dashboard_ so that you can easily find it.

**Modifying `settings.json`, part two**

Now that you have an Azure Storage account, let's grab the _Connection String_ and add it to your _TestCLI_ and _TestApp_ `settings.json`.

![Azure Blob Keys](./resources/assets/blob-storage-keys.PNG)

_Cosmos DB_

Detailed "Getting Started" instructions can be [found online](https://docs.microsoft.com/en-us/azure/cosmos-db/documentdb-get-started), but we'll walk through what you need for this lab.

Within the Azure Portal, click **Create a resource->Databases->Azure Cosmos DB**.

![New Cosmos DB](./resources/assets/create-cosmosdb-portal.png)

Once you click this, you'll have to fill out a few fields as you see fit. 

![Cosmos DB Creation Form](./resources/assets/create-cosmosdb-formfill.png)

In our case, select the ID you'd like, subject to the constraints that it needs to be lowercase letters, numbers, or dashes. We will be using the SQL API so we can create a document database that is queryable using SQL syntax, so select `SQL` as the  API. Let's use the same Resource Group as we used for our previous steps, and the same location, select _Pin to dashboard_ to make sure we keep track of it and it's easy to get back to, and hit Create.

**Modifying `settings.json`, part three**

Once creation is complete, open the panel for your new database and select the _Keys_ sub-panel.

![Keys sub-panel for Cosmos DB](./resources/assets/docdb-keys.png)

You'll need the **URI** and the **PRIMARY KEY** for your _TestCLI's_ and the _TestApp's_ `settings.json` file, so copy those into there and you're now ready to store images and data into the cloud.


### Continue to [2_ImageProcessor](./2_ImageProcessor.md)



Back to [README](./0_readme.md)
