# lab04.6-manage_models_and_automate_model_retraining - Manage Models and Automate Model Retraining with Azure Machine Learning Services
This hands-on lab guides you through managing and regtraining models using [Azure Machine Learning Services](https://docs.microsoft.com/en-us/azure/machine-learning/preview/overview-what-is-azure-ml) with the Azure Machine Learning Workbench. 

In this workshop, you will:
- [ ] Understand ***TODO***

You'll focus on the objectives above, not Data Science, Machine Learning or a difficult scenario.  

***NOTE:*** There are several pre-requisites for this course, including an understanding and implementation of: 
  *  Programming using an Agile methodology
  *  Machine Learning and Data Science
  *  Working with the Microsoft Azure Portal

There is a comprehensive Learning Path you can use to prepare for this course [located here](https://github.com/Azure/learnAnalytics-CreatingSolutionswiththeTeamDataScienceProcess-/blob/master/Instructions/Learning%20Path%20-%20Creating%20Solutions%20with%20the%20Team%20Data%20Science%20Process.md).

## Introduction and setup 
The [Primary Concepts for this Workshop are here](https://docs.microsoft.com/en-us/azure/machine-learning/preview/model-management-overview) and [another here](https://docs.microsoft.com/en-us/azure/machine-learning/preview/model-management-configuration). We'll refer to these throughout the labs.

***NOTE*** The following steps must be completed ***prior*** to attempting this workshop:

  *  You will use Visual Studio Team Services (VSTS) account as your code-control system. If you do not have a VSTS where you can create new projects:  
       *  First, [read all information in this link](https://docs.microsoft.com/en-us/azure/machine-learning/preview/using-git-ml-project)
       *  Next, perform steps 1-2 **only**. Record the git repo location from the VSTS location you created and bring it to class with you
  *  You will need a Microsoft Azure account. You can use a production Azure account if you are able to create objects. You can also use your Microsoft Developer Network (MSDN) account (if you have one) to complete this workshop. If you don't have access to a corporate or MSDN account: 
       *  Create a free account [using this process](https://azure.microsoft.com/free/)
  *  You will need an Azure Machine Learning Services account. 
       *  [Open this reference](https://docs.microsoft.com/en-us/azure/machine-learning/preview/quickstart-installation), and complete only the sections marked **"Sign in to the Azure portal"** and **"Create Azure Machine Learning accounts"**. 
       *  Write down the *Experimentation account name* and bring it to class

  *  You can install the Azure Machine Learning Workbench and Docker locally:
        *  [Open this reference](https://docs.microsoft.com/en-us/azure/machine-learning/preview/quickstart-installation) and follow the sections marked **Install Azure Machine Learning Workbench on Windows**
        *  You'll also need Docker for certain parts of the lab. To install it, [open this reference](https://www.docker.com/docker-windows) and follow the instructions for installing Docker locally.

  *  Or you can use a Windows Data Science Virtual Machine (DSVM) to run this lab: 
        *  [Navigate to this path](https://azuremarketplace.microsoft.com/en-us/marketplace/apps/microsoft-ads.windows-data-science-vm), and create a Windows Azure Data Science Virtual Machine (DSVM). 
           *  Choose a VM size of: D4S_V3, with 4 virtual CPUs and 14-Gb RAM. This VM size is not available in all regions, so choose an appropriate region (`East US 2` is a good choice).
           *  When the DSVM is deployed, start it using the [Azure portal.](https://portal.azure.com)
        *  After you create and Start the DSVM, log in to it and double-click the "Install Azure Machine Learning Workbench" icon. Finish the installation by following the on-screen instructions. The installer downloads all the necessary dependent components, such as Python, Miniconda, and other related libraries. The installation might take around half an hour to finish all the components. When complete, the Azure Machine Learning Workbench is installed in the following directory: C:\\Users\\%USERNAME%\\AppData\\Local\\AmlWorkbench

## Azure Machine Learning Model Management

AMLS Model Management has the following structure: 

![Image](https://docs.microsoft.com/en-us/azure/machine-learning/preview/media/model-management-overview/modelmanagement.png)

### Lab: Working with Models in AMLS
In this lab you'll ***TODO*** using the Azure Machine Learning Workbench.
- [ ] [Open this link](https://docs.microsoft.com/en-us/azure/machine-learning/preview/how-to-use-tdsp-in-azure-ml), ***TODO***

## Retraining 
Once you have Operationalized your model, you will want to improve its performance. To retrain your model, the following workflow is used: 

![Image](https://docs.microsoft.com/en-us/azure/machine-learning/preview/media/model-management-overview/modelmanagementworkflow.png)

## Workshop Completion

In this workshop you learned how to:
- [ ] ***TODO***

You may now delete and decommission the following resources if you wish:
  * The Azure Machine Learning Services accounts and workspaces
  * Any deployments, executions, and other associated Machine Learning Services elements
  * The Visual Studio Team Services Repository
  * Any Data Science Virtual Machines you have created. NOTE: Even if "Shutdown" in the Operating System, unless these Virtual Machines are "Stopped" using the Azure Portal you are incurring run-time charges. If you Stop them in the Azure Portal, you will be charged for the storage the Virtual Machines are consuimg. 

