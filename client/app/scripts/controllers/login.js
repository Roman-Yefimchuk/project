"use strict";

angular.module('application')

    .controller('LoginController', [

        '$scope',
        '$rootScope',
        '$location',
        '$cookies',
        'apiService',
        'loaderService',
        'NAME_PATTERN',
        'PASSWORD_PATTERN',

        function ($scope, $rootScope, $location, $cookies, apiService, loaderService, NAME_PATTERN, PASSWORD_PATTERN) {

            function isNameValid() {
                var name = ($scope['name'] || '').toLowerCase();
                return NAME_PATTERN.test(name);
            }

            function isPasswordValid() {
                var password = ($scope['password'] || '').toLowerCase();
                return PASSWORD_PATTERN.test(password);
            }

            function quickLogin() {
                $scope.name = 'Master';
                $scope.password = 'qwerty';

                login();
            }

            function login() {

                loaderService.showLoader();

                $scope.errorMessage = null;

                apiService.login({
                    name: $scope.name,
                    password: $scope.password
                }, {
                    success: function (response) {
                        switch (response['role']) {
                            case 'user':
                            {
                                $cookies.token = response.token;
                                $location.path("/user");
                                break;
                            }
                            case 'master':
                            {
                                $cookies.token = response.token;
                                $location.path("/master");
                                break;
                            }
                            default :
                            {
                                $scope.errorMessage = "Якась помилка";
                                loaderService.hideLoader();
                                break;
                            }
                        }
                    },
                    failure: function (error) {
                        $scope.errorMessage = error;

                        loaderService.hideLoader();
                    }
                });
            }

            $scope.errorMessage = null;
            $scope.name = "";
            $scope.password = "";

            $scope.isNameValid = isNameValid;
            $scope.isPasswordValid = isPasswordValid;
            $scope.quickLogin = quickLogin;
            $scope.login = login;
        }
    ]
);
