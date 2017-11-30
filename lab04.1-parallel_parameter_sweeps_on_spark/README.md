# Lab 4.1: Parallel parameter sweeps on Spark

In this lab, we will use the Azure CLI to provision and configure a HDI Spark cluster and use Docker containers on it to run parameter sweep for a given model. Parameter sweep is more commonly known as hyper-parameter tuning. A hyper-parameter is any model input that cannot be directly learned from data and must instead be declared (or guessed) by the data scientist. In order to choose good hyper-parameter values data scientists rely in some cases on no more than some basic rules of thumb and lots of trial and error. Most models have not one but many hyper-parameters and therefore finding a good combination of hyper-parameters (using grid search for example) is a very iterative process. When we "tune" (roughly optimize) our hyper-parameters we train models with different values for hyper-parameters each time and choose the that works best. It is therefore essential to be able to run multiple models efficiently and concurrently. Spark clusters are a good place to perform parameter sweep because we can run many jobs in parallel and let Spark handle the load-balancing and management.

1. To create a Spark cluster we will use the template found [here](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure%2Fazure-quickstart-templates%2Fmaster%2F101-hdinsight-spark-linux%2Fazuredeploy.json) and the Azrue CLI. The credentials to log into the cluster along with some other information is stored in `parameters-hdi.json`. To provision the cluster we simply run the command below (if we do not wish to store passwords in the file, we can remove them and instead we will be prompted to enter a password when we run the command).

```
az group deployment create -g azurebootcamplab41 --template-file template-hdi.json --parameters @parameters-hdi.json
```

2. Open the Azure ML Workbench and start a new project called `parameter_sweep` that uses the **Parameter Sweep on Spark** as template. Place the new project in **Documents**. Open the project and go to **File > Open Command Prompt** and type the following commands to prepare the docker environment and the Spark cluster to run our jobs.
```
az ml experiment prepare -c docker
az ml experiment prepare -c azbootcamphdispark -g azurebootcamplab41
```
3. Return to the Workbench and go to **File > Open Project (Code)** to open it using Code. Examine the script `sweep_spark.py`. Find the learning algorithm we're using and the set of hyper-parameters that we want to optimize over and their ranges.

[[The following command doesn't run. Says I need to prepare the environment first, but I successfully ran the previous command so the environment should have been prepared already. However, running the rest of the code still works, so it's nothing to worry about.]]

4. Now we specify the Spark cluster as the compute target by running the following command:
```
az ml computetarget attach --name azbootcamphdispark --address azbootcamphdispark-ssh.azurehdinsight.net --username sshuser --password DataScience2017! --type cluster
```
This will create the files `aml_config/azbootcamphdispark.compute` and `aml_config/azbootcamphdispark.runconfig`. We now have a pair of such files for local, Docker and one for Spark.
5. Finally, we can now run the job. We will first run it in Docker, which is great for debugging purposes and working locally while we're still in development mode. To do so just run `az ml experiment submit -c docker .\sweep_spark.py` on the Command Line. If we were able to run the Docker example successfully, we can now see if we can replicate it in Spark. To do so simply replace `docker` with `azbootcamphdispark` in the above command and rerun it. If the Docker run failed, we may need to swich Docker to a Windows context (by right-clicking on the Docker icon in the taskbar) and upon getting an error message, switch it back to a Linux context and then try again.
6. Report the accuracy of the best model that came out of the parameter sweep.
