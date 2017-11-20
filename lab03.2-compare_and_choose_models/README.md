# Lab 3.2: Model management using the Azure Machine Learning Workbench

Here is the high-level architecture of a machine learning end-to-end solution using the Azure ML Workbench. We encourage you to return to this chart as you run through this lab to see how all the pieces come together.

<div style="text-align:center"><img src ="https://docs.microsoft.com/en-us/azure/machine-learning/preview/media/overview-general-concepts/hierarchy.png" width="800"/></div>

The three main Azure resources we will deal with are the following:

 - An **Experimentation account** contains workspaces, which contain our projects. We can add multiple users or "seats" to an experimentation account. To use the Workbench and run experiments, we must create an experimentation account.
 - A **Model Management** account is for managing models. We use it to register models and bundle the models and its code and dependencies into **manifests**, which are in turn used to create Docker images, which in turn build containerized web services that can be deployed locally or in the cloud.
 - An **environment** denotes a particular computing resource that is used for deploying and managing your models. It can be your local computer, a Linux VM on Azure, or a Kubernetes cluster running in Azure Container Service, depending on context and configuration. Your model is hosted in a Docker container running in these environments and exposed as a REST API endpoint.

Source: https://docs.microsoft.com/en-us/azure/machine-learning/preview/overview-general-concepts

## Preparing the environment

Welcome to model management using the Azure Machine Learning Workbench. To run this lab, we will be using the Data Science Virtual Machine (DSVM) on Azure. If you haven't already, please follow these steps to get the DSVM ready for the lab.

1. Open this location and follow the instructions you see [here](https://docs.microsoft.com/en-us/azure/machine-learning/data-science-virtual-machine/provision-vm). Make sure to
  a) Select the Data Science Virtual Machine - Windows 2016;
  b) Select SDD Drives;
  c) Select D4S_v3 as the size (only available in [certain locations](https://azure.microsoft.com/en-us/blog/introducing-the-new-dv3-and-ev3-vm-sizes)).
2. Log in to the Data Science Virtual Machine (DSVM), open Server Manager, and use the **Add roles and features** wizard to add Hyper-V (use all default settings). If you launch Docker without doing this, you will be prompted by Docker to enable Hyper-V. Say yes and log into the server after it reboots.
3. Open Firefox (if on the DSVM) and navigate to [https://aka.ms/azureml-wb-msi](https://aka.ms/azureml-wb-msi), run the downloader to install AMLS Workbench.
4. Next, download and run the [Docker installer](https://download.docker.com/win/stable/Docker%20for%20Windows%20Installer.exe) for Windows. You will be prompted to logout upon completion, do NOT log out until AMLS Workbench has finished installing.
5. If you haven't done so already, navigate to [this link](https://docs.microsoft.com/en-us/azure/machine-learning/preview/quickstart-installation) and complete only the **Create Azure Machine Learning accounts** section.
6. Once everything is installed on the DSVM restart.

## Preparing the Workbench to run the project

1. Log into the DSVM and launch Docker for Windows.
2. While you are waiting for Docker to start running, open the Azure Machine Learning Workbench. You will be prompted to authenticate using you Azure account. Please do so.
3. Click on your initials at the bottom-left corner of the Workbench and make sure that you are using the correct account (this should be an Experimentation account with a matching Model Management account).
4. Go to **File > Configure Project IDE** and name your IDE `Code` with the following path `C:\Program Files\Microsoft VS Code\Code.exe`. This will allow you to open the entire project in Visual Studio Code, which is our editor of choice for this lab.
5. Go to **File > Open Project (VSCode)** to open the project in Visual Studio Code. It is not necessary to use Code to make edit our course files but it is much more convenient. We will return to Code when we need to make changes to the existing scripts.
6. We now log into the Azure CLI using our Azure account. Return to the Workbench and go to **File > Open Command Prompt**. Check that the Azure CLI is installed on the DSVM by typing `az -h`. Now type `az login` and copy the access code you are given. In Firefox open a **private tab** using **CTRL+SHIFT+P** then enter the URL `aka.ms/devicelogin` and when prompted, paste in the access code. You will next be prompted to authenticate using your Azure account.
7. We now set the Azure CLI to use the right Azure account. From the command prompt, enter `az account list –o table` to see your available accounts. Then copy the subscription ID from the Azure account you used to create your AML Workbench account and type `az account set –s <subscription_id>`, replacing `<subscription_id>` with the account ID.

## Running the modeling script in Docker

Our next task is to successfully run the project in a Docker container. To do so, we will need to make a set of changes to the project.

1. Go to Code and find the script `CATelcoCustomerChurnModelingDocker.py` and replace line 13 with the following line: `df = pd.read_csv('data/CATelcoCustomerChurnTrainingSample.csv')`.
2. Go to `aml_config/docker.compute` and replace line 2 with `baseDockerImage: "microsoft/mmlspark:plus-0.7.91"`.
3. Go to `aml_config/docker.runconfig` and replace its content with 

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

4. On Code, go to **File > Save All** to save all the above changes. Then return to the Workbench and go to **File > Save** for the saved changes to take effect.
5. In order to run the experiment in a Docker container, we must prepare a Docker image. We will do so programatically by going to **File > Open Command Prompt** and typing `az ml experiment prepare -c docker`. Notice all the changes that are happening as this command is running. This should take a few minutes.
  **Note:** At this point, if you get an error at the top about `image operating system "linux" cannot be used on this platform`, you need to click on the Docker logo on the right-hand side in the taskbar and switch Docker to use Windows containers (which will result Docker saying there was an error) and then back to Linux containers. Then run the above command again. When finished, you should get a message saying `Your environment is now ready`.
6. We can now run our experiment in a Docker container by submitting the following command: `az ml experiment submit -c docker CATelcoCustomerChurnModelingDocker.py`. Alternatively, we can go to the **Project Dashboard**, select "docker" as the run configuration, select the `CATelcoCustomerChurnModelingDocker.py` script and click on the run button. In either case, we should be able to see a new job starting on the **Jobs** in the pannel on the right-hand side. Click on the finished job to see the **Run Properties** such as **Duration**. Notice under **Outputs** there are no objects, so the script did not create any artifacts. Click on the green **Completed** to see any results printed by the script, including the model accuracy.
7. Return to Code and ddd the following code snippet to the bottom of `CATelcoCustomerChurnModelingDocker.py` and rerun the experiment. The purpose of the code snippet is to serialize the model on disk in the `outputs` folder.

```
import pickle

print ("Export the model to model.pkl")
f = open('./outputs/model.pkl', 'wb')
pickle.dump(dt, f)
f.close()
```

8. Rerun the experiment and when finished click on the job and notice the output `model.pkl` in the **Run Properties** pane under **Outputs**. Select this output and download it and place it in new folder called `outputs` under the project directory.

## Creating a web service out of the scoring script

Let's now see how we can create a scoring web service from the above model  inside a docker image. There are multiple steps that go into doing that. We will be running commands from the command line, but we will also log into the Azure portal in order to see which resources are being created as we run various Azure CLI commands.

1. Using Code, replace line 16 in `score.py` with `model = joblib.load('./outputs/model.pkl')` (keeping the indentation). Save your change.
2. Log into the Azure portal and find all the resources under your resource group `sethmottaml`. This should include an Experimentation and a Model Management account. Open the Model Management resource and click on **Model Management** icon on the top.
3. If we're doing this for the first time, then we need to set up an environment. We usually have a staging and a production environment. We can deploy our models to the staging environment to test them and then redeploy them to the production environment once we're happy with the result. To create a new environment run `az ml env setup -l eastus2 -n sethmottamlstage -g sethmottaml`. We can look at all the environments uder our subscription using `az ml env list -o table`. Creating the new environment takes about one minute, after which we can activate it using `az ml env set -n sethmottamlstage -g sethmottaml` and list it using `az ml env show`.
4. We next set our Model Management account by running `az ml account modelmanagement set -n sethmottmm -g sethmottaml`.
5. We are now finally ready to deploy our model as a web service. We do so by running `az ml service create realtime -n churnpred --model-file ./outputs/model.pkl -f score.py -r python -s service_schema.json`. Notice the three steps that take place as the command is running. First we register the model, then we create a manifest, then we create a Docker image, and finally we initialize a Docker container that services our prediction app.

[[At this point I'm stuck because the command above creates a model manifest and an image, but not a scoring service, and when I try to create a scoring service directly out of the image from the Model Management portal, I get an error message.]]

```
az ml model register -m model.pkl -n model.pkl
az ml model list -o table

# specify the model ID to create a manifest
az ml manifest create -n churnpred -f score.py -s service_schema.json -r python -i afd3180d4eec4cd48110b71494d6b1ce

# specify the manifest ID to create an image
az ml manifest list -o table
az ml image create -n churnpred --manifest-id b6a91c86-1624-48b8-8e74-f18e2f52638f
# this create the image and place it in the Azure container registry, which is a compute environment for machine learning models on Azure.

az ml service update realtime -i service_id_on_portal --image-id new_image_id

# Now from the portal, select the image and choose **Create Service**. Switch the environment to a production environment. You can create one using `az ml env setup -l eastus2 -n sethmottamlprod -g sethmottaml`. Once a service is created you can also click on the AppInsights link to monitor the model.

az ml service create realtime -n iris --model-file model.pkl -f iris_score.py -r python -s service_schema.json
registers model
creates manifest

```
