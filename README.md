# Release Management Strategies for Mobile Developers

Continuous Integration/Continuous Deployment (CI/CD) for mobile developers is tricky. The CI part is straight-forward enough: you make your changes, push the code, and wait for the build to compile and the tests to pass. 

The CD part is where the bumps come in and getting your app in the hands of testers is of paramount importance. 

The sign and distribute process for apps is labor intensive, and worthwhile to get automated. Mobile devices are notorious for their different capabilities and screen sizes and apps are best deployed to real devices to be tested by humans. Plus, can you imagine the horror if the app you released to the store was buggy because the process you used to build and deploy to the store wasn’t the same as the test deployment?

Fear not! This repo is going to walk you through setting up a CI/CD pipeline for a mobile app with Azure DevOps and then distribute that app using App Center.

## Continuous Delivery ... of Value

You can think of Mobile DevOps as a lot of things. You're continuously delivering test builds of the app to your testers. You can setup releases to deliver to the App Stores. Unit and UI tests can run with every push to a branch.

But what each and every one of those things are really delivering is ... value.

By setting up a DevOps pipeline to run your builds, tests, and distribute your releases through - you are delivering value to your bosses, co-workers, testers, and most importantly customers by having a machine perform tasks in a repeatable manner.

And those tasks are trivial - like increasing a version number of a build - but if you forget it, or mess it up - can lead to big consequences. Which is why having the machine do it for you - over and over again - is so important.

## Four Stages of Mobile DevOps

When putting together a build pipeline, or a set of repeatable tasks that are run one after the other to compile, test, and possibly distribute an app, there are four stages that you need to keep in mind.

1. Continuous Integration and Testing - pull the code from the repo, build it, and ensure all tests pass.
1. Versioning - make sure any build has a unique version number and possibly app name to help distinguish it
1. Signing and Provisioning - especially important when deploying to the stores, keys and profiles need to be provided to guarantee you built the app and not some other company
1. Distributing - this could be simply emailing an Android APK file. Or it could hooking into the Apple App Store to deliver an app for eventual public distribution.

What's interesting is that while one could definitively assign a task to a particular stage. The tasks may not run in order.

In other words, you may find yourself running tasks from the **Versioning** and **Signing and Provisioning** stages before completing all the tasks in the **Continuous Integration and Testing** stage.

The tasks belong to a stage, but they can be intermingled in the order in which they are run.

## Build Pipeline

You can think of a pipeline in Mobile DevOps as a series of tasks. Each task will be run sequentially, one after the other. The tasks accomplish exactly one thing. So their scope is very narrow. As such, many tasks may be needed to fulfill each stage of the Mobile DevOps process.

With Azure DevOps, the build pipelines can be created in either YAML or in a visual editor. The advantage of creating a pipeline visually is ... well, it's visual and you get a drag and drop experience with everything laid out in front of you.

YAML however affords you the advantage to check your build pipeline into your source control repository. This way any changes to the pipeline can be checked-in and versioned just like any other piece of code. And when setting up continuous integration - the version of the YAML defined in the branch will be run - making it easy to setup builds for individual developers.

### General Workflow

A high-level workflow for building and distributing mobile applications is:

1. Kick off the build pipeline based on a trigger (manually or in reaction to a pull request or similar). As part of this, the branch of the code that will be built is determined and sent to the pipeline.
1. Download and restore any project dependencies - such as NuGet for Xamarin.
1. Take care of any version number maintenance along with any application naming, such as putting "beta" in the display name.
1. Build the project - creating the final package.
1. Sign the package, indicating you are who you say you are.
1. Distribute the package to testers, the store, or elsewhere.

Each platform, iOS, Android, Windows, and so on, will be built in its own pipeline. And while tasks in each pipeline must be run sequentially, the pipelines can be run synchronously.

### Continuous Integration and Testing

This stage involves retrieving the code from source control, running any tests on it, and compiling it into a final version that will be acted upon in the later stages.

When building a Xamarin application, there are a couple of tasks that are common to this stage and will need to be performed regardless of the app that you are building.

One of which is setting the NuGet version. Generally you'll want to set it to 4.4.1.

Another is downloading and restoring the NuGet packages for the project.

Building the project is necessary here.

And of course, if you happen to be running any unit or UI tests, those would happen in this stage as well.

#### Interesting Tidbits

When restoring the NuGets, it is faster if you restore directly against the Android `*.csproj` file. And it is faster if you have a separate solution file which only contains the iOS project and its necessary project dependencies. This just

### Versioning

Depending on the intention of the build, whether the resulting package will be deployed to testers or to the App Sotre or so on, you will need to update the build number. You may also want to change the name that's displayed on the device's screen so users know which version they are about to open.

There is a great community contributed versioning task that will accomplish just that. This task actually modifies the `AndroidManifest.xml` or `Info.plist` files to do its work. So technically it's not limited to versioning or naming.

But you will want to at least take care of the versioning before any builds occur. This way keeping your numbering clean for future bug reports and/or feature enhancements in your work tracking system.

#### Interesting Tidbits

One of the great things about Azure DevOps is that it's expandable. Anybody can write custom tasks for it, and submit those tasks as extensions to a _Marketplace_.

Check out the [marketplace here](https://marketplace.visualstudio.com/azuredevops/?WT.mc_id=mobile-0000-masoucou). In it you'll find many different types of extensions that you can put into your build pipelines - and more - like your Boards or Test Plans.

The extension I mentioned above that modifies the `Info.plist` or `AndroidManifest.xml` is called [Mobile App Tasks for iOS and Android](https://marketplace.visualstudio.com/items?itemName=vs-publisher-473885.motz-mobile-buildtasks&WT.mc_id=mobilereleasestrategies-github-masoucou) and it works on all types of iOS and Android projects, not just Xamarin ones.

### Signing and Provisioning

If you plan on doing anything other than building your app, this step(s) is very important. And it involves more than only tasks. It also involves having several files uploaded to the [**Library**](https://docs.microsoft.com/azure/devops/pipelines/library?WT.mc_id=mobile-0000-masoucou) portion of DevOps.

You need to upload either your [Android KeyStore file](https://docs.microsoft.com/xamarin/android/deploy-test/signing?WT.mc_id=mobile-0000-masoucou), or your [iOS certificate's P12 and provisioning file](https://docs.microsoft.com/xamarin/ios/get-started/installation/device-provisioning/?WT.mc_id=mobile-0000-masoucou). The Library is a safe spot to store these, as it's limited only to the pipelines in the project - meaning people not granted access to your project won't have access to your sensitive files.

Both the KeyStore and P12 files have passwords that go along with them. And you should store those in encrypted variables within your pipeline. You can read more about generating KeyStore files and P12 files here.

iOS provisioning profiles determine which devices an app can be installed on. You can have development profiles, ad-hoc profiles, and of course store distribution profiles. And each of those profiles must be associated with a certificate as well. All of these will be generated through Apple's developer portal. Again, find out more info here.

On Android, you'll sign the APK file. Which means you can do it after your build process is complete.

On iOS, the signing is a part of the build process. So you must first install the certificate and provisioning profile _before_ the build.

#### Interesting Tidbits

In an interesting twist, you can re-sign iOS IPA files after they have been built. You still need to install the certificate and provisioning profile to the machine - and supply the appropriate passwords and parameters. But there are built-in DevOps tasks that allow you to resign an IPA file. This way you can use the same IPA file that you did testing on for a release to a store as well.

### Distributing

Distributing the apps to testers or the store is where things get fun. There are tasks built right into DevOps that allow you to do distribution. However, I have found that using [App Center Distribution](https://docs.microsoft.com/appcenter/distribution/?WT.mc_id=mobile-0000-masoucou) to be an easier means to do so. And there happens to be a [DevOps task to deploy](https://docs.microsoft.com/appcenter/distribution/vsts-deploy?WT.mc_id=mobile-0000-masoucou) to App Center Distribution.

App Center Distribution allows you to hook into the [various stores](https://docs.microsoft.com/appcenter/distribution/stores/?WT.mc_id=mobile-0000-masoucou), be they [Google](https://docs.microsoft.com/appcenter/distribution/stores/googleplay?WT.mc_id=mobile-0000-masoucou), [Apple](https://docs.microsoft.com/appcenter/distribution/stores/apple?WT.mc_id=mobile-0000-masoucou), or whatever. It also allows you to harness the testing capabilities of each store. In other words, you can deploy to Apple's TestFlight for external beta testing.

Plus you can also [setup internal testing groups](https://docs.microsoft.com/appcenter/distribution/groups?WT.mc_id=mobile-0000-masoucou). With all of this control, App Center is the perfect abstraction layer for your distribution needs, and pair it with the fact that you can deploy to it with a DevOps task, it makes perfect sense to use it.

#### Interesting Tidbits

App Center allows you to do more than distribute builds. It has its own CI pipeline that uses DevOps under the hood. However, I find that using Azure DevOps to be more powerful.

App Center also offers a Mobile Backend as a Service (or MBaaS) with components ranging from push notifications, on-device data integration with Cosmos DB, and authentication with Azure AD B2C.

## Release Pipelines

It's entirely possible that you'll want to do more with your releases than a "one-time-shot" as can be accomplished with a build pipeline.

And that's where a [**Release Pipeline**](https://docs.microsoft.com/azure/devops/pipelines/release/?view=azure-devops&WT.mc_id=mobile-0000-masoucou) comes into play.

During a build pipeline, you can save the compiled application as an artifact. That artifact then can be passed to a release pipeline.

Within these pipelines you can do things such as require manual approval before a release is actually... well ... released.

So you can think of these release pipelines as wholly separate from the build pipelines and are meant to apply some advanced management to the release process.


## Summary

Azure DevOps provides a means to handle both building, testing, and releasing mobile applications.

It does this through pipelines, which are composed of tasks. The tasks only do one thing - like restore NuGet packages for a solution.

When thinking of how to structure the pipeline for your mobile app, there are 4 areas that you need to take into account.

1. Continuous Integration and Testing
1. Versioning
1. Signing and Provisioning
1. Distributing

Each area can be handled by one or more tasks. But tasks from the areas may not be run in order - for example, you may need to run versioning tasks before completing all the continuous integration tasks.

Each build pipeline will be unique to your application. However, with the concepts presented above in mind, you'll be well on your way to creating a solid, repeatable process that continuously delivers value to your users.

## More Info

For more information, these resources are invaluable:

* [A complete AND FREE course on building applications with Azure DevOps](https://docs.microsoft.com/learn/paths/build-applications-with-azure-devops/?WT.mc_id=mobile-0000-masoucou)
* [Xamarin.iOS provisioning info](https://docs.microsoft.com/xamarin/ios/get-started/installation/device-provisioning/?WT.mc_id=mobile-0000-masoucou)
* [Signing Android apps](https://docs.microsoft.com/xamarin/android/deploy-test/signing?WT.mc_id=mobile-0000-masoucou)
* [Info on preparing your Xamarin app for publishing](https://docs.microsoft.com/learn/modules/prepare-to-publish-your-xamarin-application/?WT.mc_id=mobile-0000-masoucou)