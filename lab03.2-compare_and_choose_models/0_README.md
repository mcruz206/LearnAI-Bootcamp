# Lab 3.2: Building and comparing models with Azure ML Workbench

Data science is a very iterative process that involves lots of trial and error. As such, the Azure Machine Learning Workbench (Workbench for short) makes it easy for data scientists to iterate on the model building and deployment process. Among other things, Workbench facilitates two common problems we encounter when developing ML solutions: 

- **Model selection**: Run and compare different models across collected metrics (with a few extra lines of python code). In this context, a single model estimate is referred to as an **experiment** and we talk about training and scoring experiments. However this term should not be confused with a statistical experiment (a randomized controlled experiment) and to avoid this confusion we try to avoid overusing the term **experiment** and use **run** or **job** instead to refer to a singe experiment and use the term **model selection** to refer to the whole process of **experimentation** (running and comparing many different experiments).
- **Model management**: Lets us bind specific models to their underlying code repository and allows the *promotion* of a model into staging and production without changes to the API servicing the model. The need for model management arises from the need to perform model selection in a tractable way and with the ability to perform rollbacks.

In this lab we will focus on how to use Workbench to facilitate model selection. In a different lab, we will learn how Workbench can also be used to do model management.

## Praparing Workbench and running a single experiment

1. Open the Workbench and create a new project called `classifying_iris`. Choose the **Classifying Iris** as the project template and `Documents` folder as its directory. Open the project and go to **File > Open Command Prompt** to access the command line from within the project parent folder. 
2. Run `az group create -n azurebootcamplab32 -l eastus2` to create a resource group called `azurebootcamplab32` for the resources we will provision throughout this lab. Then run the following command to create an Azure ML Experimentation account, a Model Management account, and a Workspace.
```
az storage account create -n azurebootcampeastus2 -g azurebootcamplab32 --sku Standard_LRS
az group deployment create -g azurebootcamplab32 --template-file template-azml.json --parameters @parameters-azml.json
```
In case the deployment results in an error message coded `RoleAssignmentUpdateNotPermitted`, we can safely ignore it as long as the assets `azureuseramlexp`, `azureuseramlws` and `azureuseramlmm` were added to the resource group. We can check all the resources under a resource group from the portal or by running `az resource list -g azurebootcamplab32 -o table`.  
FYI, as an alternative, we can also create the above from the Azure portal by navigating to [this link](https://docs.microsoft.com/en-us/azure/machine-learning/preview/quickstart-installation) and completing the section **Create Azure Machine Learning accounts**.
3. From the command prompt, run the following command to submit the training experiment: 
```
az ml experiment submit -c docker-python iris_sklearn.py
```
This runs a single training experiment. If we are doing this for the first time, then prior to running the experiment Workbench will create a docker image for us, which will take a few minutes. Once the image is ready, as long as we use the same image, we can run experiments on it quickly and without the initial delay.

## Running multiple experiments

4. Go to **File > Open Project (Code)** to edit the project scripts using Code. Open the script called `iris_sklearn.py`. Go to lines 45-48 and examine the code snippet there. We use the `sys.argv` function to pass extra arguments to Python when we run it from the command line. In our script, we have an optional regularization parameter that we can declare when we before running the experiment (otherwise it defaults to the value in line 45).
5. We will now run multiple experiments in order to perform model selection. Each experiment will consist of a model with a different regularization parameter. To make it easier to iterate over the different vaules of the regularization parameter, we have the short script called `run.py` with creates a list of regularization parameters we want to iterate over and then runs the same `az ml experiment submit` command we ran earlier, but this time with the regularization parameter explicitly passed as well. Open `run.py` in Code and change `local` to `docker-python` in line 9. Save the change.
6. Return to the command line and run `python run.py` to run multiple experiments. As the experiments are running, return to the Workbench and open the **Jobs** pannel on the right-hand side and monitor jobs as they're running.
![](./images/jobs-pannel.jpg =300x)
Click on the green **Completed** button for one of the jobs to examine the logs created by the script.
7. Once all the jobs finish running, go to the the **Runs** tab on the left-hand side and click on **All Runs**. Then examine the metrics and visualizations we are presented with.
![](./images/runs-tab.jpg =200x)
At the top we have four pannels, one showing information about the jobs we ran and the other three showing some metrics collected by each run (a run here is a single experiment).
![](./images/top-four-pannel.jpg =700x)
To see how these metrics tie back to the Python script, open `iris_sklearn.py` in **Code** and find where the model's accuracy is being logged. Hint: the Python object storing it is called `accuracy` in the script. 
8. Note that we have two ways of logging information in Workbench: 
  - We can simply rely on Python's `print` function, as can be seen by `print("Accuracy is {}".format(accuracy))` for example. In such a case, we can go the the **Jobs** pannel and click on the green **Completed** button to see any printed logs for a given run.
  ![](./images/click-completed.jpg =700x)
  - We can rely on the `get_azureml_logger` method to instantiate a logger object and use it to log information, such as in `run_logger.log("Accuracy", accuracy)`. This produces the accuracy chart from the **Runs** tab.
  ![](./images/accuracy-chart.jpg =500x)
  The biggest advantage of this second approach is that the chart has an interactive component to it. For example, we can click on the chart to select the point with the highest accuracy and as we do so, the corresponding run is automatically selected and we can examine its content by clicking on it.
  ![](./images/highest-accuracy.jpg =700x)
  We can also hold down **CTRL** and click on the next run with the highest accuracy and once again as we do so the run is automatically selected for us. With two or more selected runs, we can now click on **Compare** to compare them across various metrics.
  ![](./images/runs-table.jpg =600x)
  This allows us to compare meta-data between runs, any metrics we collected using the `get_azureml_logger` method, as well as any visualizations created by the Python script as part of the run.

## Logging new metrics 

We now add two new metrics to the logger in the `iris_sklearn.py` script, then rerun the experiments to see the additional output that is created as a result.

1. Since the training experiment `iris_sklearn.py` keeps track on precision and recall, we can use those to find the **F-score** which is a sort of average of precision and recall. In our script, the variables `precision` and `recall` are not single numbers but arrays. This is because obtain a different precision and recall by changing our probability threshold for being classified as positive. Similarly, we obtain an array of F-scores by using different threshold values, so we will also log the maximum F-score value (a single number). In **Code** paste in the below code snippet after line 71 in `iris_sklearn.py`, then save the script.
```
f_score = 2*(precision*recall)/(precision + recall)
run_logger.log("Fscore", f_score)
run_logger.log("MaxFscore", max(f_score))
print ("Max F_1 is {}".format(max(f_score)))
```
2. Return to the Workbench and go the the **Runs** tab and click on **All Runs**. Scroll down to the table listing all the runs, click on the checkbox next to `RUN NUMBER` to select them all and click on **Archive**. Repeat this until all the runs have been archived.
![](./images/archive-runs.jpg =500x)
3. From the **Command Prompt** rerun `python run.py` and go to the **Jobs** pannel to monitor jobs as they are running. Once all the jobs are finish running, click on the green **Completed** button to view their output. Find the job with regularization rate 0.009765625 and report its maximum F-score (under `Max F_1 is ...`). The output on this page is produced as a result of `print` statements in the script (or functions that return output). 
![](./images/printed-output.jpg =600x)
For the same run, now click on the blue link just above the green **Completed** button to see the **Run Properties** pane. Find the regularization rate and the F-score in this tab. The output in this pane is created partly as a result of meta-data collected for each job (such as **Start Time** and **Duration**) and partly as a result of metrics that we logged using the `run_logger.log` function. 
![](./images/run-properties.jpg =500x)
Scroll down to see the visuals created by array we logged, including the `Fscore` visual that should now also appear.
![](./images/array-visuals.jpg =500x)
Scroll further down to look at visualizations created by the Python script itself. These visualizations were not explicitly logged, but they are also tracked and presented here.
![](./images/python-visualizations.jpg =500x)
4. Click on **All Runs** from the **Runs** tab and click on the little settings icon on the right.
![](./images/run-settings.jpg =500x)
In the window that opens, put a check mark in the box next to `MaxFscore` then click on **Apply**.
![](./images/checkmark-fscore.jpg =400x)
You should now see an additional plot showing the value for `MaxFscore` accross the different runs.
5. Choose the two models with the highest `MaxFscore` (simply click on the two highest point on the chart). Notice how doing so automatically selects them in the table with all the runs just below the chart. Now click on the **Compare** button to compare the two models.
![](./images/max-fscore.jpg =600x)
Of the two models, find the one with the highest accuracy (you will find accuracy under **Logged Metrics**) and note its `runNumber` (at the very top). Then click on **Run List** to return to the table of all the runs and this time click on the `RUN NUMBER` for that model.
6. In **Run Properties** under **Outputs** select the binary object that stores our model (called `model.pkl` by the script) and click on **Promote**.
![](./images/promote-model.jpg =600x)
Click on **Project Dashboard** and then on the project path at the top. 
![](./images/project-dashboard.jpg =600x)
A Windows Explorer window will open. From there go to the `classifying_iris` folder. When we promote a model, a new folder is created in our project directory called `assets` (if there was none before) and a link to the `model.pkl` is placed there, called `model.pkl.link`. The model object can also be directly downloaded using the **Download** button under **Outputs** in **Run Properties**. Once we have the model object, we can use it to create a scoring script.  
Promoted models are registered and versioned in our **Azure Model Management** account and can be viewed in the Azure portal under the Model Management portal. This link can be used to later download the model object itself. 
![](./images/models-in-mm-portal.jpg =500x)
Later registered models can be used to create a scoring service. We learn more about this and the Model Management portal in later labs.
7. Before we finish this lab, let's just briefly go over how to programmatically do what we did above by using the Azure CLI instead of the Workbench GUI. Go to **File > Open Command Prompt** from Workbench and enter `az ml history list -o table` to get the history of runs. Select a particular run by copying its `Run_id` then paste it into the following command to see artifacts from a given run:
``` 
az ml history info --run <run_id> --artifact driver_log
```
The resulting output matches the output we saw when clicking on the green **Completed** button of a given run.  
To promote a model object, we simply run
```
az ml history promote --run <run_id> --artifact-path outputs/model.pkl --name model.pkl
```
The model object is assumed to be in `outputs/model.pkl` in the above command. The name given to the model object in the Model Management portal will also `model.pkl`. If models with this name is already in the Model Management portal, then each additional one will appear with a new version so that scoring services can later be rolled back to older models if need be.  
A promoted model object can be downloaded (and put in a folder called `output`) using the following command:
```
az ml asset download --link-file assets/model.pkl.link -d outputs
```
We will cover model management in greater detail in later labs.