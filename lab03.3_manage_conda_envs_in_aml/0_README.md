# Updating conda to include additional external dependencies for deep learning or MML

## What is conda?

Conda is an multi-platform, open-source package management system. Fundamentally it tries to solve replicability by creating a clearn environment in which our code and all its dependencies can be placed and run. It is primarily used for python, but can be used for multiple languages (including R). It is generally packaged and distributed through the Anaconda python distribution supported by Anaconda, Inc (a Microsoft partner).

In the context of Python, Conda is similar to `pip` and can be used to install and update Python packages, but Conda goes beyond that and also installs library dependencies outside of Python, such as C++ dependencies.

With Azure ML Workbench projects, we use Docker for managing system requirements and dependencies between compute environments (local, remote, Spark). Within Docker itself, we use Conda to manage Python dependencies for a given project. We use the command line `conda` command to interact with `conda` (in the case of Python, we also have some control in Jupyter notebooks)

CRITICAL NOTE: conda is not used (by default) for local script runs, which use the root environment instead. This can cause dependencies to break between compute environments. As a general rule, it is better to use Docker even when working locally.

1. Open the Workbench and create a new project called `hello_bootcamp`, make it a blank project and place it in the `Documents` folder. Open the project and go to **File > Open Command Prompt** to access the command line from within the project parent folder. Type `python`, then in the python console paste this in:

  ```
  import sys
  print(sys.executable)

  import matplotlib
  print("matplotlib version:", matplotlib.__version__)
  ```
  Note the path to the Python executable and the `matplotlib` version. Now type `exit()` to leave the Python console and return to the command prompt.  
  If working locally without the use of Conda environments, we would be using our local root Python installation, which means that all projects would rely on the same environment and where conflicting dependencies can cause collusion, and where missing system dependencies can cause headaches when going from development to staging or production. Therefore, if working locally it is still better to leverage Docker (and Conda within Docker) instead of relying on the root Python executable as we did above. Let's now see how we can do this.
2. To see if Conda is installed, type `conda --version`. Now type `conda list` to see a listing of installed python packages. Some of these packages are installed using `pip` or `easy_install`, some are installed using `conda install`.
3. Open the project with Code by going to **File > Open Project (Code)**. Open the `conda_dependencies.yml` file and examine the content. What package dependencies are specified here? Add `matplotlib=2.0.2` to the list of dependencies for this project then save the file.
4. To create a Conda environment, return to the command prompt and type

```
conda env create -f aml_config/conda_dependencies.yml
```

which creates an environment called `project_environment` by default (we can rename it using the `-n` switch). Once an environment is created, it needs to be activated by running `activate project_environment`. Activate the environment and run `python`, then in the python console type 

```
import sys
print(sys.executable)

import matplotlib
print("matplotlib version:", matplotlib.__version__)
```

Compare the path to the Python executable and the `matplotlib` version to the one we obtained earlier. Type `exit()` to return from the Python console to the command line.  
5. To deactivate an environment simply type `deactivate`. Note that an environment is only active for the open Command Prompt session and will be deactivated if we close the session. We can also permanently remove this environment using `conda env remove -n project_environment`. Deactivate and remove the Conda environment.
So far we manually created and activated Conda environments. This is useful in order to see some of what happens behind the scene when we run an experiment in Workbench. However, in practice the above steps are built-in and automatically handled by Workbench itself.
6. Return to Code and create a new script and call it `my_script.py` then paste in the following into it:

```
import sys
print(sys.executable)

import matplotlib
print("matplotlib version:", matplotlib.__version__)
```

Now save changes by going to **File > Save All**.
7. Return to Workbench and verify that the changes are visible. Now run `my_script.py` using Docker as the compute environment. To do so, we must first prepare the Docker environment. This will take a few minutes, after which, we can sumbit the experiment. Return to the command prompt run the following command: 

```
az ml experiment prepare -c docker
```

The above command prepares a new Docker image that we can run our experiment in. We can directly access this image by typing `docker image list` and copying the name of the image (the one prefixed with `azureml_`). Then we type `docker run -it <image_name>` which puts us directly inside the command line of the Docker image. In here we can type `python` to launch a Python session and paste in the same Python code from above to check the Python executable path and the `matplotlib` version. We can then type `exit()` to leave the Python console and type `exit` to leave the Docker shell and return to the main command prompt. Manually logging into the Docker environment is tedious and not necessary in most cases. Instead we can run the following command to directly run the script in the Conda environment inside the Docker image.

```
az ml experiment submit -c docker my_script.py
```

8. In the Workbench, go to the **Jobs** panel on the right and click on the green **Completed** button to see any results printed by the script. Does the `matplotlib` version number match the version number in `conda_dependencies.yml`?
Conda creates an execution environment for our project and binds our Python scripts and their dependencies so that its execution environment can be isolated from that of other projects. My submitting the above command, we created a Docker image with our project and used Conda to handle the script and its dependencies, which Conda does by creating a new "environment" for the project. We created a Conda environment automatically in the last step is because in the `aml_config/docker.runconfig` file we point to `conda_dependencies.yml` and we set `PrepareEnvironment: true`.
9. Most of us do not develop or test our Python scripts from the Python console. Instead we prefer to use an IDE like Code or Jupyter Notebooks. To launch a Jupyter notebook session for a given project, return to the command prompt and run `az ml notebook start`. This will open up a browser session (`localhost:8888`) and present our project parent directory. On the right side, click on **New** and examine the content of the dropdown. It should include `hello_bootcamp local` and `hello_bootcamp docker` with are the Conda environments associated with our project. Click on `hello_bootcamp docker` and paste in the following code in the cell of the new Jupyter notebook that opens.

```
import sys
print(sys.executable)

import matplotlib
print("matplotlib version:", matplotlib.__version__)
```

Check the `matplotlib` version and the Python executable path and confirm that it matches what we earlier.  
As we saw above, leveraging Docker and Conda gives us the ideal environment to run our projects, and we can leverage them both from the command line and Jupyter. However, Docker is not always available on Windows machines. On the DSVM for example, we need to use `D_v3` instances (and enable Hyper-V) in order be able to install and use Docker. When docker is not an option and we can only work locally, then we can (and definitely should) still use Conda. Let's see how this can be done.
9. From the command prompt, create a new Conda environment by pointing to `conda_dependencies.yml` and use `-n myenv` to rename it from `project_environment` to `myenv` (renaming the Conda environment is an optional step). Go to the `aml_config/local.compute` file and point the Python location from the root Python directory to the Python directory specific to this environment. You can do so by replacing `pythonLocation: "python"` with `pythonLocation: "%USERPROFILE%/AppData/Local/amlworkbench/Python/envs/myenv/python.exe"`. To make sure that this works, run `my_script.py` locally and inspect the results generated by the script (just as we did in step 3). 
10. It is likely that the run in the last step fails because of Python dependencies that are not properly set up in the local environment. Create a new file called `localenv_conda_dependencies.yml` and place it in `aml_config`. Place the following content inside of it and save it:

```
# Conda environment specification. The dependencies defined in this file will be
# automatically provisioned for runs against docker, VM, and HDI cluster targets.

# Details about the Conda environment file format:
# https://conda.io/docs/using/envs.html#create-environment-file-by-hand

# For Spark packages and configuration, see spark_dependencies.yml.

name: myenv
dependencies:
  - python=3.5.2
  - scikit-learn=0.18.1
  - ipykernel=4.6.1
  ## aml specific
  - psutil=5.2.2
  - scipy=0.18.1
  - numpy=1.11.3 # needed for appropriate version of pandas
  - pandas=0.19.2 # needs this version.
  - matplotlib==2.0.2

  - pip:
    # The API for Azure Machine Learning Model Management Service.
    - azure-ml-api-sdk==0.1.0a11

    # Library for collecting data from operationalized models
    - azureml.datacollector==0.1.0a13
    # aml specific:
    ## these are tagged to the version of AML
    - https://azuremldownloads.blob.core.windows.net/wheels/latest/azureml.logging-1.0.79-py3-none-any.whl?sv=2016-05-31&si=ro-2017&sr=c&sig=xnUdTm0B%2F%2FfknhTaRInBXyu2QTTt8wA3OsXwGVgU%2BJk%3D
    - https://azuremldownloads.blob.core.windows.net/wheels/latest/azureml.dataprep-0.1.1711.15263-py3-none-any.whl?sv=2016-05-31&si=ro-2017&sr=c&sig=xnUdTm0B%2F%2FfknhTaRInBXyu2QTTt8wA3OsXwGVgU%2BJk%3D
    - applicationinsights==0.11.0
```

Notice in the above file we refer directly to the Conda environment using `name: myenv`. In `aml_config/local.runconfig` change the `CondaDependenciesFile` to point to `"aml_config/localenv_conda_dependencies.yml"` instead.
11. Return to the command prompt and remove your Conda environment `myenv` using the following command:

```
conda env remove -n myenv
```

Now create a new Conda environment (also called `myenv`) and point it to `aml_config/localenv_conda_dependencies.yml` instead.
12. From the command line, run the experiment on `my_script.py` again. This time we successfully run `my_script.py` in a local Conda environment. Does the `matplotlib` package version match what we specified in `conda_dependencies.yml`?  
It bears repeting that using Conda locally is the right approach if Docker is not available, but otherwise it's better to use Conda inside a Docker image because Docker can also handle certain system dependencies that Conda wasn't built to handle.

## Conclusions

When developing our code on a local machines (laptops) or on DSVMs:

1. Try to always use versions for packages in `conda_dependencies.yml`.
2. Leverage Docker images and Conda environments for every project. A Docker image manages system dependencies as well.
3. If Docker is not available, still leverage Conda environments but be prepared to deal with additional system dependencies when moving the project to staging or production. The most common ones are compiler-related (such as C++ and Fortran).
4. If you don't use Docker images and don't rely on Conda environments (in other words if you always run your projects using the local Python root executable), then you should expect to run into many headaches when 
  - upgrading any major Python packages (especially the data science and machine learning packages),
  - moving between different projects with different package dependencies
  - deploying the project to staging and production.
