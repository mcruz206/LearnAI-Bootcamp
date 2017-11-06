# lab03.3-deploy_to_remote_environment - Deploying Your Machine Learning Model To A Remote Environment
This hands-on lab guides you through deploying a Machine Learning algorithm to a remote environment using [Azure Machine Learning Services](https://docs.microsoft.com/en-us/azure/machine-learning/preview/overview-what-is-azure-ml) with the Azure Machine Learning Workbench. 

In this workshop, you will:
- [ ] Understand how to deploy your Experiments on remote Data Science Virtual Machines 
- [ ] Understand how to deploy your Experiments on remote Data Science VMs with GPU's
- [ ] Understand how to deploy your Experiments on HDInsight Clusters running Spark

You'll focus on the objectives above, not Data Science, Machine Learning or a difficult scenario.  

***NOTE:*** There are several pre-requisites for this course, including an understanding and implementation of: 
  *  Programming using an Agile methodology
  *  Maching Learning and Data Science

There is a comprehensive Learning Path you can use to prepare for this course [located here](https://github.com/Azure/learnAnalytics-CreatingSolutionswiththeTeamDataScienceProcess-/blob/master/Instructions/Learning%20Path%20-%20Creating%20Solutions%20with%20the%20Team%20Data%20Science%20Process.md).

## Introduction and setup 
[Primary Concepts are here](https://docs.microsoft.com/en-us/azure/machine-learning/preview/experimentation-service-configuration)

***NOTE***: These steps must be completed ***prior*** to attempting this workshop.
  *  You will use Visual Studio Team Services as your code-control system. First, [read all information in this link](https://docs.microsoft.com/en-us/azure/machine-learning/preview/using-git-ml-project), and then perform steps 1-2 **only**. Record the git repo location from the VSTS location you created and bring it to class with you.
  *  You will need a Microsoft Azure account. You can use a production Azure account if you are able to create objects. You can also use your Microsoft Developer Network (MSDN) account (if you have one) to complete this workshop. If you don't have access to a corporate or MSDN account you can create a free account [using this process](https://azure.microsoft.com/free/).
  *  You will need an Azure Machine Learning Services account. [Open this reference](https://docs.microsoft.com/en-us/azure/machine-learning/preview/quickstart-installation), and complete only the sections marked **"Sign in to the Azure portal"** and **"Create Azure Machine Learning accounts"**. Write down the *Experimentation account name* and bring it to class.
  *  You can install the Azure Machine Learning Workbench locally:
        *  [Open this reference](https://docs.microsoft.com/en-us/azure/machine-learning/preview/quickstart-installation) and follow the sections marked **Install Azure Machine Learning Workbench on Windows** or choose your OS type from the instructions there.

  *  Or you can use a Windows Data Science Vitual Machine (DSVM) to run this lab: 
        *  [Navigate to this path](https://azuremarketplace.microsoft.com/en-us/marketplace/apps/microsoft-ads.windows-data-science-vm), and create a Windows Azure Data Science Virtual Machine (DSVM). Choose a VM size of: DS3_V2, with 4 virtual CPUs and 14-Gb RAM. When the DSVM is deployed, start it using the [Azure portal.](https://portal.azure.com)
        *  After you create and Start the DSVM, log in to it and double-click the "Install Azure Machine Learning Workbench" icon. Finish the installation by following the on-screen instructions. The installer downloads all the necessary dependent components, such as Python, Miniconda, and other related libraries. The installation might take around half an hour to finish all the components. When complete, the Azure Machine Learning Workbench is installed in the following directory: C:\Users\<user>\AppData\Local\AmlWorkbench

## Deploying an Experiment Locally - TODO
TODO

The Azure ML environment has this configuration: 
![AMLS Environment](https://docs.microsoft.com/en-us/azure/machine-learning/preview/media/overview-general-concepts/hierarchy.png)

TODO 

![Local AMLS Experiment run](https://docs.microsoft.com/en-us/azure/machine-learning/preview/media/experimentation-service-configuration/local-native-run.png)
### Lab: Deploy an Experiment Locally
In this lab you'll create an experiment, examine it's configuration, and run the experiment locally. You'll set up the experiment in the AMLS Workbench tool, and then run all experiments from the command line interface (CLI)
- [ ] Open the Azure Machine Learning Services Workbench tool locally or on your Data Science Virutal Machine. 
- [ ] [Navigate to this resource, and ](https://docs.microsoft.com/en-us/azure/machine-learning/preview/experimentation-service-configuration).

## Deploying an Experiment to a remote Data Science Virtual Machine in Azure - TODO
TODO

You configure the  Azure ML experiment flow using this process: 
![AMLS Experiment Flow](https://docs.microsoft.com/en-us/azure/machine-learning/preview/media/experimentation-service-configuration/experiment-execution-flow.png)

### Lab: Deploy an Experiment to a remote Data Science Virtual Machine
In this lab you'll TODO
- [ ] [TODO](Create a new workbench project), 


## Workshop Completion
In this workshop you learned how to:
- [ ] Deploy your Experiments on remote Data Science Virtual Machines 
- [ ] Deploy your Experiments on remote Data Science VMs with GPU's
- [ ] Deploy your Experiments on HDInsight Clusters running Spark

You may now decommission and delete the following resources if you wish:
  * The Azure Machine Learning Services accounts and workspaces
  * Any Data Science Virtual Machines you have created. NOTE: Even if "Shutdown" in the Operating System, unless these Virtual Machines are "Stopped" using the Azure Portal you are incurring run-time charges. If you Stop them in the Azure Portal, you will be charged for the storage the Virtual Machines are consuimg. 
