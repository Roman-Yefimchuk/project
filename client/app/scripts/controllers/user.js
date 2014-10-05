"use strict";

angular.module('application')

    .controller('UserController', [

        '$scope',
        '$location',
        'loaderService',
        'userService',

        function ($scope, $location, loaderService, userService) {

            loaderService.showLoader();



            userService.getData({
                success: function (user) {
                    $scope.user = user;
                    loaderService.hideLoader();
                },
                failure: function (error) {
                    $location.path('/');
                }
            });
        }
    ]
);
