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
