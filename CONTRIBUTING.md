###
Build and push nuget package
```
cd .\ReusableEfCoreIncludes\
rm bin
dotnet pack --configuration Release
dotnet nuget push ReusableEfCoreIncludes.<version>.nupkg --api-key <api_key> --source https://api.nuget.org/v3/index.json
```