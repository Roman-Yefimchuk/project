"use strict";

angular.module('application')

    .controller('MasterResultController', [

        '$rootScope',
        '$scope',
        '$location',
        'apiService',
        'loaderService',
        'userService',
        'dialogsService',

        function ($rootScope, $scope, $location, apiService, loaderService, userService, dialogsService) {

            var data = $rootScope.data;
            $rootScope.data = null;

            function setExistProblem(problem) {
                $scope.masterProblem = problem;
                $scope.showDropdown = false;
            }

            function setExistSolution(solution) {
                $scope.masterSolution = solution;
                $scope.showDropdown = false;
            }

            function clear(property) {
                $scope[property] = '';
            }

            function sendMasterSolution() {

                var entityTypeId = $scope.entityTypeId;
                var entityAccessoryId = $scope.entityAccessoryId;
                var ports = $scope.ports;
                var problem = $scope.masterProblem;
                var solution = $scope.masterSolution;

                apiService.addMasterSolution(entityTypeId, entityAccessoryId, ports, problem, solution, {
                    success: function () {
                        dialogsService.showAlert({
                            title: "Успіх",
                            message: "Ви успішно внесли свій вклад у вирішення даної проблеми",
                            onClose: function (closeCallback) {
                                closeCallback();
                                $location.path('/master');
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

            $scope.showProblemDropdown = false;
            $scope.showSolutionDropdown = false;

            $scope.masterProblem = "";
            $scope.masterSolution = "";

            $scope.sendMasterSolution = sendMasterSolution;
            $scope.clear = clear;
            $scope.setExistProblem = setExistProblem;
            $scope.setExistSolution = setExistSolution;

            loaderService.showLoader();

            userService.getData({
                success: function (user) {

                    $scope.user = user;

                    apiService.getMasterSolution(data, {
                        success: function (response) {

                            $scope.entityTypeId = response['entityTypeId'];
                            $scope.entityAccessoryId = response['entityAccessoryId'];
                            $scope.entityType = response['entityType'];
                            $scope.entityAccessory = response['entityAccessory'];
                            $scope.ports = response['ports'];

                            var suggestions = [];

                            _.forEach(response['suggestions'], function (suggestion) {
                                suggestions.push({
                                    problem: suggestion['Problem'],
                                    solution: suggestion['Solution']
                                });
                            });

                            $scope.suggestions = suggestions;

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
