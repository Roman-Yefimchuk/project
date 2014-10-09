"use strict";

angular.module('application')

    .controller('UserController', [

        '$scope',
        '$location',
        '$q',
        'loaderService',
        'userService',
        'apiService',

        function ($scope, $location, $q, loaderService, userService, apiService) {

            var dropDownModel = {
                entityTypeDropDown: false,
                entityModelDropDown: false,
                problemDropDown: false
            };

            var userOptionModel = {
                entityType: null,
                entityModel: null,
                problem: null
            };

            var allEntityTypes = null;
            var allEntityModels = null;
            var allProblems = null;

            function setEntityType(entityType) {

                if (userOptionModel.entityType != entityType) {
                    userOptionModel.entityType = entityType;

                    updateEntityModels();
                    updateProblems();
                }

                dropDownModel.entityTypeDropDown = false;
            }

            function setEntityModel(entityModel) {

                if (userOptionModel.entityModel != entityModel) {
                    userOptionModel.entityModel = entityModel;
                }

                dropDownModel.entityModelDropDown = false;
            }

            function setProblem(problem) {
                userOptionModel.problem = problem;
                dropDownModel.problemDropDown = false;
            }

            function updateEntityModels() {
                var entityType = userOptionModel.entityType;
                var entityModels = [];

                _.forEach(allEntityModels, function (entityModel) {
                    if (entityModel.entityTypes == '#any') {
                        entityModels.push(entityModel);
                    } else {
                        if (_.contains(entityModel.entityTypes, entityType.type)) {
                            entityModels.push(entityModel);
                        }
                    }
                });

                if (!_.contains(entityModels, userOptionModel.entityModel)) {
                    userOptionModel.entityModel = null;
                }

                $scope.entityModels = entityModels;
            }

            function updateProblems() {
                var entityType = userOptionModel.entityType;
                var problems = [];

                _.forEach(allProblems, function (problem) {
                    if (problem.entityTypes == '#any') {
                        problems.push(problem);
                    } else {
                        if (_.contains(problem.entityTypes, entityType.type)) {
                            problems.push(problem);
                        }
                    }
                });

                if (!_.contains(problems, userOptionModel.problem)) {
                    userOptionModel.problem = null;
                }

                $scope.problems = problems;
            }

            function getSolution() {
                var resultPath = $location.path('/result');
                resultPath.search({
                    entityTypeId: userOptionModel.entityType['id'],
                    entityModelId: userOptionModel.entityModel['id'],
                    problemId: userOptionModel.problem['id']
                });
            }

            function getEntityTypesSelectTitle() {
                if ($scope.entityTypes['length'] > 0) {
                    if (userOptionModel.entityType) {
                        return userOptionModel.entityType['name'];
                    }
                    return 'Не вибрано';
                }
                return 'Немає доступних варіантів';
            }

            function getEntityModelsSelectTitle() {
                if ($scope.entityModels['length'] > 0) {
                    if (userOptionModel.entityModel) {
                        return userOptionModel.entityModel['model'];
                    }
                    return 'Не вибрано';
                }
                return 'Немає доступних варіантів';
            }

            function getProblemsSelectTitle() {
                if ($scope.problems['length'] > 0) {
                    if (userOptionModel.problem) {
                        return userOptionModel.problem['description'];
                    }
                    return 'Не вибрано';
                }
                return 'Немає доступних варіантів';
            }

            $scope.dropDownModel = dropDownModel;
            $scope.userOptionModel = userOptionModel;

            $scope.entityTypes = [];
            $scope.entityModels = [];
            $scope.problems = [];

            $scope.setEntityType = setEntityType;
            $scope.setEntityModel = setEntityModel;
            $scope.setProblem = setProblem;

            $scope.updateEntityModels = updateEntityModels;
            $scope.updateProblems = updateProblems;

            $scope.getSolution = getSolution;

            $scope.getEntityTypesSelectTitle = getEntityTypesSelectTitle;
            $scope.getEntityModelsSelectTitle = getEntityModelsSelectTitle;
            $scope.getProblemsSelectTitle = getProblemsSelectTitle;

            loaderService.showLoader();

            userService.getData({
                success: function (user) {

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

                        allEntityTypes = values.entityTypes;
                        allEntityModels = values.entityModels;
                        allProblems = values.problems;

                        $scope.entityTypes = values.entityTypes;
                        $scope.user = user;

                        loaderService.hideLoader();
                    });
                },
                failure: function (error) {
                    $location.path('/');
                }
            });
        }
    ]
);
