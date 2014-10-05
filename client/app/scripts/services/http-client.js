'use strict';

angular.module('application')

    .service('httpClientService', [

        '$http',

        function ($http) {

            return {
                sendRequest: function (params, handler) {

                    var successCallback = handler.success || angular.noop;
                    var failureCallback = handler.failure || angular.noop;

                    var request = $http(params);

                    request.success(function (response, status, headers, config) {

                        if (response) {

                            if (response.status) {
                                successCallback(response.data || {});
                            } else {
                                var error = response.error;
                                failureCallback(error);
                            }
                        } else {
                            failureCallback("EMPTY_SERVER_RESPONSE");
                        }
                    });

                    request.error(function (data, status, headers, config) {
                        (handler.failure || angular.noop)(status);
                    });
                }
            };
        }
    ]
);