"use strict";

angular.module('application')

    .controller('ProblematicSituationDialogController', [

        '$scope',
        '$modalInstance',
        '$q',
        'options',
        'apiService',

        function ($scope, $modalInstance, $q, options, apiService) {

            var defaultCloseCallback = function (closeCallback) {
                closeCallback();
            };

            var dropDownModel = {
                entityTypeDropDown: false,
                entityModelDropDown: false,
                problemDropDown: false
            };

            var optionModel = {
                entityType: "",
                entityModel: "",
                problem: "",
                solution: ""
            };

            var onAdd = options.onAdd || defaultCloseCallback;
            var onCancel = options.onCancel || defaultCloseCallback;

            var allEntityTypes = null;
            var allEntityModels = null;
            var allProblems = null;

            function get$Q(callback) {
                var deferred = $q.defer();
                callback(deferred.resolve, deferred.reject);
                return deferred.promise;
            }

            $q.all({
                entityTypes: get$Q(function (resolve, reject) {
                    apiService.getEntityTypes({
                        success: function (data) {

                            var entityTypes = [];

                            _.forEach(data, function (item) {
                                entityTypes.push({
                                    id: item['Id'],
                                    name: item['Name'],
                                    type: item['Type']
                                });
                            });

                            resolve(entityTypes);
                        },
                        failure: function (error) {
                            reject(error);
                        }
                    });
                }),
                entityModels: get$Q(function (resolve, reject) {
                    apiService.getEntityModels({
                        success: function (data) {

                            var entityModels = [];

                            _.forEach(data, function (item) {
                                if (item['EntityTypes'] == '#any') {
                                    entityModels.push({
                                        id: item['Id'],
                                        model: item['Model'],
                                        entityTypes: '#any'
                                    });
                                } else {
                                    entityModels.push({
                                        id: item['Id'],
                                        model: item['Model'],
                                        entityTypes: item['EntityTypes'].split(',')
                                    });
                                }
                            });

                            resolve(entityModels);
                        },
                        failure: function (error) {
                            reject(error);
                        }
                    });
                }),
                problems: get$Q(function (resolve, reject) {
                    apiService.getProblems({
                        success: function (data) {

                            var problems = [];

                            _.forEach(data, function (item) {
                                if (item['EntityTypes'] == '#any') {
                                    problems.push({
                                        id: item['Id'],
                                        description: item['Description'],
                                        entityTypes: '#any'
                                    });
                                } else {
                                    problems.push({
                                        id: item['Id'],
                                        description: item['Description'],
                                        entityTypes: item['EntityTypes'].split(',')
                                    });
                                }
                            });

                            resolve(problems);
                        },
                        failure: function (error) {
                            reject(error);
                        }
                    });
                })
            }).then(function (values) {
                $scope.allEntityTypes = allEntityTypes = values.entityTypes;
                $scope.allEntityModels = allEntityModels = values.entityModels;
                $scope.allProblems = allProblems = values.problems;
            });

            function add() {
                onAdd(function () {
                    $modalInstance.close();
                });
            }

            function cancel() {
                onCancel(function () {
                    $modalInstance.close();
                });
            }

            function isAddButtonDisabled() {
                return !(optionModel.entityType && optionModel.entityModel &&
                    optionModel.problem && optionModel.solution);
            }

            $scope.dropDownModel = dropDownModel;
            $scope.optionModel = optionModel;

            $scope.add = add;
            $scope.cancel = cancel;
            $scope.isAddButtonDisabled = isAddButtonDisabled;
        }
    ]
);
