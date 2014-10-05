"use strict";

angular.module('application')

    .service('apiService', [

        '$cookies',
        '$http',
        'httpClientService',
        'API_SERVER_URL',

        function ($cookies, $http, httpClientService, API_SERVER_URL) {
            return {
                login: function (data, handler) {
                    httpClientService.sendRequest({
                        method: 'POST',
                        data: data,
                        url: API_SERVER_URL + '/api/login/'
                    }, handler);
                },
                signUp: function (data, handler) {
                    httpClientService.sendRequest({
                        method: 'POST',
                        data: data,
                        url: API_SERVER_URL + '/api/sign-up/'
                    }, handler);
                },
                getSituations: function (handler) {
                    var request = $http.get('/app/stubs/situations.json');
                    request.then(function (response) {
                        var data = response.data;
                        (handler.success || angular.noop)(data);
                    }, function (error) {
                        (handler.failure || angular.noop)(error);
                    });
                },
                search: function (data, handler) {
                    httpClientService.sendRequest({
                        method: 'POST',
                        data: {
                            token: $cookies.token || 'empty_token',
                            data: data
                        },
                        url: API_SERVER_URL + '/api/search/'
                    }, handler);
                }
            };
        }
    ]
);