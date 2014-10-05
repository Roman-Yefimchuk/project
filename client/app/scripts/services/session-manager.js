'use strict';

angular.module('application')

    .service('sessionManagerService', [

        '$cookies',
        'httpClientService',
        'API_SERVER_URL',

        function ($cookies, httpClientService, API_SERVER_URL) {
            var url = API_SERVER_URL + '/api/session-manager/';
            return {
                isAuthenticated: function (handler) {
                    httpClientService.sendRequest({
                        method: 'POST',
                        data: {
                            action: 'is_authenticated',
                            token: $cookies.token || 'empty_token'
                        },
                        url: url
                    }, handler);
                },
                getUserData: function (handler) {
                    httpClientService.sendRequest({
                        method: 'POST',
                        data: {
                            action: 'get_user_data',
                            token: $cookies.token || 'empty_token'
                        },
                        url: url
                    }, handler);
                },
                logout: function (handler) {
                    httpClientService.sendRequest({
                        method: 'POST',
                        data: {
                            action: 'logout',
                            token: $cookies.token || 'empty_token'
                        },
                        url: url
                    }, handler);
                }
            };
        }
    ]
);
