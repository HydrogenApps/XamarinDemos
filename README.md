XamarinDemos
============

This repo contains a number of sample Xamarin apps that have been put together for doing presentation demos. Each demo folder will usually contain:

* The end solution
* Partial solutions that represent a step in the overall demo
* A codesnippets document that contains snippets of code that can be copy/paste during the demo
* A DemoScript document that outlines the steps to take during the demo. Contains references to the code snippets to use

## XplatChat

XplatChat, short for Cross-platform Chat, is a basic (and I mean real basic!) realtime chat app that can be run across iOS/Android/WinPhone. It uses SignalR client to connect up to a SignalR hub hosted in IIS.

End solution contains:

* XplatChat.Service.Endpoint - The ASP.Net project that hosts the SignalR chathub
* XplatChat.Contract.Pcl - Contains models to use when communicating with SignalR chathub
* XplatChat.NativeApp.* - An implementation of the app for each platform using platform specific views
* XplatChat.FormsApp.* - An implementation of the app using Xamarin.Forms views

A couple of notes:

* The url of the server is hardcoded so will need to be changed manually to suit your environment before you build
* nuget.config points NuGet to a locally stored NugetRepository (in case internet connectivity can't be relied on when you're presenting)
