# Consuming a scoring service

This hands-on lab guides you through consuming a Machine Learning scoring service using [Azure Machine Learning Services](https://docs.microsoft.com/en-us/azure/machine-learning/preview/overview-what-is-azure-ml) with the Azure Machine Learning Workbench. 

In this workshop, you will:
- [ ] Understand how to consume a deployed model from a Web API

You'll focus on the objectives above, not Data Science, Machine Learning or a difficult scenario.  

***NOTE:*** There are several pre-requisites for this course, including an understanding and implementation of: 
  *  Programming using an Agile methodology
  *  Machine Learning and Data Science
  *  Intermediate to Advancced Python programming
  *  Familiarity with Web Services and API Programming
  *  Familiarity with [Swagger codegen](https://github.com/swagger-api/swagger-codegen)

There is a comprehensive Learning Path you can use to prepare for this course [located here](https://github.com/Azure/learnAnalytics-CreatingSolutionswiththeTeamDataScienceProcess-/blob/master/Instructions/Learning%20Path%20-%20Creating%20Solutions%20with%20the%20Team%20Data%20Science%20Process.md).

## Introduction and setup 

***NOTE***: These steps must be completed ***prior*** to attempting this workshop.
  *  You will need a Microsoft Azure account. You can use a production Azure account if you are able to create objects. You can also use your Microsoft Developer Network (MSDN) account (if you have one) to complete this workshop. If you don't have access to a corporate or MSDN account you can create a free account [using this process](https://azure.microsoft.com/free/).
  *  You will need an Azure Machine Learning Services account. [Open this reference](https://docs.microsoft.com/en-us/azure/machine-learning/preview/quickstart-installation), and complete only the sections marked **"Sign in to the Azure portal"** and **"Create Azure Machine Learning accounts"**. Write down the *Experimentation account name* and bring it to class.
  *  You can install the Azure Machine Learning Workbench locally:
        *  [Open this reference](https://docs.microsoft.com/en-us/azure/machine-learning/preview/quickstart-installation) and follow the sections marked **Install Azure Machine Learning Workbench on Windows** or choose your OS type from the instructions there.

  *  Or you can use a Windows Data Science Virtual Machine (DSVM) to run this lab: 
        *  [Navigate to this path](https://azuremarketplace.microsoft.com/en-us/marketplace/apps/microsoft-ads.windows-data-science-vm), and create a Windows Azure Data Science Virtual Machine (DSVM). Choose a VM size of: DS3_V2, with 4 virtual CPUs and 14-Gb RAM. When the DSVM is deployed, start it using the [Azure portal.](https://portal.azure.com)
        *  After you create and Start the DSVM, log in to it and double-click the "Install Azure Machine Learning Workbench" icon. Finish the installation by following the on-screen instructions. The installer downloads all the necessary dependent components, such as Python, Miniconda, and other related libraries. The installation might take around half an hour to finish all the components. When complete, the Azure Machine Learning Workbench is installed in the following directory: C:\Users\<user>\AppData\Local\AmlWorkbench

## Consuming the Model

The general configuration for working with the API settings in Docker has this view:

![DockerAPI](https://docs.microsoft.com/en-us/azure/container-service/media/container-service-deploy-spring-boot-app-on-linux/lx03.png)

We will review these articles in class: 
  1.  [Primary Concepts](https://docs.microsoft.com/en-us/azure/machine-learning/preview/model-management-consumption)

### Lab: Creating a model and consuming it

In this lab you'll create an experiment, examine its configuration, and run the experiment locally, using both a local and a local Docker container. You'll set up the experiment in the AMLS Workbench tool, and then run all experiments from the command line interface (CLI)
- [ ] Open the Azure Machine Learning Services Workbench tool locally or on your Data Science Virtual Machine. 
- [ ] [Navigate to this Resource](https://docs.microsoft.com/en-us/azure/machine-learning/preview/model-management-consumption) and complete the steps you see there.

## Workshop Completion

In this workshop you learned how to:
- [ ] Understand how to consume a deployed model from a Web API

You may now decommission and delete the following resources if you wish:
  * The Azure Machine Learning Services accounts and workspaces, and any Web API's
  * Any Data Science Virtual Machines you have created. NOTE: Even if "Shutdown" in the Operating System, unless these Virtual Machines are "Stopped" using the Azure Portal you are incurring run-time charges. If you Stop them in the Azure Portal, you will be charged for the storage the Virtual Machines are consuming. 
