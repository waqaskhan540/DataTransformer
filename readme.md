# Introduction 

A simple application to import csv file and store as Json and also in a SQL Database.

## Components

The application is composed of following components.

### Web API

The `API` component is responsible for uploading the `csv` file and storing it at a temporary location for further processing 

### JSON Transformation Service

This component is responsible for transforming the csv file into JSON and store on disk.

### SQL Transformation Service

This component is responsible for transforming the csv file and storing in a Database.

### Hangfire Server

Hangfire server is responsible for running background scheduled jobs to read csv files from storage and process them accordingly.


## Tools & SDKs

- Visual Studio 2019 (v16.8.3)
- SQL Server 2016
- .NET 5.0

## Steps to Run


Open `appsettings.json` and configure the following settings

```
 "ConnectionStrings": {
    "Default": "Data Source=.;Initial Catalog=db;Integrated Security=True"
  },
```
Configure storage locations for each job

```
  "JsonJobConfig": {
    "SourcePath": "D:\\JsonJob\\Src",
    "ProcessingPath": "D:\\JsonJob\\Processing",
    "FailedPath": "D:\\JsonJob\\Failed",
    "ProcessedPath": "D:\\JsonJob\\Processed"
  },

  "DbJobConfig": {
    "SourcePath": "D:\\DbJob\\Src",
    "ProcessingPath": "D:\\DbJob\\Processing",
    "FailedPath": "D:\\DbJob\\Failed",
    "ProcessedPath": "D:\\JsonJob\\Processed"
  },

```
- `SourcePath` is the location where `api` will store the uploaded `csv` file
- `ProcessingPath` is the location to store files which are being processed
- `FailedPath` is the location where files which couldn't be processed will be stored
- `ProcessedPath` is store the processed files

## Running as Standalone

To run the application in standalone mode please simply run the API project and upload the `csv` file from  `swagger`.

1. `FileUploadController` receives the request and delegates to `IFileUploadService` 
2. `IFileUploadService` copies the file to `SourcePath` folder for each transformer 
3. `IMediator` publishes a notification of type `FileUploadedEvent`
4. `DbTransformer` and `JsonTransformer` listen to the above published notification and start processing the uploaded file

## Running with Hangfire Server

To run the application with Hangfire server follow below steps.

1. Comment below code from `FileUploadController` to stop publishing notification
> await _mediator.Publish(new FileUploadedEvent
                {
                    FileName = filename
                });
                
2. Set both `Data.Transformer.Api` and `DataTransformer.Hangfire.Server` projects as startup projects and run both
3. Upload `csv` file from `Swagger` 
4. Two Hangfire background services are scheduled to run every 30 minutes, but can be triggered manually from hangfire dashboard, jobs can be triggered by going to `https://localhost:44364/hangfire/recurring` 
5. Trigger `JsonTransformationJob` manually and once the processing is finished, the processed Json file should be available at `ProcessedPath` for `JsonJobConfig`
6. Trigger `SqlTransformationJob` manually and after processing is finished, records should be available in the `Products` table of database.

## Possible Improvements

1. Monitoring for files i.e. `pending` , `processing` , `processed` or `failed`
2. Retry logic with `Polly` (or any) to try processing the file for few times before moving to Failed folder.
