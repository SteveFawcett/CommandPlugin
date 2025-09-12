# PLUGIN_NAME

## Purpose

## Configuration [stanza]

## 

## Building the template

```
dotnet build --configuration:Release --target:Template -p:OutputDirectory=".\template"
```

## Delivering a Project that uses a Template
```
dotnet publish --verbosity:detailed -p:Configuration=Debug -p:AssemblyVersion="1.0.0"  -r win-x64 -p:PlatformTarget=x64 -t:Deliver
```