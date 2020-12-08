
# Deployment notes

[*** DRAFT ***]

There are different ways of deploying Azure Functions:
 - deploy from Visual Studio : WebDeploy OR ZipDeploy
 - deploy from Azure DevOps : ZipDeploy
 - other...

<br/>
<br/>

## Visual Studio
 1. download the Azure Function publish profile file (.pubxml)
 2. import it: right click on project --> Publish --> New --> Import Profile
 3. deploy the app: right click on project --> Publish --> select the profile --> press [Publish]

<br/>
<br/>

## Azure pipelines

### Build

**Compile for ZipDeploy (mode I) : MSBuild + ArchiveFiles**

Output: $(build.artifactStagingDirectory)/BasicFunctionsZD.zip  
[&rarr; Details here](https://docs.microsoft.com/en-us/azure/devops/pipelines/tasks/deploy/azure-function-app?view=azure-devops#error-publish-using-zip-deploy-option-is-not-supported-for-msbuild-package-type)
```yaml
- task: VSBuild@1
  inputs:
    solution: '**/BasicFunctions.csproj'
    msbuildArgs: '/p:DeployOnBuild=true /p:DeployDefaultTarget=WebPublish /p:WebPublishMethod=FileSystem /p:DeleteExistingFiles=True /p:publishUrl="$(build.artifactStagingDirectory)\BasicFunctionsZD"'
    platform: '$(buildPlatform)'
    configuration: '$(buildConfiguration)'
- task: ArchiveFiles@2
  inputs:
    rootFolderOrFile: '$(build.artifactStagingDirectory)/BasicFunctionsZD'
    includeRootFolder: false
    archiveType: 'zip'
    archiveFile: '$(build.artifactStagingDirectory)/BasicFunctionsZD.zip'
    replaceExistingArchive: true
```

**Compile for ZipDeploy (mode II): dotnet publish with autozip**

Output: $(Build.ArtifactStagingDirectory)/CmdBasedFunctionZD/CmdBasedFunction.zip
```yaml
- task: DotNetCoreCLI@2
  displayName: 'Publish the project - CmdBasedFunction'
  inputs:
    command: 'publish'
    projects: '**/CmdBasedFunction.csproj'
    publishWebProjects: false
    arguments: '--no-build --configuration $(buildConfiguration) --output $(Build.ArtifactStagingDirectory)/CmdBasedFunctionZD'
    zipAfterPublish: true
```
<br/>
<br/>

### Deployment
```yaml
- task: AzureFunctionApp@1
  inputs:
    azureSubscription: 'subscription_name'
    appType: 'functionApp'
    appName: 'appname'
    package: '$(System.ArtifactsDirectory)/**/BasicFunctionsZD.zip'
    deploymentMethod: 'auto'
```

Example: https://docs.microsoft.com/en-us/learn/modules/deploy-azure-functions/4-deploy-azure-function

<br/>
<br/>
<br/>

## Misc
### ZipDeploy breaks traditional Visual Studio publish from profile

After deploying from DevOps Pipeline, the traditional WebDeploy from Visual Studio hangs with errors like:
```
2>An error occurred when the request was processed on the remote computer.
2>Could not find file 'D:\home\site\wwwroot\App_Offline.htm'. 
```
This is because DevOps task AzureFunctionApp@1 sets the Funtcion App to configuration to run-from-package:
```
{"WEBSITE_RUN_FROM_PACKAGE":"1"}
```
Delete the entry if you need to deploy from Visual Studio via WebDeploy.


### Create a local .zip file for ZIP deployment
The output zip must start from the files insiede abc folder and must not have an inside folder 'abc'.  The option abc\\* solve the issue.
```
dotnet publish BasicFunctions -c Release -o _temp\abc
powershell Compress-Archive _temp\abc\* _temp\abc.zip -Force
```

### Publish & Download artifacts

 - 'pipeline' version (NEW) : PublishPipelineArtifact@1 DownloadPipelineArtifact@2
 - 'build' version (OLD) : PublishBuildArtifacts@1 DownloadBuildArtifacts@0 


Note: if you use the old PublishBuildArtifacts and get the artifact with DownloadPipelineArtifact, you'lle get this misleading warning.
```
##[warning]Please use Download Build Artifact task for downloading Build Artifact type artifact. 
```
It should be "use the new PublishPipelineArtifact instead of PublishBuildArtifacts in the source pipeline".



 [&rarr; documentation](https://docs.microsoft.com/en-us/azure/devops/pipelines/artifacts/build-artifacts?view=azure-devops&tabs=yaml)

