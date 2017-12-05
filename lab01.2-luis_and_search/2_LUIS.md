## 2_Luis:
Estimated Time: 20-30 minutes

## LUIS

First, let's [learn about Microsoft's Language Understand Intelligent Service (LUIS)](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/Home).

Now that we know what LUIS is, we'll want to plan our LUIS app. We'll be creating a bot that returns images based on our search, that we can then share or order. We will need to create intents that trigger the different actions that our bot can do, and then create entities to model some parameters than are required to execute that action. For example, an intent for our PictureBot may be "SearchPics" and it triggers the Search service to look for photos, which requires a "facet" entity to know what to search for. You can see more examples for planning your app [here](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/plan-your-app).

Once we've thought out our app, we are ready to [build and train it](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/luis-get-started-create-app). These are the steps you will generally take when creating LUIS applications:
  1. [Add intents](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/add-intents) 
  2. [Add utterances](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/add-example-utterances)
  3. [Add entities](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/add-entities)
  4. [Improve performance using features](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/add-features)
  5. [Train and test](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/train-test)
  6. [Use active learning](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/label-suggested-utterances)
  7. [Publish](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/publishapp)


### Lab: Creating the LUIS service in the portal

In the Portal, hit **Create a resource** and then enter **LUIS** in the search box and choose **Language Understanding Intelligent Service**:

This will lead you to fill out a few details for the API endpoint you'll be creating, choosing the API you're interested in and where you'd like your endpoint to reside, as well as what pricing plan you'd like. The free tier is sufficient for this lab. Since LUIS stores images internally at Microsoft (in a secure fashion), to help improve future Cognitive Services offerings, you'll need to check the box to confirm you're ok with this.

Once you have created your new API subscription, you can grab the key from the appropriate section of the blade and add it to your list of keys.

![Cognitive API Key](./resources/assets/cognitive-keys.PNG)

### Lab: Adding intelligence to your applications with LUIS

In the next lab, we will create our PictureBot. First, let's look at how we can use LUIS to add some natural language capabilities. LUIS allows you to map natural language utterances (words/phrases/sentences the user might say when talking to the bot) to intents (tasks or actions the user wants to perform).  For our application, we might have several intents: finding pictures, sharing pictures, and ordering prints of pictures, for example.  We can give a few example utterances as ways to ask for each of these things, and LUIS will map additional new utterances to each intent based on what it has learned.  

Navigate to [https://www.luis.ai](https://www.luis.ai) and sign in using your Microsoft account.  (This should be the same account that you used to create the LUIS key in the previous section).  You should be redirected to a list of your LUIS applications at [https://www.luis.ai/applications](https://www.luis.ai/applications).  We will create a new LUIS app to support our bot.  

> Fun Aside: Notice that there is also an "Import App" next to the "New App" button on [the current page](https://www.luis.ai/applications).  After creating your LUIS application, you have the ability to export the entire app as JSON and check it into source control.  This is a recommended best practice, so you can version your LUIS models as you version your code.  An exported LUIS app may be re-imported using that "Import App" button.  If you fall behind during the lab and want to cheat, you can click the "Import App" button and import the [LUIS model](./resources/code/LUIS/PictureBotLuisModel.json).  

From [https://www.luis.ai/applications](https://www.luis.ai/applications), click the "New App" button.  Give it a name (I chose "PictureBotLuisModel") and set the Culture to "English".  You can optionally provide a description. Then click "Create".  

![LUIS New App](./resources/assets/LuisNewApp.png) 

You will be taken to a Dashboard for your new app.  The App Id is displayed; note that down for later as your **LUIS App ID**.  Then click "Create an intent".  

![LUIS Dashboard](./resources/assets/LuisDashboard.jpg) 

We want our bot to be able to do the following things:
+ Search/find pictures
+ Share pictures on social media
+ Order prints of pictures
+ Greet the user (although this can also be done other ways as we will see later)

Let's create intents for the user requesting each of these.  Click the "Add intent" button.  

Name the first intent "Greeting" and click "Save".  Then give several examples of things the user might say when greeting the bot, pressing "Enter" after each one.  After you have entered some utterances, click "Save".  

![LUIS Greeting Intent](./resources/assets/LuisGreetingIntent.jpg) 

Let's see how to create an entity.  When the user requests to search the pictures, they may specify what they are looking for.  Let's capture that in an entity.  

Click on "Entities" in the left-hand column and then click "Add custom entity".  Give it an entity name "facet" and entity type "Simple".  Then click "Save".  

![Add Facet Entity](./resources/assets/AddFacetEntity.jpg) 

Next, click "Intents" in the left-hand sidebar and then click the yellow "Add Intent" button.  Give it an intent name of "SearchPics" and then click "Save".  

Just as we did for Greetings, let's add some sample utterances (words/phrases/sentences the user might say when talking to the bot).  People might search for pictures in many ways.  Feel free to use some of the utterances below, and add your own wording for how you would ask a bot to search for pictures. 

+ Find outdoor pics
+ Are there pictures of a train?
+ Find pictures of food.
+ Search for photos of a 6-month-old boy
+ Please give me pics of 20-year-old women
+ Show me beach pics
+ I want to find dog photos
+ Search for pictures of women indoors
+ Show me pictures of girls looking happy
+ I want to see pics of sad girls
+ Show me happy baby pics

Once we have some utterances, we have to teach LUIS how to pick out the **search topic** as the "facet" entity. Whatever the "facet" entity picks up is what will be searched. Hover and click over the word (or drag to select a group of words) and then select the "facet" entity. 

![Labelling Entity](./resources/assets/LabellingEntity.jpg) 

So the following list of utterances...

![Add Facet Entity](./resources/assets/SearchPicsIntentBefore.jpg) 

...may become something like this when the facets are labeled.  

![Add Facet Entity](./resources/assets/SearchPicsIntentAfter.jpg) 

Don't forget to click "Save" when you are done!  

Finally, click "Intents" in the left sidebar and add two more intents:
+ Name one intent **"SharePic"**.  This might be identified by utterances like "Share this pic", "Can you tweet that?", or "post to Twitter".  
+ Create another intent named **"OrderPic"**.  This could be communicated with utterances like "Print this picture", "I would like to order prints", "Can I get an 8x10 of that one?", and "Order wallets".  
When choosing utterances, it can be helpful to use a combination of questions, commands, and "I would like to..." formats.  

Note too that there is one intent called "None".  Random utterances that don't map to any of your intents may be mapped to "None".  

We are now ready to train our model.  Click "Train & Test" in the left sidebar.  Then click the train button.  This builds a model to do utterance --> intent mapping with the training data you've provided. Training is not always immediate. Sometimes, it gets queued and can take several minutes.

Then click on "Publish App" in the left sidebar.  You have several options when you publish your app, including enabling [verbose endpoint response or Bing spell checker](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/PublishApp). If you have not already done so, select the endpoint key that you set up earlier, or follow the link to add a key from your Azure account.  You can leave the endpoint slot as "Production".  Then click "Publish".  



![Publish LUIS App](./resources/assets/PublishLuisApp.png) 

Publishing creates an endpoint to call the LUIS model.  The URL will be displayed, which will be explained in a later lab.

Click on "Train & Test" in the left sidebar.  Check the "Enable published model" box to have the calls go through the published endpoint rather than call the model directly. Try typing a few utterances and see the intents returned.  
>Unfortunately, there is a bug open with "Enable published model", and it only works in Chrome. You can either download Chrome and try this, or skip it but remember that it's not enabled.

![Test LUIS](./resources/assets/TestLuis.png) 

You can also [test your published endpoint in a browser](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/PublishApp#test-your-published-endpoint-in-a-browser). Copy the URL, then replace the `{YOUR-KEY-HERE}` with one of the keys listed in the Key String column for the resource you want to use. To open this URL in your browser, set the URL parameter `&q` to your test query. For example, append `&q=Find pictures of dogs` to your URL, and then press Enter. The browser displays the JSON response of your HTTP endpoint.

**Finish early? Try these extra credit tasks:**


Create additional entities that can be leveraged by the "SearchPics" intent. For example, we know that our app determines age - try creating a prebuilt entity for age. 

Explore using custom entities of entity type "List" to capture emotion and gender. See the example of emotion below. 

![Custom Emotion Entity with List](./resources/assets/CustomEmotionEntityWithList.jpg) 

> Note: When you add more entities or features, don't forget to go to **Intents>Utterances** and confirm/add more utterances with the entities you add. Also, you will need to retrain and publish your model.


### Continue to [3_Bot](./3_Bot.md)

Back to [README](./0_readme.md)