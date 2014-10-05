"use strict";

angular.module('application')

    .controller('MasterController', [

        '$scope',
        '$location',
        'loaderService',
        'userService',
        'apiService',

        function ($scope, $location, loaderService, userService, apiService) {

            loaderService.showLoader();

            userService.getData({
                success: function (user) {

                    apiService.getSituations({
                        success: function (data) {

                            $scope.user = user;
                            $scope.data = data;
                            loaderService.hideLoader();
                        },
                        failure: function (error) {
                            alert(error);
                        }
                    });
                },
                failure: function (error) {
                    $location.path('/');
                }
            });
        }
    ]
);
