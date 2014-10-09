"use strict";

angular.module('application')

    .controller('LogoutController', [

        '$scope',
        '$location',
        '$cookies',
        'userService',
        'loaderService',

        function ($scope, $location, $cookies, userService, loaderService) {

            loaderService.showLoader();

            userService.logout(function () {
                $cookies.token = undefined;
                $location.path('/');
            });
        }
    ]
);
