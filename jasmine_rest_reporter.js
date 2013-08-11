(function() {
    if (! jasmine) {
        throw new Exception("jasmine library does not exist in global namespace!");
    }

    /**
     * Basic reporter that outputs spec results to the browser console.
     * Useful if you need to test an html page and don't want the TrivialReporter
     * markup mucking things up.
     *
     * Usage:
     *
     * jasmine.getEnv().addReporter(new jasmine.ConsoleReporter());
     * jasmine.getEnv().execute();
     */
    var RESTReporter = function() {
        this.started = false;
        this.finished = false;

        this.executed_specs = 0;
        this.passed_specs = 0;

        this.logCounter = 0;
        this.logs = new Array();
    };

    RESTReporter.prototype = {
        /*
        specFilter: function (spec) {
            return null
        },
        */

        reportRunnerResults: function(runner) {
            this.logSuiteResult(this.executed_specs, this.executed_specs - this.passed_specs);

            this.logAll();

             this.finished = true;
        },

        reportRunnerStarting: function(runner) {
            this.started = true;

            this.executed_specs = 0;
            this.passed_specs = 0;

        },

        reportSpecResults: function(spec) {
                var resultText = "Failed.";

                if (spec.results().passed()) {
                    this.passed_specs++;
                    resultText = "Passed.";
                }

                this.logSpecResult(spec.suite.description, spec.description, resultText);
        },

        reportSpecStarting: function(spec) {
            this.executed_specs++;
        },

        reportSuiteResults: function(suite) {
        },

        log: function(str) {
            var console = jasmine.getGlobal().console;
            if (console && console.log) {
                console.log(str);
            }

            this.logAjax(str);
        },

        logSpecResult: function (suiteDescription, specDescription, resultText) {
            this.logs.push({ action: 'SpecResult', payload: { suiteDescription: suiteDescription, specDescription: specDescription, resultText: resultText } });
            //this.logAjax('SpecResult', suiteDescription + '|' + specDescription + '|' + resultText);
        },

        logSuiteResult: function (numberOfSpecs, numberOfFails) {
            this.logs.push({ action: 'SuiteResult', payload: { numberOfSpecs: numberOfSpecs, numberOfFails: numberOfFails } });
            //this.logAjax('SuiteResult', numberOfSpecs + "|" + numberOfFails);
        },

        logAll: function () {
            this.logCounter = 0;

            this.logAjax(this.logs[this.logCounter].action, this.logs[this.logCounter].payload);
        },

        logAjax: function (action, payload) {

            $.ajax({
                context: this,

                type: 'GET',

                //url: 'http://192.168.0.6:8181/api/logger/1/jsonp',
                url: 'http://192.168.0.6:8181/api/Logger/' + action,

                //crossDomain: true,
                //contentType: 'application/json; charset=utf-8',

                data: payload,
                dataType: 'jsonp',

                success: function (result) {
                    
                    this.logCounter++;

                    if (this.logCounter < this.logs.length)
                        this.logAjax(this.logs[this.logCounter].action, this.logs[this.logCounter].payload);

                },
                //error: function (jqXHR, textStatus, errorThrown) { alert(errorThrown); },

                async: false
                //cache: false
            });
        }

    };

    // export public
    jasmine.RESTReporter = RESTReporter;
})();