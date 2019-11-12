# Contract Driven API

Example implementation of the Consumer Driven Contract pattern for an API implemented in .NET Core


## The solution

The solution is composed of three projects:

- The API project is a demo .NET Core 2.2 API that exposes an endpoint to retrieve cryptocurrencies' price indexes. Currently the only cryptocurrency supported is bitcoin.

- The Consumer.Tests project defines with Pact tests what is expected from the API.

- The Producer.Tests project contains the tests to verify that the responses from the API match the mocked ones the HTTP requests/responses defined in the Pact file.

### To build the solution run:

```
    dotnet build ./src
```

### To run the tests run:

```
    dotnet test ./src
```
