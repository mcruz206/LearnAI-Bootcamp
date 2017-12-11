# Lab 4.2: Leverage Batch AI Training for parallel training on GPUs

An AI workflow is very iterative. We go from questions, to data, to building models and operationalizing them and we then iterate on this workflow over and over again. As we do this, we run into many DevOps kind of tasks, such as moving from development to production, provisioning, monitoring, scaling VMs up and down based on demand, or writing code to hadle failures and run at scale. As data scientists, we want these tasks to be handled by others as much as possible so we can focus on what we data scientists are good at.

The purpose of Azure BatchAI is to make it as easy as possible for data scientists to iterate as they build and deploy models. With Azure BatchAI, with a few commands on the command line (or using the Python API, which is slightly more verbose) we can provision a VM and deploy our code on the VM by submitting jobs. We can monitor the status of running jobs both from the command line and from the Azure portal. 

The material for this lab was mostly derived from the [Azure BatchAI documentation page](https://docs.microsoft.com/en-us/azure/batch-ai/quickstart-cli).

## How to access Azure BatchAI from the command line

1. We will use a Data Science Virtual Machine (DSVM) to run this lab. Azure BatchAI is already installed on the DSVM and integrated into the Azure CLI. Log into the DSVM and open up the browser. Download the data used in this lab from [this link](https://batchaisamples.blob.core.windows.net/samples/BatchAIQuickStart.zip?st=2017-09-29T18%3A29%3A00Z&se=2099-12-31T08%3A00%3A00Z&sp=rl&sv=2016-05-31&sr=b&sig=hrAZfbZC%2BQ%2FKccFQZ7OC4b%2FXSzCF5Myi4Cj%2BW3sVZDo%3D), unzip it and go to the unzipped folder. We will see three files: `ConvNet_MNIST.py` which has our script and `Train-28x28_cntk_text.txt` and `Test-28x28_cntk_text.txt` which have our training and test data. The data is a very typical dataset called [MNIST](https://en.wikipedia.org/wiki/MNIST_database), containing images hand-written digits. In the above datasets, the images have already been pre-processed so the data is ready for analysis. The analysis will consist of a Python script which will use the CNTK framework to build a deep learning model for recognizing hand-written digits.
2. From the above folder, launch the Command Prompt by typing `cmd` from the address bar at the top.
![](./images/address-bar-cmd.jpg =500x)
Because Azure BatchAI is still in preview, we need to register to use it. We can do so by the commands `az provider register -n Microsoft.BatchAI` and `az provider register -n Microsoft.Batch`. The registration will take a few minutes. We can check if it's done by typing `az provider show -n Microsoft.BatchAI -o table`.

## Preparing Azure BatchAI to run jobs

1. We will now create a resource group to contain our Azure BatchAI jobs. Azure BatchAI is right now available in a limited number of regions. We will be using "East US". Type `az group create --name azurebootcamplab42 --location eastus` to create the resource group and type `az configure --defaults group=azurebootcamplab42 location=eastus` to make the resource group and location default for the current session.
2. We will also create a storage account for storing data and script. Type `az storage account create --name azbootcampbatchstore --sku Standard_LRS` to do so.
3. Our next task is to create a set of environment variables that will be used to provision our VM later. If we're using the Windows Command Prompt we will run the following commands:
```
set AZURE_STORAGE_ACCOUNT=azbootcampbatchstore
az storage account keys list --account-name azbootcampbatchstore -o tsv --query [0].value > deleteme.txt
set /p AZURE_STORAGE_KEY=< deleteme.txt
set AZURE_BATCHAI_STORAGE_ACCOUNT=azbootcampbatchstore
set /p AZURE_BATCHAI_STORAGE_KEY=< deleteme.txt
del deleteme.txt
```
If we're using the `bash` (Linux) command prompt (assuming the Azure CLI is installed, instructions are [here](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest)), we use the following commands:
```
export AZURE_STORAGE_ACCOUNT=azbootcampbatchstore
export AZURE_STORAGE_KEY=$(az storage account keys list --account-name azbootcampbatchstore -o tsv --query [0].value)
export AZURE_BATCHAI_STORAGE_ACCOUNT=azbootcampbatchstore
export AZURE_BATCHAI_STORAGE_KEY=$(az storage account keys list --account-name azbootcampbatchstore -o tsv --query [0].value)
```
4. Our next task will be to create a shared storage account and a folder called `mnistcntksample` inside it where we will upload the data and the script. Run the following commands to do this:
```
az storage share create --name batchaiquickstart
az storage directory create --share-name batchaiquickstart  --name mnistcntksample

az storage file upload --share-name batchaiquickstart --source Train-28x28_cntk_text.txt --path mnistcntksample
az storage file upload --share-name batchaiquickstart --source Test-28x28_cntk_text.txt --path mnistcntksample
az storage file upload --share-name batchaiquickstart --source ConvNet_MNIST.py --path mnistcntksample
```
5. We can now visit the Azure portal to make sure that the above assets were created and the files uploaded.
![](./images/azbatchai-storage.jpg =600x)
![](./images/azbatchai-files.jpg =600x)

## Managing jobs in Azure BatchAI

The above tasks were needed to get our environment ready for use by Azure BatchAI, but they are not needed every time. We are now ready to put Azure BatchAI to use. BatchAI is an extension of the Azure CLI, the command line utility to spin and manage Azure resources. To access it, we simply type `az batchai`. For example, type `az batchai -h` to get a high-level idea of the things we can do with Azure BatchAI.

1. In this part of the lab, we will be spinning a new NC6 GPU cluster (for deep learning) to submit our CNTK job. Along we the cluster specs, we also provide the shared file and credentials to log into the cluster if we need to. Run the following command to spin up the cluster:
```
az batchai cluster create --name mycluster --vm-size STANDARD_NC6 --image UbuntuLTS --min 1 --max 1 --afs-name batchaiquickstart --afs-mount-path azurefileshare --user-name azureuser --password DataScience2017!
```
Then type `az batchai cluster list -o table` to confirm that the cluster was created. Find the cluster on the Azure portal to make sure we can see it on the portal as well. 
2. We can also store the cluster specs and credentials in an ARM (Azure Resource Manager) template and use Azure BatchAI to spin up the cluster based on the template. Go to the Azure portal and find and start provisioning the above VM, but in the last step before provisioning, instead of clicking on the **create** button to provision the resource, [[Need to finish writing instructions for this]].
3. It is time to submit a job using Azure BatchAI. We will create a json template to encapsulate the execution environment for our job, including the input and output folders, the Docker image from which a container is generated to run our job, and finally the script itself. Copy and paste the below template into a new file called `job.json` and place it in the same directory as the rest of the data.
```
{
 "properties": {
     "stdOutErrPathPrefix": "$AZ_BATCHAI_MOUNT_ROOT/azurefileshare",
    "inputDirectories": [{
         "id": "SAMPLE",
         "path": "$AZ_BATCHAI_MOUNT_ROOT/azurefileshare/mnistcntksample"
     }],
     "outputDirectories": [{
         "id": "MODEL",
         "pathPrefix": "$AZ_BATCHAI_MOUNT_ROOT/azurefileshare",
         "pathSuffix": "model",
         "type": "custom"
     }],
     "containerSettings": {
         "imageSourceRegistry": {
             "image": "microsoft/cntk:2.1-gpu-python3.5-cuda8.0-cudnn6.0"
         }
     },
     "nodeCount": 1,
     "cntkSettings": {
         "pythonScriptFilePath": "$AZ_BATCHAI_INPUT_SAMPLE/ConvNet_MNIST.py",
         "commandLineArgs": "$AZ_BATCHAI_INPUT_SAMPLE $AZ_BATCHAI_OUTPUT_MODEL"
     }
 }
}
```
4. To run the job, simply submit the following command:
```
az batchai job create --name myjob --cluster-name mycluster --config job.json
```
Then use `az batchai job list -o table` to see a listing of all running jobs. While the job is runs, go to the Azure portal and find the shared folder `azurefileshare` and drill down to find the log files that are being generated. We can access the log files directly from the command line. For example, if any jobs fail, we can query `stdouterr` to see what happened by typing `az batchai job list-files --name myjob --output-directory-id stdouterr`. While a job is running, monitor its progress by running the following command:
```
az batchai job stream-file --job-name myjob --output-directory-id stdouterr --name stderr.txt
```
5. To interrupt a job while it's running, simply type `CTRL+C` on the command line and confirm (for this lab, we will say no to avoid killing the job). Wait for the job to finish and run `az batchai job delete --name myjob` to delete it. [[Not exactly sure what it means to delete a job after its finished running.]]
6. Finally, type `az batchai cluster delete --name mycluster` to bring down the cluster after we're down using it.