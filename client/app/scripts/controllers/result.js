"use strict";

angular.module('application')

    .controller('ResultController', [

        '$scope',
        '$routeParams',
        '$location',
        'apiService',
        'loaderService',
        'userService',
        'dialogsService',

        function ($scope, $routeParams, $location, apiService, loaderService, userService, dialogsService) {

            var entityTypeId = $routeParams.entityTypeId;
            var entityModelId = $routeParams.entityModelId;
            var problemId = $routeParams.problemId;

            function setExistSolution(solution) {
                $scope.userSolution = solution;
                $scope.showDropdown = false;
            }

            function sendUserSolution() {
                apiService.addSolution(entityTypeId, entityModelId, problemId, $scope.userSolution, {
                    success: function () {
                        dialogsService.showAlert({
                            title: "Успіх",
                            message: "Ви успішно внесли свій вклад у вирішення даної проблеми",
                            onClose: function (closeCallback) {
                                closeCallback();
                                $location.path('/user');
                            }
                        });
                    },
                    failure: function (error) {
                        dialogsService.showAlert({
                            title: "Помилка",
                            message: "Неможливо виконати запит",
                            onClose: function (closeCallback) {
                                closeCallback();
                            }
                        });
                    }
                });
            }

            $scope.showDropdown = false;
            $scope.userSolution = "";

            $scope.sendUserSolution = sendUserSolution;
            $scope.setExistSolution = setExistSolution;

            loaderService.showLoader();

            userService.getData({
                success: function (user) {

                    $scope.user = user;

                    apiService.getSolution(entityTypeId, entityModelId, problemId, {
                        success: function (response) {

                            $scope.entityType = response['EntityType'];
                            $scope.entityModel = response['EntityModel'];
                            $scope.problem = response['Problem'];

                            var suggestions = [];

                            _.forEach(response['Suggestions'], function (suggestion) {
                                suggestions.push({
                                    text: suggestion['Text'],
                                    weight: suggestion['Weight']
                                });
                            });

                            $scope.suggestions = _.sortBy(suggestions, function (suggestion) {
                                return -suggestion.weight;
                            });

                            loaderService.hideLoader();
                        },
                        failure: function (error) {
                            loaderService.hideLoader();
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
