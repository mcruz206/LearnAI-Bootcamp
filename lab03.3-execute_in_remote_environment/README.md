# Executing Your Machine Learning Model In A Remote Environment

This hands-on lab guides you through executing a machine learning data preparation or model training work load in a remote environment using [Azure Machine Learning Services](https://docs.microsoft.com/en-us/azure/machine-learning/preview/overview-what-is-azure-ml) with the Azure Machine Learning Workbench. 

In this workshop, you will:
- [ ] Understand how to execute your workloads on remote Data Science Virtual Machines 
- [ ] Understand how to execute your workloads on remote Data Science VMs with GPU's
- [ ] Understand how to execute your workloads on HDInsight Clusters running Spark

You'll focus on the objectives above, not Data Science, Machine Learning or a difficult scenario.  

***NOTE:*** There are several pre-requisites for this course, including an understanding and implementation of: 
  *  Programming using an Agile methodology
  *  Machine Learning and Data Science
  *  Intermediate to Advanced Python programming
  *  Familiarity with Docker containers 
  *  Familiarity with GPU Technology
  *  Familiarity with Spark programming

There is a comprehensive Learning Path you can use to prepare for this course [located here](https://github.com/Azure/learnAnalytics-CreatingSolutionswiththeTeamDataScienceProcess-/blob/master/Instructions/Learning%20Path%20-%20Creating%20Solutions%20with%20the%20Team%20Data%20Science%20Process.md).

## Executing an Experiment Locally

The general configuration for working with Azure Machine Learning has these components:
![Azure Machine Learning Components](https://docs.microsoft.com/en-us/azure/machine-learning/preview/media/overview-general-concepts/hierarchy.png)

### Configuration Files

When you run a script in Azure Machine Learning (Azure ML) Workbench, the behavior of the execution is controlled (usually) by files in the **aml_config** folder in your experimentation directory. 
The files are: 
  * ***conda_dependencies.yml*** - A conda environment file that specifies the Python runtime version and packages that your code depends on. When Azure ML Workbench executes a script in a Docker container or HDInsight cluster, it uses this file to create a conda environment for your script to run. 
  * ***spark_dependencies.yml*** - Specifies the Spark application name when you submit a PySpark script and Spark packages that needs to be installed. You can also specify any public Maven repository or Spark package that can be found in those Maven repositories.
  * ***compute target*** files - Specifies connection and configuration information for the compute target. It is a list of name-value pairs. ***[compute target name].compute*** contains configuration information for the following environments:
    *  local
    *  docker
    *  remotedocker
    *  cluster
  * ***run configuration*** files - Specifies the Azure ML experiment execution behavior such as tracking run history, or what compute target to use
    * [run configuration name].runconfig

The Azure Machine Learning Services Workbench tool combines all of these components into one location. You can use a graphical or command-line approach to managing these components.  

![Local AMLS Experiment run](https://docs.microsoft.com/en-us/azure/machine-learning/preview/media/experimentation-service-configuration/local-native-run.png)

### Lab: Execute an Experiment Locally

In this lab you'll create an experiment, examine its configuration, and run the experiment locally, using both a `local` compute and a `localdocker` compute. You'll set up the experiment in the AML Workbench tool, and then run all experiments from the command line interface (CLI)
- [ ] Open the Azure Machine Learning Workbench tool locally or on your Data Science Virtual Machine. 
- [ ] Create a new experiment using the Iris example.
- [ ] [Navigate to this resource](https://docs.microsoft.com/en-us/azure/machine-learning/preview/experimentation-service-configuration), and complete the sections marked **"Launching the CLI"** through **"Running a script on local Docker"**
- [ ] Replace the line marked: `az ml experiment submit -c docker myscript.py` use `az ml experiment submit -c docker-python iris_sklearn.py`.

## Executing an Experiment to a remote Data Science Virtual Machine in Azure

You configure the Azure ML experiment flow for a remote run using this process: 

![AMLS Remote Experiment Flow](https://docs.microsoft.com/en-us/azure/machine-learning/preview/media/experimentation-service-configuration/remote-vm-run.png)

### Lab: Execute an Experiment to a remote Data Science Virtual Machine

In this lab you will create an experiment, examine its configuration, and run the experiment on a remote Docker container. You'll set up the experiment in the AMLS Workbench tool, and then run all experiments from the command line interface (CLI)

- [ ] [Open this Reference and create an Ubuntu Data Science Virtual Machine](https://docs.microsoft.com/en-us/azure/machine-learning/data-science-virtual-machine/dsvm-ubuntu-intro)

    - [ ] Choose a size of *Standard D4s v3 (4 vcpus, 16 GB memory)*
    - [ ] Use a password, not a SSH Key
    - [ ] Start the VM and connect to it using ssh. If you use some version of bash, the command is: `ssh *nameyoupicked*@*ipaddressofvm*`
    - [ ] Check to ensure Docker is functional on your Linux DSVM with the following command:

```
sudo docker run docker/whalesay cowsay "The best debugging is done with CTRL-X. - Buck Woody"
```

- [ ] Open the Azure Machine Learning Services Workbench tool locally or on your Windows Data Science Virtual Machine. 
- [ ] [Navigate to this resource](https://docs.microsoft.com/en-us/azure/machine-learning/preview/experimentation-service-configuration), and complete the section marked **"Running a script on a remote Docker"**

### Running on a remove Spark or GPU cluster

To run your scripts on Spark, [review this link](https://docs.microsoft.com/en-us/azure/machine-learning/preview/experimentation-service-configuration), referencing the section marked **"Running a script on an HDInsight cluster"**

To run your scripts on GPU in a remote machine, you can follow the guidance in this article: [How to use GPU in Azure Machine Learning](https://docs.microsoft.com/en-us/azure/machine-learning/preview/how-to-use-gpu). Focus on the section **Configure Azure ML Workbench to Access GPU**

## Workshop Completion

In this workshop you learned how to:
- [ ] Execute your workloads on remote Data Science Virtual Machines 
- [ ] Execute your workloads on remote Data Science VMs with GPU's
- [ ] Execute your workloads on HDInsight Clusters running Spark

You may now decommission and delete the following resources if you wish:
  * The Azure Machine Learning Services accounts and workspaces
  * Any Data Science Virtual Machines you have created. NOTE: Even if "Shutdown" in the Operating System, unless these Virtual Machines are "Stopped" using the Azure Portal you are incurring run-time charges. If you Stop them in the Azure Portal, you will be charged for the storage the Virtual Machines are consuming. 
