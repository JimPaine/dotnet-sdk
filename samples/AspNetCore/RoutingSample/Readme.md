# ASP.NET Core Routing Sample

This sample shows using Dapr with ASP.NET Core routing. This application is a simple and not-so-secure banking application. The application uses the Dapr state-store for its data storage.

It exposes the following endpoints over HTTP:
 - GET `/{id}`: Get the balance for the account specified by `id`
 - POST `/deposit`: Accepts a JSON payload to deposit money to an account
 - POST `/withdraw`: Accepts a JSON payload to withdraw money from an account

The application also registers for pub-sub with the `deposit` and `withdraw` topics.

 ## Running the Sample

 To run the sample locally run this comment in this directory:
 ```sh
 dapr run --app-id routing --app-port 5000 dotnet run
 ```

 The application will listen on port 5000 for HTTP.

 ### Examples

**Deposit Money**

 ```sh
curl -X POST http://localhost:5000/deposit \
        -H 'Content-Type: application/json' \
        -d '{ "id": "17", "amount": 12 }'
 ```

```txt
 {"id":"17","balance":12}
```

 ---

**Withdraw Money**

 ```sh
curl -X POST http://localhost:5000/withdraw \
        -H 'Content-Type: application/json' \
        -d '{ "id": "17", "amount": 10 }'
 ```

```txt
{"id":"17","balance":2}
```

 ---

**Get Balance**

```sh
curl http://localhost:5000/17
```

```txt
{"id":"17","balance":2}
```

 ---

 **Withdraw Money (pubsub)**

```sh
dapr publish -t withdraw -p '{"id": "17", "amount": 15 }'
```

 ---

**Deposit Money (pubsub)**

```sh
dapr publish -t deposit -p '{"id": "17", "amount": 15 }'
```

 ---

## Code Samples

*All of the interesting code in this sample is in Startup.cs*

 ```C#
public void ConfigureServices(IServiceCollection services)
{
    services.AddDaprClient();

    ...
}
 ```

 `AddDaprClient()` registers the `StateClient` service with the dependency injection container. This service can be used to interact with the Dapr state-store.

---

```C#
app.UseCloudEvents();
```

`UseCloudEvents()` registers the Cloud Events middleware in the request processing pipeline. This middleware will unwrap requests with Content-Type `application/cloudevents+json` so that application code can access the event payload in the request body directly. This is recommended when using pub/sub unless you have a need to process the event metadata yourself.

---

```C#
app.UseEndpoints(endpoints =>
{
    endpoints.MapSubscribeHandler();

    endpoints.MapGet("{id}", Balance);
    endpoints.MapPost("deposit", Deposit).WithTopic("deposit");
    endpoints.MapPost("withdraw", Withdraw).WithTopic("withdraw");
});
```

`MapSubscribeHandler()` registers an endpoint that will be called by the Dapr runtime to register for pub-sub topics. This is is not needed unless using pub-sub.

`MapGet(...)` and `MapPost(...)` are provided by ASP.NET Core routing - these are used to setup endpoints to handle HTTP requests.

`WithTopic(...)` associates an endpoint with a pub-sub topic.

---

```C#
async Task Balance(HttpContext context)
{
    var client = context.RequestServices.GetRequiredService<StateClient>();

    var id = (string)context.Request.RouteValues["id"];
    var account = await client.GetStateAsync<Account>(id);
    if (account == null)
    {
        context.Response.StatusCode = 404;
        return;
    }

    context.Response.ContentType = "application/json";
    await JsonSerializer.SerializeAsync(context.Response.Body, account, serializerOptions);
}
```

Here `GetRequiredService<StateClient>()` is used to retrieve the `StateClient` from the service provider.

`client.GetStateAsync<Account>(id)` is used to retrieve an `Account` object from that state-store using the key in the variable `id`. The `Account` object stored in the state-store as JSON. If no entry is found for the specified key, then `null` will be returned.

---

```C#
await client.SaveStateAsync(transaction.Id, account);
```

`SaveStateAsync(...)` is used to save data to the state-store. 
