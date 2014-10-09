"use strict";

angular.module('application')

    .controller('MasterController', [

        '$scope',
        '$location',
        'loaderService',
        'userService',
        'apiService',

        function ($scope, $location, loaderService, userService, apiService) {

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

                function recycleEntityModels() {
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
                        userOptionModel.entityModel = entityModels[0];
                    }

                    $scope.entityModels = entityModels;
                }

                if (allEntityModels) {
                    recycleEntityModels();
                } else {
                    apiService.getEntityModels({
                        success: function (data) {
                            allEntityModels = [];

                            _.forEach(data, function (item) {
                                allEntityModels.push({
                                    id: item['Id'],
                                    model: item['Model'],
                                    entityTypes: item['EntityTypes'] == '#any' && '#any' || item['EntityTypes'].split(',')
                                });
                            });

                            recycleEntityModels();
                        },
                        failure: function (error) {
                            alert(error);
                        }
                    });
                }
            }

            function updateProblems() {

                function recycleProblems() {
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
                        userOptionModel.problem = problems[0];
                    }

                    $scope.problems = problems;
                }

                if (allProblems) {
                    recycleProblems();
                } else {
                    apiService.getProblems({
                        success: function (data) {
                            allProblems = [];

                            _.forEach(data, function (item) {
                                allProblems.push({
                                    id: item['Id'],
                                    description: item['Description'],
                                    entityTypes: item['EntityTypes'] == '#any' && '#any' || item['EntityTypes'].split(',')
                                });
                            });

                            recycleProblems();
                        },
                        failure: function (error) {
                            alert(error);
                        }
                    });
                }
            }

            function getSuggestions() {
                var resultPath = $location.path('/result');
                resultPath.search({
                    entityTypeId: userOptionModel.entityType['id'],
                    entityModelId: userOptionModel.entityModel['id'],
                    problemId: userOptionModel.problem['id']
                });
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

            $scope.getSuggestions = getSuggestions;

            loaderService.showLoader();

            userService.getData({
                success: function (user) {

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

                            userOptionModel.entityType = entityTypes[0];

                            $scope.entityTypes = entityTypes;
                            $scope.user = user;

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
