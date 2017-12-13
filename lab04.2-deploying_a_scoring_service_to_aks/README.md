# Deploying a scoring service to the Azure Container (AKS)

This hands-on lab guides you through deploying a Machine Learning scoring file to a remote environment using [Azure Machine Learning Services](https://docs.microsoft.com/en-us/azure/machine-learning/preview/overview-what-is-azure-ml) with the Azure Machine Learning Workbench. 

In this workshop, you will:
- [ ] Understand how to create a model file
- [ ] Generate a scoring script and schema file
- [ ] Prepare your scoring environment
- [ ] Create a real-time web service
- [ ] Run the real-time web service
- [ ] Examine the output blob data

You'll focus on the objectives above, not Data Science, Machine Learning or a difficult scenario.  

***NOTE:*** There are several pre-requisites for this course, including an understanding and implementation of: 
  *  Programming using an Agile methodology
  *  Machine Learning and Data Science
  *  Intermediate to Advancced Python programming
  *  Familiarity with Docker containers and Kubernetes

There is a comprehensive Learning Path you can use to prepare for this course [located here](https://github.com/Azure/learnAnalytics-CreatingSolutionswiththeTeamDataScienceProcess-/blob/master/Instructions/Learning%20Path%20-%20Creating%20Solutions%20with%20the%20Team%20Data%20Science%20Process.md).

## Building the Scoring for remote Deployment

(Note - [Our primary example is here](https://docs.microsoft.com/en-us/azure/machine-learning/preview/tutorial-classifying-iris-part-3) and [Another example is here](https://blogs.technet.microsoft.com/machinelearning/2017/09/25/deploying-machine-learning-models-using-azure-machine-learning/) )

The general configuration for working with the  Azure Container Service has this architecture:

![AKS](https://azurecomcdn.azureedge.net/mediahandler/acomblog/media/Default/blog/15159959-b5cd-4fe9-aeba-441139943ecd.png)

We will review these articles in class: 
  1.  [A quick overview of the Azure Container Service (AKS)](https://docs.microsoft.com/en-us/azure/aks/kubernetes-walkthrough)
  2.  [Understanding Service Principals](https://docs.microsoft.com/en-us/azure/aks/kubernetes-service-principal)
  3.  [Scoring Setup and Configuration](https://docs.microsoft.com/en-us/azure/machine-learning/preview/deployment-setup-configuration)
  4.  [Scaling Clusters](https://docs.microsoft.com/en-us/azure/machine-learning/preview/how-to-scale-clusters)


### Lab: Check for model build and pkl file creation

In this lab you'll create an experiment, examine its configuration, and run the experiment locally, using both a local and a local Docker container. You'll set up the experiment in the AMLS Workbench tool, and then run all experiments from the command line interface (CLI)
- [ ] Open the Azure Machine Learning Services Workbench tool locally or on your Data Science Virtual Machine. 
- [ ] [Navigate to this resource](https://docs.microsoft.com/en-us/azure/machine-learning/preview/tutorial-classifying-iris-part-2), and ensure you have completed all sections there.
- [ ] [Next, navigate to this resource](https://docs.microsoft.com/en-us/azure/machine-learning/preview/tutorial-classifying-iris-part-3), and complete all sections there.

## Workshop Completion

In this workshop you learned how to:
- [ ] Understand how to create a model file
- [ ] Generate a scoring script and schema file
- [ ] Prepare your scoring environment
- [ ] Create a real-time web service
- [ ] Run the real-time web service
- [ ] Examine the output blob data

You may now decommission and delete the following resources if you wish:
  * The Azure Machine Learning Services accounts and workspaces
  * Any Data Science Virtual Machines you have created. NOTE: Even if "Shutdown" in the Operating System, unless these Virtual Machines are "Stopped" using the Azure Portal you are incurring run-time charges. If you Stop them in the Azure Portal, you will be charged for the storage the Virtual Machines are consuming. 
