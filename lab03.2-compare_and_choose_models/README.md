# Lab 3.2: Model management using the Azure Machine Learning Workbench

Here is the high-level architecture of an end-to-end solution with Azure ML Workbench (or Workbench for short) handling both the development and operationalization of a Machine Learning model. We should return to this chart as we run through this lab to see how all the pieces come together.

<div style="text-align:center"><img src ="https://docs.microsoft.com/en-us/azure/machine-learning/preview/media/overview-general-concepts/hierarchy.png" width="800"/></div>

The three main Azure resources we will consume in this lab are as follows:

 - An **Experimentation account** contains workspaces, which is where our projects are sitting. When working in teams, we can add multiple users or "seats" to an experimentation account. To use the Workbench and run experiments, we must create an experimentation account.
 - A **Model Management account** is for managing models. Model management is essential to both model development and deployment. We use a model management account to register models and bundle models and code (including dependencies) into **manifests**. Manifests are in turn used to create Docker **images**, and those images in turn build containerized web services that run instances of our deployed application, locally or in the cloud.
 - An **environment** denotes a particular computing resource that is used for deploying and managing models. It can be a local computer, a Linux VM on Azure, or a Kubernetes cluster running in Azure Container Service. A model hosted in a Docker container runs in these environments and is exposed as a REST API endpoint.

Source: https://docs.microsoft.com/en-us/azure/machine-learning/preview/overview-general-concepts

After this lab is finished, we will have a better idea of how to use the Workbench and accompanying Azure services in order to

- minimize the time and effort that goes into the iterative process of building and evaluating ML models
- smooth out the transition of going from development to production for operationalizing ML models
- get back to doing more data science and less administrative or devops types of tasks

### 3.1 Running the modeling script in Docker

Our task in this section is to successfully run the project in a Docker container. To do so, we will need to start a new Workbench project based on an existing template and make a set of changes to the project in order to run it successfully. 

1. Open the Workbench and press **CTRL+N** to create a new project. Name the project `churn_prediction` and use the `Documents` folder as the project directory. Finally, in the box called `Search Project Templates`, type `churn` and select the template called `Customer Churn Prediction`. Press **Create** to create the project.
2. Go to **File > Open Project (Code)** to edit the project scripts using Code. Find the script `CATelcoCustomerChurnModelingDocker.py` and replace line 13 with the following line: `df = pd.read_csv('data/CATelcoCustomerChurnTrainingSample.csv')`.
3. Go to `aml_config/docker.compute` and replace line 2 with `baseDockerImage: "microsoft/mmlspark:plus-0.7.91"`.
4. Go to `aml_config/docker.runconfig` and replace its content with 

```
ArgumentVector:
  - "$file"
Target: "docker"
EnvironmentVariables:
  "EXAMPLE_ENV_VAR": "Example Value"
Framework: "PySpark"
CondaDependenciesFile: "aml_config/conda_dependencies.yml"
SparkDependenciesFile: "aml_config/spark_dependencies.yml"
PrepareEnvironment: true
TrackedRun: true
```

5. On Code, go to **File > Save All** to save all the above changes. Then return to the Workbench and check to make sure the changes are visible here. We can check by clicking on the **Files** tab on the left pannel and opening one of the files we changed.
6. In order to run the experiment in a Docker container, we must prepare a Docker image. We will do so programatically by going to **File > Open Command Prompt** and typing `az ml experiment prepare -c docker`. Notice all the changes that are happening as this command is running. This should take a few minutes.
  **Note:** At this point, there is a strange Docker behavior for which we propose an easy solution: we may get an error at the top about `image operating system "linux" cannot be used on this platform`.

  <div style="text-align:center"><img src ="./images/linux-image-not-found.jpg" width="600"/></div>

  To resolve it we click on the Docker logo on the right-hand side in the taskbar and switch Docker to use Windows containers. This will result in a new Docker error:

  <div style="text-align:center"><img src ="./images/docker-windows-image.jpg" width="300"/></div>

  Now we switch Docker back to Linux containers (by going to the taskbar once more).

  <div style="text-align:center"><img src ="./images/switch-linux-containers.jpg" width="500"/></div>

  We then return to the command prompt and run the above command again. This will take a few minutes. When finished, we should get a message saying `Your environment is now ready`.

7. We can now run our experiment in a Docker container by submitting the following command: `az ml experiment submit -c docker CATelcoCustomerChurnModelingDocker.py`. Alternatively, we can go to the **Project Dashboard**, select "docker" as the run configuration, select the `CATelcoCustomerChurnModelingDocker.py` script and click on the run button. In either case, we should be able to see a new job starting on the **Jobs** in the pannel on the right-hand side. Click on the finished job to see the **Run Properties** such as **Duration**. Notice under **Outputs** there are no objects, so the script did not create any artifacts. Click on the green **Completed** to see any results printed by the script, including the model accuracy. It is worth noting that the Azure CLI runs on both the Windows and Linux command line. To see this in action, from the Windows command prompt type `bash` to switch to a Linux command prompt and submit `az ml experiment submit -c docker CATelcoCustomerChurnModelingDocker.py` a second time.
8. Return to Code and add the following code snippet to the bottom of `CATelcoCustomerChurnModelingDocker.py` and rerun the experiment. The purpose of the code snippet is to serialize the model on disk in the `outputs` folder.

```
import pickle

print ("Export the model to model.pkl")
f = open('./model.pkl', 'wb')
pickle.dump(dt, f)
f.close()
```

9. Rerun the experiment and when finished click on the job and notice the output `model.pkl` in the **Run Properties** pane under **Outputs**. Select this output and download it and place it in new folder called `outputs` under the project directory.

### 3.2 Creating a web service out of the scoring script

Let's now see how we can create a scoring web service from the above model inside a docker image. There are multiple steps that go into doing that. We will be running commands from the command line, but we will also log into the Azure portal in order to see which resources are being created as we run various Azure CLI commands.

1. Using Code, replace line 16 in `score.py` with `model = joblib.load('./model.pkl')` (keeping the indentation). Save the change.
2. Run `az group create -n azurebootcamplab32 -l eastus2` to create a resource group called `azurebootcamplab32` for the resources we will provision throughout this lab. Then run the following command to create an Azure ML Experimentation account, a Model Management account, and a Workspace.
```
az storage account create -n azurebootcampeastus2 -g azurebootcamplab32 --sku Standard_LRS
az group deployment create -g azurebootcamplab32 --template-file template-azml.json --parameters @parameters-azml.json
```
In case the deployment results in an error message coded `RoleAssignmentUpdateNotPermitted`, we can safely ignore it as long as the assets `azureuseramlexp`, `azureuseramlws` and `azureuseramlmm` were added to the resource group. We can check all the resources under a resource group from the portal or by running `az resource list -g azurebootcamplab32 -o table`.

FYI, as an alternative, we can also create the above from the Azure portal by navigating to [this link](https://docs.microsoft.com/en-us/azure/machine-learning/preview/quickstart-installation) and completing the section **Create Azure Machine Learning accounts**.
3. Log into the Azure portal and find all the resources under the resource group `azurebootcamplab32`. This should include an Experimentation and a Model Management account. Open the Model Management resource and click on **Model Management** icon on the top.
4. If we're doing this for the first time, then we need to set up an environment. We usually have a staging and a production environment. We can deploy our models to the staging environment to test them and then redeploy them to the production environment once we're happy with the result. To create a new environment run `az ml env setup --cluster -l eastus2 -n bootcampvmstage -g azurebootcamplab32`. We can look at all the environments uder our subscription using `az ml env list -o table`. Creating the new environment takes about one minute, after which we can activate it using `az ml env set -n bootcampvmstage -g azurebootcamplab32` and list it using `az ml env show`.
5. We next set our Model Management account by running `az ml account modelmanagement set -n azureuseramlmm -g azurebootcamplab32`.
6. We are now finally ready to deploy our model as a web service. We do so by running `az ml service create realtime -n churnpred --model-file ./model.pkl -f score.py -r python -s service_schema.json`. Notice the three steps that take place as the command is running. First we register the model, then we create a manifest, then we create a Docker image, and finally we initialize a Docker container that services our prediction app. We can go to the Azure portal and go to click on the resource named `azureuseramlmm` under the resource group `azurebootcamplab32`, then click on **Model Management**.
<div style="text-align:center"><img src ="./images/model-management-portal.jpg" width="600"/></div>
In the Model Management portal, we can view the three resources that are created as the above command runs: the manifest, the image, and the service. Click on each to view the resources.
<div style="text-align:center"><img src ="./images/model-management-services.jpg" width="600"/></div>
7. We will now recreate the same service, but in three separate commands instead of one command as we did adove. This will help us better understands how one steps leads to the next. First we will register the model object `model.pkl`.
```
az ml model register -m model.pkl -n model.pkl
```
Run `az ml model list -o table` and we can see that two different versions of the same model now exist. This is because the model was registered under the same name. We can now create a manifest for the model in Azure Container Services. To do so, in the next command, we replace `<model_id>` with the model ID that was returned in the last command:
```
az ml manifest create -n churnpred -f score.py -s service_schema.json -r python -i <model_id>
```
We can run `az ml manifest list -o table` to list our manifests, which returns the manifest ID. We can then replace `<manifest_id>` in the next command with our manifest ID to create an image from a model manifest.
```
az ml image create -n churnpred --manifest-id <manifest_id>
```
This creates the image and places it in the Azure container registry, which can be thought of as a compute environment for machine learning models on Azure. The above command creates an image with a matching image ID. We can look up the manifest ID and the image ID from the Azure portal as well. Finally, the last step is to create a service out of the image we have:
```
az ml service update realtime -i <service_id_on_portal> --image-id <new_image_id>
```
Notice that since a service already existed for the model, we used `az ml service update` to update it instead of `az ml service create` to create a new one. Updating the service is a preferable options for models in production because we won't need to make to make any changes to the REST APIs calling the service.
8. Let's now create a new environment that plays the role of a *production* environment for us. This could be a bigger VM serving as a production VM or a VM in a different location so the service can be available in multiple locations.
```
az ml env setup --cluster -l eastus2 -n bootcampvmprod -g azurebootcamplab32
```
We can create a new service programmatically in the same way we did earlier. But we can also go to the Model Management portal, select the image and choose **Create Service** and switch the environment to the production environment `bootcampvmprod` we just created.