
# Deployment notes

There are different ways to compile and deploy Azure Functions. Sometime it's a mess. :(

[*** DRAFT ***]


## Create a local .zip file for ZIP deployment
The output zip must start from the files insiede abc folder and must not have an inside folder 'abc'.  The option abc\\* solve the issue.
```
dotnet publish BasicFunctions -c Release -o _temp\abc
powershell Compress-Archive _temp\abc\* _temp\abc.zip -Force
```

## Azure pipelines

### Compile as ZipDeploy (mode I)
Details here: https://docs.microsoft.com/en-us/azure/devops/pipelines/tasks/deploy/azure-function-app?view=azure-devops#error-publish-using-zip-deploy-option-is-not-supported-for-msbuild-package-type
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

### Compile as ZipDeploy (mode II)
```yaml
- task: DotNetCoreCLI@2
  displayName: 'Publish the project - $(buildConfiguration)'
  inputs:
    command: 'publish'
    projects: '**/*.csproj'
    publishWebProjects: false
    arguments: '--no-build --configuration $(buildConfiguration) --output $(Build.ArtifactStagingDirectory)/$(buildConfiguration)'
    zipAfterPublish: true
```

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


