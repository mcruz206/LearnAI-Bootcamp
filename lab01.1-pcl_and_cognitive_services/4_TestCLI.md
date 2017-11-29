## (optional) 4_TestCLI
Estimated Time: 10-15 minutes

## (optional) Exploring Cosmos DB

Azure Cosmos DB is our resilient NoSQL PaaS solution and is incredibly useful for storing loosely structured data like we have with our image metadata results. There are other possible choices (Azure Table Storage, SQL Server), but Cosmos DB gives us the flexibility to evolve our schema freely (like adding data for new services), query it easily, and can be quickly integrated into Azure Search (which we'll do in `lab01.2-luis_and_search`).

Cosmos DB is not a focus of this workshop, but if you're interested in what's going on - here are some highlights from the code we will be using:
- Navigate to the `DocumentDBHelper.cs` class in the `ImageStorageLibrary`. Many of the implementations we are using can be found in the [Getting Started guide](https://docs.microsoft.com/en-us/azure/cosmos-db/documentdb-get-started).
- Go to `TestCLI`'s `Util.cs` and review  the `ImageMetadata` class. This is where we turn the `ImageInsights` we retrieve from Cognitive Services into appropriate Metadata to be stored into Cosmos DB.
- Finally, look in `Program.cs` and notice in `ProcessDirectoryAsync`. First, we check if the image and metadata have already been uploaded - we can use `DocumentDBHelper` to find the document by ID and to return `null` if the document doesn't exist. Next, if we've set `forceUpdate` or the image hasn't been processed before, we'll call the Cognitive Services using `ImageProcessor` from the `ImageProcessingLibrary` and retrieve the `ImageInsights`, which we add to our current `ImageMetadata`. 
- Once all of that is complete, we can store our image - first the actual image into Blob Storage using our `BlobStorageHelper` instance, and then the `ImageMetadata` into Cosmos DB using our `DocumentDBHelper` instance. If the document already existed (based on our previous check), we should update the existing document. Otherwise, we should be creating a new one.

### (optional) Lab: Loading Images using TestCLI

We will implement the main processing and storage code as a command-line/console application because this allows you to concentrate on the processing code without having to worry about event loops, forms, or any other UX related distractions. Feel free to add your own UX later.

Once you've set your Cognitive Services API keys, your Azure Blob Storage Connection String, and your Cosmos DB Endpoint URI and Key in your _TestCLI's_ `settings.json`, you can run the _TestCLI_.

Run _TestCLI_, then open Command Prompt and navigate to your Starting-ImageProcessing\TestCLI folder (Hint: use the "cd" command to change directories). Then enter `.\bin\Debug\TestCLI.exe`. You should get the following result:

```
    > .\bin\Debug\TestCLI.exe

    Usage:  [options]

    Options:
    -force            Use to force update even if file has already been added.
    -settings         The settings file (optional, will use embedded resource settings.json if not set)
    -process          The directory to process
    -query            The query to run
    -? | -h | --help  Show help information
```

By default, it will load your settings from `settings.json` (it builds it into the `.exe`), but you can provide your own using the `-settings` flag. To load images (and their metadata from Cognitive Services) into your cloud storage, you can just tell _TestCLI_ to `-process` your image directory as follows:

```
    > .\bin\Debug\TestCLI.exe -process c:\my\image\directory
```

Once it's done processing, you can query against your Cosmos DB directly using _TestCLI_ as follows:

```
    > .\bin\Debug\TestCLI.exe -query "select * from images"
```

### Continue to [5_Challenge_and_Closing](./5_Challenge_and_Closing.md)



Back to [README](./0_readme.md)