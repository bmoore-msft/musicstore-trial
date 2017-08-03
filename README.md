# musicstore-trial
This is a sample app for trying out the capabilities of Azure Application Insights and Snapshot Debugging.  The original app can be found [here](http://mvcmusicstore.codeplex.com/).  This app has been slightly modified to help show some of the features in Azure Diagnostics (e.g. added some exceptions to trigger the snapshot debugger) and there's more detail on that below.  The sample app can be found running at [http://musicstore-trial.azurewebsites.net/](http://musicstore-trial.azurewebsites.net/)

## Projects in the Solution
There are 3 projects in the solution that will enable you to set up your own app and resource group for collecting and looking at diagnostics information.  Here's a description of each project.

#### MvcMusicStore
This is the web application itself. The original app can be found [here](http://mvcmusicstore.codeplex.com/).  This app has been slightly modified to help show some of the features in Azure Diagnostics.  The app is essentially read-only (you can't create accounts or buy anything) and some exceptions have been added in obvious places to show how Azure Diagnostics will handle them.  There is also a "bug" in the database to simulate tracking down an issue in a production database in Azure.  What that bug is will be left as the exercise.  The database for the web app is contained in the SQL bacpac which is in the Azure Resource Group deployment project.

#### MvcMusicStore.deployment
This is the deployment project that will create all the necessary resources in Azure to deploy the web app, SQL db, Application Insights and web tests.  Most of these resources have a "free" tier (though the default is standard) or you can use a [free trial](https://azure.microsoft.com/en-us/free/) to try this out.  This project contains 2 templates for 2 different steps in deployment - the first is deploying the application's resources, the second deploy web tests to simulate user load to create diagnostics data.

#### MvcMusicStore.LoadTest
This project contains the web tests, though they aren't really used in the walk through because the tests are deployed using an Azure Resource Manager template in the deployment project.  This project is included since the web tests were originally created by the project before they were migrated to the deployment template.

## Getting Started
If you want to try the sample yourself and see the diagnostics data do the following:

### Clone the Repo
Clone the repo to **c:\source** - that path is not required but it will make following along easier.

### Build the Solution
Open the solution in Visual Studio 2017 (2017 required for the debugging features) and rebuild the solution.  This will install the nuget packages required for the app, which includes some new ones needed for [snapshot debugging](https://docs.microsoft.com/en-us/azure/application-insights/app-insights-snapshot-debugger).

### Deploy the Environment
The first step, is to deploy the resources for the environment, the next step will be to deploy the app itself.  Because a workflow may very will deploy the app many times (new releases, bug fixes) the app is not deployed with the template. It's possible to do so, but faster when separated.

Right-click on the MvcMusicStore.deployment project and create a new deployment.   Create a resource group and deploy the **azuredeploy.json** template. When running the deployment, be sure to change the parameter values for **siteName** and **sqlServerName** as these must be globally unique across Azure.  It's a good idea to change the sqlServerAdminLogin and of course, provide a password.  You don't have to save the password in the parameters file and it won't be unless you check the box to do so.

Deploying the environment will deploy the bacapc in the project to seed the database.  This step can take 10-15 minutes to complete.

### Publish the Web App
Once the environment is deployed, use the web publish wizard in to deploy the web application.  Select the web app you created in the previous step and don't worry about the connectStrings or the instrumentationKey for App Insights, those are handled by Azure through the deployment template.

### Add Web Tests
Now that the app is up and running you can deploy the web tests.  To do this deploy the webtest.json template in the deployment project.  Be sure to change the GUIDs in the parameters file that uniquely identify each web test.  Easily generate them using [https://www.guidgen.com/](https://www.guidgen.com/).

## Diagnostics
Once the app and tests are deployed App Insights will start collecting the data.  The web tests will trigger exceptions that can be viewed in the Azure portal and can be debugged using the [snapshot debugger](https://docs.microsoft.com/en-us/azure/application-insights/app-insights-snapshot-debugger).  Go to your resource group in the Azure portal and start exploring.  It may take a few minutes for the data to begin showing in App Insights and a bit longer for the exception count to be high enough to show you snapshots.  If you want to trigger the exception yourself, try registering a new account (which will trigger an exception) and refesh the page 5 times - this threshold will trigger snapshot collection.  After a few minutes you can drill into App Insights or the App Map and view the snapshots.

## Feedback
Questions or comments, feel free to send feedback, pull requests or issues.


