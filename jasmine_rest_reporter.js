(function() {
    if (! jasmine) {
        throw new Exception("jasmine library does not exist in global namespace!");
    }

    /**
     * Reported for Jasmine which communicates with a REST based receiver
     * https://github.com/Red-Folder/Jasmine-REST-Reporter
     *
     * Usage:
     *
     * jasmine.getEnv().addReporter(new jasmine.RestReporter("http://192.168.0.1:8181"));
     * jasmine.getEnv().execute();
     */
    var RESTReporter = function(listenerURL) {
        this.executed_specs = 0;
        this.passed_specs = 0;

        this.listenerURL = listenerURL
    };

    RESTReporter.prototype = {
        reportRunnerResults: function(runner) {
			this.log('SuiteResult', { numberOfSpecs: this.executed_specs, numberOfFails: this.executed_specs - this.passed_specs });
        },

        reportRunnerStarting: function(runner) {
            this.executed_specs = 0;
            this.passed_specs = 0;
        },

        reportSpecResults: function(spec) {
           var resultText = "Failed.";

           if (spec.results().passed()) {
              this.passed_specs++;
              resultText = "Passed.";
           }

           this.log('SpecResult', { suiteDescription: spec.suite.description, specDescription: spec.description, resultText: resultText });
        },

        reportSpecStarting: function(spec) {
            this.executed_specs++;
        },

        reportSuiteResults: function(suite) {
        },

        log: function (action, payload) {

            $.ajaxQueue({
                context: this,

                type: 'GET',

                url: this.listenerURL + '/api/Logger/' + action,

                data: payload,
                dataType: 'jsonp',

                async: false
            });
        }

    };

    // export public
    jasmine.RESTReporter = RESTReporter;
 })();