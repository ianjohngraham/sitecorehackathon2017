# Dropbox Provider for the Data Exchange Framework

This is a POC for integrating Dropbox with the Sitecore Media Library using the Sitecore Data Exchange Framework.

Please use at your own risk.

## Getting Started 
Download and install the Data Exchange Framework v1.3 and the Sitecore Provider here:

https://dev.sitecore.net/Downloads/Data_Exchange_Framework/1x/Data_Exchange_Framework_1_3.aspx

Install the Sitecore Package provided in the repository here: /SitecorePackage/Dropbox Provider for DEF-1.00.zip

Make a small change to your Web.config:

 <dependentAssembly>
        <assemblyIdentity name="Newtonsoft.Json" publicKeyToken="30ad4fe6b2a6aeed" />
        <bindingRedirect oldVersion="0.0.0.0-7.0.0.0" newVersion="6.0.0.0" />
 </dependentAssembly>


## Get a Token from Dropbox
 
 Got to: https://www.dropbox.com/developers/apps and login with a Dropbox account.
 
 Create a new App that will give you full Dropbox access.
 
 Generate an access token for your app.
 
 Add the token and the application name to the Endpoint settings here:
 
 /sitecore/system/Data Exchange/Dropbox Provider Tenant/Endpoints/Providers/Dropbox/Dropbox endpoint
 

# Using the Code

Just download the repository and add references to the Sitecore Libraries for the Data Exchange Framework and Core Sitecore.