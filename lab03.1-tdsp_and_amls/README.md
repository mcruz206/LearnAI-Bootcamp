# lab03.1-tdsp_and_amls - The Team Data Science Process using Azure Machine Learning
This hands-on lab guides you through using the [Team Data Science Process](https://docs.microsoft.com/en-us/azure/machine-learning/team-data-science-process/overview) using [Azure Machine Learning Services](https://docs.microsoft.com/en-us/azure/machine-learning/preview/overview-what-is-azure-ml) with the Azure Machine Learning Workbench. 

In this workshop, you will:
- [ ] Understand and use the TDSP to clearly define business goals and success criteria
- [ ] Use a code-repository system with the Azure Machine Learning Workbench using the TDSP structure
- [ ] Create an example environment
- [ ] Use the TDSP and AMLS for data acquisition and understanding
- [ ] Use the TDSP and AMLS for creating an experiment with a model and evaluation of models
- [ ] Use the TDSP and AMLS for deployment
- [ ] Use the TDSP and AMLS for project close-out and customer acceptance

You'll focus on the objectives above, not Data Science, Machine Learning or a difficult scenario.  

***NOTE:*** There are several pre-requisites for this course, including an understanding and implementation of: 
- [ ] Programming using an Agile methodology
- [ ] Maching Learning and Data Science

There is a comprehensive Learning Path you can use to prepare for this course [located here](https://github.com/Azure/learnAnalytics-CreatingSolutionswiththeTeamDataScienceProcess-/blob/master/Instructions/Learning%20Path%20-%20Creating%20Solutions%20with%20the%20Team%20Data%20Science%20Process.md).

## Introduction and setup 
[Placeholder](https://docs.microsoft.com/en-us/azure/machine-learning/preview/quickstart-installation)

***NOTE***: These steps must be completed ***prior*** to attempting this workshop.
- [ ] You will need a Microsoft Azure account. You can use a production Azure account if you are able to create objects. You can also use your Microsoft Developer Network (MSDN) account (if you have one) to complete this workshop. If you don't have access to a corporate or MSDN account you can create a free account [using this process](https://azure.microsoft.com/free/).
- [ ] You will need an Azure Machine Learning Services account. [Open this reference](https://docs.microsoft.com/en-us/azure/machine-learning/preview/quickstart-installation), and complete only the sections marked **"Sign in to the Azure portal"** and **"Create Azure Machine Learning accounts"**. Write down the *Experimentation account name* and bring it to class.
- [ ] You'll use a Windows Data Science Vitual Machine (DSVM) to run this lab. You can use this Virtual Machine in all production projects. [Navigate to this path](https://azuremarketplace.microsoft.com/en-us/marketplace/apps/microsoft-ads.windows-data-science-vm), and create a Windows Azure Data Science Virtual Machine (DSVM). Choose a VM size of: DS3_V2, with 4 virtual CPUs and 14-Gb RAM. When the DSVM is deployed, start it using the [Azure portal.](https://portal.azure.com)
- [ ] After you create and Start the DSVM, log in to it and double-click the "Install Azure Machine Learning Workbench" icon. Finish the installation by following the on-screen instructions. The installer downloads all the necessary dependent components, such as Python, Miniconda, and other related libraries. The installation might take around half an hour to finish all the components. When complete, the Azure Machine Learning Workbench is installed in the following directory: C:\Users\<user>\AppData\Local\AmlWorkbench
- [ ] You'll also use Visual Studio Team Services. And we'll also mention a few [Developer Operations (DevOps)](https://docsmsftpdfs.blob.core.windows.net/guides/azure/azure-ops-guide.pdf) integration steps as we go. First, [read all information in this link](https://docs.microsoft.com/en-us/azure/machine-learning/preview/using-git-ml-project), and then perform steps 1-2 only.

## 1. Business Understanding
In the [Business Understanding](https://docs.microsoft.com/en-us/azure/machine-learning/team-data-science-process/lifecycle-business-understanding) phase of the TDSP, you discover the questions that the organization would like answered from data. This is a group effort, involving the organization, the Data Science team, and the DevOps team along with other stakeholders. 
![Image](resources/docs/images/contosologo.gif?raw=true)
In this workshop, your organization has requested a model that will classify measurement data they have collected from local garden shops into one of three iris types. You'll begin the project using the TDSP by copying the github information from your source-control system, and then setting up the project structure using the Data Science Virtual Machine with the Azure Machine Learning Workbench.

### Lab: Set Up TDSP Structure using the Azure Machine Learning Workbench
In this lab you'll set up your project's structure, conforming to the Team Data Science Process, using the Azure Machine Learning Workbench.
- [ ] [Open this link](https://docs.microsoft.com/en-us/azure/machine-learning/preview/using-git-ml-project) and complete step 3
- [ ] [Review this link](https://github.com/Azure/Azure-TDSP-ProjectTemplate) and verify that you have the structure shown. You will use this structure throughout this workshop.

### Lab: Use-case evaluation for Data Science questions
In this lab you'll evaluate a business scenario, and detail possible predictions, classifications, or other data science questions that you can begin to explore.
- [ ] Read the scenario below carefully. Copy and paste the Scenario text below into a text file called *Business Understanding.md* in the /docs directory set up in the previous lab.
- [ ] Detail a question you could begin to explore answering with a prediction or classification algorithm. Enter that question in your *Business Understanding.md* text file. 
- [ ] Which algorithm or family of algorithms could you use to answer your question? Enter that answer in your *Business Understanding.md* text file.
- [ ] What data sources will you need to complete your prediction? Enter possible sources in your *Business Understanding.md* text file.

#### Example Scenario: 
The Orange Telecom company in France is one of the largest operators of mobile and internet services in Europe and Africa and a global leader in corporate telecommunication services. They have 256 million customers worldwide. They have significant coverage in France, Spain, Belgium, Poland, Romania, Slovakia Moldova, and a large presence Africa and the Middle East.
Customer Churn is always an issue in any company. Orange would like to predict the propensity of customers to switch provider (churn), buy new products or services (appetency), or buy upgrades or add-ons proposed to them to make the sale more profitable (up-selling). For this effort, they think churn is the first thing they would like to focus on. 

## 2. Data Acquisition and Understanding
The [Data Aquisition and Understanding](https://docs.microsoft.com/en-us/azure/machine-learning/team-data-science-process/lifecycle-data) phase of the TDSP you ingest or access data from various locations to answer the questions the organization has asked. In most cases, this data will be in multiple locations. 
Once the data is ingested into the system, you'll need to examine it to see what it holds. All data needs cleaning, so after the inspection phase, you'll replace missing values, add and change columns. You'll cover more extensive Data Wrangling tasks in other labs. 
In this workshop, we'll use a single file-based dataset. The Iris data set contains 150 records of 3 classes of iris flowers with numeric values for petal and sepal length and width.  This data set is traditionally used for classification and prediction – to see which features of an iris can identify the flower as a certain type of iris. The values for length and width can be used to classify an iris into one of three iris types: Iris setosa, Iris versicolor, or Iris virginica. Visually exploring this data also lets you see the grouping (clustering) of the records into these three different types of irises. You'll copy this data from the web into your sample data prject folder first, and then in subsequent labs you'll explore how to add more data sets in other locations such as databases or [large-scale storage[(https://docs.microsoft.com/en-us/azure/machine-learning/preview/how-to-read-write-files). 

### Lab: Ingest data from a local source
In this lab you will download the iris data set, inspect it, make a few changes, and then save the Data Wrangling steps as a Python package. 
- [ ] Download the [Iris data set here](https://archive.ics.uci.edu/ml/datasets/Iris) and store it  in the *Sample_Data* directory on your Data Science Virtual Machine's project location. 
- [ ] Open [this reference](https://docs.microsoft.com/en-us/azure/machine-learning/preview/tutorial-classifying-iris-part-1), skip the first section, and complete the steps starting with the section marked **Create a Data Preparation Task**. (Note: **DO NOT** create another project - stay in this one.) 

## 3. Modeling
The [Modeling](https://docs.microsoft.com/en-us/azure/machine-learning/team-data-science-process/lifecycle-modeling) phase of the Team Data Science Process involves (TODO)
### Lab: (TODO)
In this lab (TODO)
- [ ] Lab Step 
- [ ] Lab Step 

## 4. Deployment
The [Deployment](https://docs.microsoft.com/en-us/azure/machine-learning/team-data-science-process/lifecycle-deployment) phase of the TDSP entails (TODO) 
### Lab: (TODO)
In this lab (TODO)
- [ ] Lab Step 
- [ ] Lab Step 

## 5. Customer Acceptance
The final step in the Team Data Science Process is [Customer Acceptance](https://docs.microsoft.com/en-us/azure/machine-learning/team-data-science-process/lifecycle-acceptance). Here you focus on (TODO)
### Lab: (TODO)
In this lab (TODO)
- [ ] Lab Step 
- [ ] Lab Step 

## Workshop Completion
In this workshop we covered (TODO)
- [ ] Objective (TODO)
- [ ] Objective (TODO)
- [ ] Objective (TODO)
