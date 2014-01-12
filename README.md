# Jasime REST reporter and receiver #

A reporter for use with the Jasmine BDD framework.  This reporter sends the results via REST to a receiver (Windows Console application).

The receiver has been written to be used with Jenkins CI to automate the testing of an Android application (although I'm sure it can be used for more).  The receiver is used by Jenkins to wait for the Jasmine tests to be run.  Jasmine, using the REST reporter, will send the results of each spec and suite to the receiver.  The receiver will error (returns an error code to Jenkins) if any of the tests fail, none of the tests run or after a timeout period (10 minutes by default).  Jenkins will then act of the result code.

## Change Log ##

* 12th January 2014 - Removed the hardcoding elements
* 11th August 2013 - Original version

# How to use #

## The Jasmine bit ##

The jasmine_rest_reporter.js is used within your Jasmine tests.  Using the example project from Jasmine site (https://github.com/pivotal/jasmine/downloads);

1) Copy the jasmine_rest_reporter.js into your project
2) Copy the jQuery library into your project (I'm using jquery-1.10.2.min.js).  The jasmine_rest_reporter.js used jQuery for the AJAX functionality.
3) Amend the PluginSpecRunner.html to include script links to those two .js files
4) Amend the PluginSpecRunner.html to include the following lines just after the htmlReporter is added to the jasmineEnv:

      var restReporter = new jasmine.RESTReporter("http://192.168.0.7:8181");
      jasmineEnv.addReporter(restReporter);

NOTE: That the specific URL will likely need to be amended for your environment.

## The receiver bit ##

The receiver provides the REST service that the reporter communicates it's results to.

It's a fairly basic application - it will print to the console the details it receives from the reporter.  It will also exit with an error code if any of the tests fail, none of the tests run or after a timeout period (5 minutes).

You can build the receiver using Visual Studio 2012 Express for Windows Desktop (or greater).

1) Create a new console application
2) Replace the Program.cs with copy from GitHUb
3) Copy in the FormatterConfig.cs and LoggerController.cs from GitHub into the project
4) Use Nuget to add the following packages:

* Json.NET
* Microsort ASP.NET Web API Self Host
* WebApiContrib.Formatting.Jsonp

This should give you a Console Application ready to use

The Receiver can be run with the following options:

* --help - produces a usage message
* --ipaddress={ipaddress} - set ip address to listen on - defaults to first local IP address 
* --port={port} - port to listen on - defaults to 8181
* --timeout={seconds} - number of seconds to wait for communication before timing out - defaults to 10 minutes


## The permissions bit ##

Windows needs to be told that the Console Application is allowed to listen on the given port.  To do this run the following command:

netsh http add urlacl url=http://+:8181/ user=machine\username

where machine\username is your user account.

NOTE: If you change the port that you run the Console Application on, then this will need to be reflected in the above command.

## Licence ##

The MIT License

Copyright (c) 2013 Red Folder Consultancy Ltd

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

