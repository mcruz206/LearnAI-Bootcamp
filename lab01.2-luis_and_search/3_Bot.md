## 3_Bot:
Estimated Time: 30-40 minutes

## Building a Bot

We assume that you've had some exposure to the Bot Framework. If you have, great. If not, don't worry too much, you'll learn a lot in this section. We recommend completing [this Microsoft Virtual Academy course](https://mva.microsoft.com/en-us/training-courses/creating-bots-in-the-microsoft-bot-framework-using-c-17590#!) and checking out the [documentation](https://docs.microsoft.com/en-us/bot-framework/).

### Lab: Setting up for bot development

We will be developing a bot using the C# SDK.  To get started, you need two things:
1. The Bot Framework project template, which you can download [here](http://aka.ms/bf-bc-vstemplate).  The file is called "Bot Application.zip" and you should save it into the \Documents\Visual Studio 2017\Templates\ProjectTemplates\Visual C#\ directory.  Just drop the whole zipped file in there; no need to unzip.  
2. Download the Bot Framework Emulator for testing your bot locally [here](https://github.com/Microsoft/BotFramework-Emulator/releases/download/v3.5.33/botframework-emulator-Setup-3.5.33.exe).  The emulator installs to `c:\Users\`_your-username_`\AppData\Local\botframework\app-3.5.33\botframework-emulator.exe`. 

### Lab: Creating a simple bot and running it

In Visual Studio, go to File --> New Project and create a Bot Application named "PictureBot". Make sure you name it "PictureBot" or you may have issues later on.  

![New Bot Application](./resources/assets/NewBotApplication.png) 

>The rest of the **Creating a simple bot and running it** lab is optional. Per the prerequisites, you should have experience working with the Bot Framework. You can hit F5 to confirm it builds correctly, and move on to the next lab.

Browse around and examine the sample bot code, which is an echo bot that repeats back your message and its length in characters.  In particular, note:
+ In **WebApiConfig.cs** under App_Start, the route template is api/{controller}/{id} where the id is optional.  That is why we always call the bot's endpoint with api/messages appended at the end.  
+ The **MessagesController.cs** under Controllers is therefore the entry point into your bot.  Notice that a bot can respond to many different activity types, and sending a message will invoke the RootDialog.  
+ In **RootDialog.cs** under Dialogs, "StartAsync" is the entry point which waits for a message from the user, and "MessageReceivedAsync" is the method that will handle the message once received and then wait for further messages.  We can use "context.PostAsync" to send a message from the bot back to the user.  

Click F5 to run the sample code.  NuGet should take care of downloading the appropriate dependencies.  

The code will launch in your default web browser in a URL similar to http://localhost:3979/.  

> Fun Aside: why this port number?  It is set in your project properties.  In your Solution Explorer, double-click "Properties" and select the "Web" tab.  The Project URL is set in the "Servers" section.  

![Bot Project URL](./resources/assets/BotProjectUrl.jpg) 

Make sure your project is still running (hit F5 again if you stopped to look at the project properties) and launch the Bot Framework Emulator.  (If you just installed it, it may not be indexed to show up in a search on your local machine, so remember that it installs to c:\Users\your-username\AppData\Local\botframework\app-3.5.27\botframework-emulator.exe.)  Ensure that the Bot URL matches the port number that your code launched in above, and has api/messages appended to the end.  You should be able to converse with the bot.  

![Bot Emulator](./resources/assets/BotEmulator.png) 

### Lab: Update bot to use LUIS

We have to update our bot in order to use LUIS.  We can do this by using the [LuisDialog class](https://docs.botframework.com/en-us/csharp/builder/sdkreference/d8/df9/class_microsoft_1_1_bot_1_1_builder_1_1_dialogs_1_1_luis_dialog.html).  

In the **RootDialog.cs** file, add references to the following namespaces:

```csharp

using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

```

Then, change the RootDialog class to derive from LuisDialog<object> instead of IDialog<object>.  Next, give the class a LuisModel attribute with the LUIS App ID and LUIS key.  If you can't find these values, go back to http://luis.ai.  Click on your application, and go to the "Publish App" page. You can get the LUIS App ID and LUIS key from the Endpoint URL (HINT: The LUIS App ID will have hyphens in it, and the LUIS key will not).

```csharp

using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace PictureBot.Dialogs
{
    [LuisModel("96f65e22-7dcc-4f4d-a83a-d2aca5c72b24", "1234bb84eva3481a80c8a2a0fa2122f0")]
    [Serializable]
    public class RootDialog : LuisDialog<object>
    {

```

> Fun Aside: You can use [Autofac](https://autofac.org/) to dynamically load the LuisModel attribute on your class instead of hardcoding it, so it could be stored properly in a configuration file.  There is an example of this in the [AlarmBot sample](https://github.com/Microsoft/BotBuilder/blob/master/CSharp/Samples/AlarmBot/Models/AlarmModule.cs#L24).  

Next, delete the two existing methods in the class (StartAsync and MessageReceivedAsync).  LuisDialog already has an implementation of StartAsync that will call the LUIS service and route to the appropriate method based on the response.  

Finally, add a method for each intent.  The corresponding method will be invoked for the highest-scoring intent.  We will start by just displaying simple messages for each intent.  

```csharp

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hmmmm, I didn't understand that.  I'm still learning!");
        }

        [LuisIntent("Greeting")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hello!  I am a Photo Organization Bot.  I can search your photos, share your photos on Twitter, and order prints of your photos.  You can ask me things like 'find pictures of food'.");
        }

        [LuisIntent("SearchPics")]
        public async Task SearchPics(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Searching for your pictures...");
        }

        [LuisIntent("OrderPic")]
        public async Task OrderPic(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Ordering your pictures...");
        }

        [LuisIntent("SharePic")]
        public async Task SharePic(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Posting your pictures to Twitter...");
        } 

```

Once you've modified your code, hit F5 to run in Visual Studio, and start up a new conversation in the Bot Framework Emulator.  Try chatting with the bot, and ensure that you get the expected responses.  If you get any unexpected results, note them down and we will revise LUIS.  

![Bot Test LUIS](./resources/assets/BotTestLuis.jpg) 

In the above screenshot, I was expecting to get a different response when I said "order prints" to the bot.  It looks like this was mapped to the "SearchPics" intent instead of the "OrderPic" intent.  I can update my LUIS model by returning to http://luis.ai.  Click on the appropriate application, and then click on "Intents" in the left sidebar.  I could manually add this as a new utterance, or I could leverage the "suggested utterances" functionality in LUIS to improve my model.  Click on the "SearchPics" intent (or the one to which your utterance was mis-labeled) and then click "Suggested utterances".  Click the checkbox for the mis-labeled utterance, and then click "Reassign Intent" and select the correct intent.  

![LUIS Reassign Intent](./resources/assets/LuisReassignIntent.jpg) 

For these changes to be picked up by your bot, you must re-train and re-publish your LUIS model.  Click on "Publish App" in the left sidebar, and click the "Train" button and then the "Publish" button near the bottom.  Then you can return to your bot in the emulator and try again.  

> Fun Aside: The Suggested Utterances are extremely powerful.  LUIS makes smart decisions about which utterances to surface.  It chooses the ones that will help it improve the most to have manually labeled by a human-in-the-loop.  For example, if the LUIS model predicted that a given utterance mapped to Intent1 with 47% confidence and predicted that it mapped to Intent2 with 48% confidence, that is a strong candidate to surface to a human to manually map, since the model is very close between two intents.  

Now that we can use our LUIS model to figure out the user's intent, let's integrate Azure search to find our pictures.  

### Lab: Configure your bot for Azure Search 

First, we need to provide our bot with the relevant information to connect to an Azure Search index.  The best place to store connection information is in the configuration file.  

Open Web.config and in the appSettings section, add the following:

```xml    
    <!-- Azure Search Settings -->
    <add key="SearchDialogsServiceName" value="" />
    <add key="SearchDialogsServiceKey" value="" />
    <add key="SearchDialogsIndexName" value="images" />
```

Set the value for the SearchDialogsServiceName to be the name of the Azure Search Service that you created earlier.  If needed, go back and look this up in the [Azure portal](https://portal.azure.com).  

Set the value for the SearchDialogsServiceKey to be the key for this service.  This can be found in the [Azure portal](https://portal.azure.com) under the Keys section for your Azure Search.  In the below screenshot, the SearchDialogsServiceName would be "aiimmersionsearch" and the SearchDialogsServiceKey would be "375...".  

![Azure Search Settings](./resources/assets/AzureSearchSettings.jpg) 

### Lab: Update the bot to use Azure Search

Next, we'll update the bot to call Azure Search.  First, open Tools-->NuGet Package Manager-->Manage NuGet Packages for Solution.  In the search box, type "Microsoft.Azure.Search".  Select the corresponding library, check the box that indicates your project, and install it.  It may install other dependencies as well. Under installed packages, you may also need to update the "Newtonsoft.Json" package.

![Azure Search NuGet](./resources/assets/AzureSearchNuGet.jpg) 

Right-click on your project in the Solution Explorer of Visual Studio, and select Add-->New Folder.  Create a folder called "Models".  Then right-click on the Models folder, and select Add-->Existing Item.  Do this twice to add these two files under the Models folder (make sure to adjust your namespaces if necessary):
1. [ImageMapper.cs](./resources/code/Models/ImageMapper.cs)
2. [SearchHit.cs](./resources/code/Models/SearchHit.cs)

>You can find the files in this repository under [resources/code/Models](./resources/code/Models)

Next, right-click on the Dialogs folder in the Solution Explorer of Visual Studio, and select Add-->Class.  Call your class "SearchDialog.cs". Add the contents from [here](./resources/code/SearchDialog.cs).

Finally, we need to update your RootDialog to call the SearchDialog.  In RootDialog.cs in the Dialogs folder, update the SearchPics method and add these "ResumeAfter" methods:

```csharp

        [LuisIntent("SearchPics")]
        public async Task SearchPics(IDialogContext context, LuisResult result)
        {
            // Check if LUIS has identified the search term that we should look for.  
            string facet = null;
            EntityRecommendation rec;
            if (result.TryFindEntity("facet", out rec)) facet = rec.Entity;

            // If we don't know what to search for (for example, the user said
            // "find pictures" or "search" instead of "find pictures of x"),
            // then prompt for a search term.  
            if (string.IsNullOrEmpty(facet))
            {
                PromptDialog.Text(context, ResumeAfterSearchTopicClarification,
                    "What kind of picture do you want to search for?");
            }
            else
            {
                await context.PostAsync("Searching pictures...");
                context.Call(new SearchDialog(facet), ResumeAfterSearchDialog);
            }
        }

        private async Task ResumeAfterSearchTopicClarification(IDialogContext context, IAwaitable<string> result)
        {
            string searchTerm = await result;
            context.Call(new SearchDialog(searchTerm), ResumeAfterSearchDialog);
        }

        private async Task ResumeAfterSearchDialog(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Done searching pictures");
        }

```

Press F5 to run your bot again.  In the Bot Emulator, try searching with "find dog pics" or "search for happiness photos".  Ensure that you are seeing results when tags from your pictures are requested.  


### Continue to [4_Bot_Enhancements](./4_Bot_Enhancements.md)

Back to [README](./0_README.md)