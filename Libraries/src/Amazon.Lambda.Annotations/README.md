# Amazon.Lambda.Annotations

The Lambda Annotations is a programming model for writing .NET Lambda function. At a high level the programming model allows
idiomatic .NET coding patterns and uses [C# Source Generators](https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview) to bridge the gap between the Lambda programming model 
to the more idiomatic programming model.

For example here is a simplistic example of a .NET Lambda function that acts like a calculator plus method using the normal 
Lambda programming model. It respondes to an API Gateway REST API, pulls the operands from the resource paths, does the 
addition and returns back an API Gateway response.
```csharp
public class Functions
{
    public APIGatewayProxyResponse LambdaMathPlus(APIGatewayProxyRequest request, ILambdaContext context)
    {
        if (!request.PathParameters.TryGetValue("x", out var xs))
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }
        if (!request.PathParameters.TryGetValue("y", out var ys))
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }

        var x = int.Parse(xs);
        var y = int.Parse(ys);

        return new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = (x + y).ToString(),
            Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
        };
    } 
}
```

Using Amazon.Lambda.Annotations the same Lambda function can be written like this.
```csharp
public class Functions
{
    [LambdaFunction]
    [RestApi("/plus/{x}/{y}")]
    public int Plus(int x, int y)
    {
        return x + y;
    }
}
```

## Using References To Other Resources and Parameters in the template

To use a reference to a Resource or Parameter in the template, prefix the value with `@`.  Example shows using CloudFormation template Parameter named `LambdaRoleParameter` for the role of the Lambda function.
```csharp
public class Functions
{
    [LambdaFunction( Role="@LambdaRoleParameter")]
    [RestApi("/plus/{x}/{y}")]
    public int Plus(int x, int y)
    {
        return x + y;
    }
}
```

and place in your template:

```

  "Parameters": {
    "LambdaExecutionRole": {
      "Type": "String"
    }
  },

```

The above two examples when used together will the use the value of LambdaRoleParameter as the role during deployment.

## Source Generator

To bridge the gap from Lambda Annotations programming model to the normal programming model a .NET source generator is included in this package.
After adding the attributes to your .NET code the source generator will generate the translation code between the 2 programming models. It will also
keep in sync the generated information including a new function handler string into the CloudFormation template. The usage of source
generator is transparent to the user. The source generator supports syncing of both JSON and YAML CloudFormation templates.
```csharp
[LambdaFunction(Name = "Plus")]
[RestApi(LambdaHttpMethod.Get, "/plus/{x}/{y}")]
public int Plus(int x, int y)
{
    return x + y;
}
```
The source generator adds the following entry in the CloudFormation template corresponding to the above Lambda function:
```json
"Plus": {
      "Type": "AWS::Serverless::Function",
      "Metadata": {
        "Tool": "Amazon.Lambda.Annotations",
        "SyncedEvents": [
          "RootGet"
        ]
      },
      "Properties": {
        "Runtime": "dotnet6",
        "CodeUri": ".",
        "MemorySize": 256,
        "Timeout": 30,
        "Policies": [
          "AWSLambdaBasicExecutionRole"
        ],
        "PackageType": "Zip",
        "Handler": "TestServerlessApp::TestServerlessApp.Functions_Plus_Generated::Plus",
        "Events": {
          "RootGet": {
            "Type": "Api",
            "Properties": {
              "Path": "/plus/{x}/{y}",
              "Method": "GET"
          }
        }
      }
    }
  }
```

To see the code that is generated by the source generator turn the verbosity to detailed when executing a build. From the command this 
is done by using the `--verbosity` switch.
```
dotnet build --verbosity detailed
```
To change the verbosity in Visual Studio go to Tools -> Options -> Projects and Solutions and adjust the MSBuild verbosity drop down boxes.

## Dependency Injection

Lambda Annotations supports using dependency injection. A class can be marked with a `LambdaStartup` attribute. The class will 
have a `ConfigureServices` method for configuring services.

The services can be injected by either constructor injection or using the `FromServices` attribute on a method parameter of
the function decorated with the `LambdaFunction` attribute.

Services injected via the constructor have a lifecycle for the length of the Lambda compute container. For each Lambda 
invocation a scope is created and the services injected using the `FromServices` attribute are created within the scope.

Example startup class:
```csharp
[LambdaStartup]
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAWSService<Amazon.S3.IAmazonS3>();
        services.AddScoped<ITracker, DefaultTracker>();
    }
}
```

Example function using DI:
```csharp
public class Functions
{
    IAmazonS3 _s3Client;

    public Functions(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }


    [LambdaFunction]
    [HttpApi(LambdaHttpMethod.Put, "/process/{name}", Version = HttpApiVersion.V2)]
    public async Task Process([FromServices] ITracker tracker, string name, [FromBody] string data)
    {
        tracker.Record();

        await _s3Client.PutObjectAsync(new PutObjectRequest
        {
            BucketName = "storage-bucket",
            Key = name,
            ContentBody = data
        });
    }
}
```

## Lambda .NET Attributes

List of .NET attributes currently supported.


* LambdaFunction
    * Placed on a method. Indicates this method should be exposed as a Lambda function.
* LambdaStartup
    * Placed on a class. Indicates this type should be used as the startup class and is used to configure the dependency injection and middleware. There can only be one class in a Lambda project with this attribute.

### Event Attributes    

Event attributes configuring the source generator for the type of event to expect and setup the event source in the CloudFormation temlate. If an event attribute is not set the
parameter to the `LambdaFunction` must be the event object and the event source must be configured outside of the code.

* RestApi
    * Configures the Lambda function to be called from an API Gateway REST API. The HTTP method and resource path are required to be set on the attribute.
* HttpApi
    * Configures the Lambda function to be called from an API Gateway HTTP API. The HTTP method, HTTP API payload version and resource path are required to be set on the attribute.

### Parameter Attributes

* FromHeader
    * Map method parameter to HTTP header value
* FromQuery
    * Map method parameter to query string parameter
* FromRoute
    * Map method parameter to resource path segment
* FromBody
    * Map method parameter to HTTP request body. If parameter is a complex type then request body will be assumed to be JSON and deserialized into the type.
* FromServices
    * Map method parameter to registered service in IServiceProvider


## Project References

If you are using API Gateway event attributes, such as `RestAPI` or `HttpAPI`, you need to manually add a package reference to `Amazon.Lambda.APIGatewayEvents` in your project, otherwise the project will not compile. We do not include it by default in order to keep the `Amazon.Lambda.Annotations` library lightweight.

This release only supports API Gateway Events. As we add support for other types of events, such as S3 or DynamoDB, the list of required package references will depend on the Lambda .NET attributes you are using. 

**Note**: If you are using [dependency injection](#dependency-injection) to write functions for other service events , such as S3 or DynamoDB, you will need to reference these packages in your project as well