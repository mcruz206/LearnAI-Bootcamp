## (optional) 4_Cosmos

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