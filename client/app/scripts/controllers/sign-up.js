"use strict";

angular.module('application')

    .controller('SignUpController', [

        '$scope',
        '$location',
        '$cookies',
        'loaderService',
        'apiService',
        'NAME_PATTERN',
        'PASSWORD_PATTERN',

        function ($scope, $location, $cookies, loaderService, apiService, NAME_PATTERN, PASSWORD_PATTERN) {

            function isNameValid() {
                var name = ($scope.name || '');
                return NAME_PATTERN.test(name);
            }

            function isPasswordValid() {
                var password = ($scope['password'] || '').toLowerCase();
                return PASSWORD_PATTERN.test(password) && $scope.password == $scope.retypedPassword;
            }

            function quickSingUp() {
                $scope.name = 'User';
                $scope.password = 'qwerty';

                signUp();
            }

            function signUp() {

                loaderService.showLoader();

                $scope.errorMessage = null;

                apiService.signUp({
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
            $scope.retypedPassword = "";

            $scope.isNameValid = isNameValid;
            $scope.isPasswordValid = isPasswordValid;
            $scope.quickSingUp = quickSingUp;
            $scope.signUp = signUp;
        }
    ]
);
