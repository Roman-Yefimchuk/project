"use strict";

angular.module('application')

    .controller('MasterController', [

        '$rootScope',
        '$scope',
        '$location',
        '$q',
        'loaderService',
        'userService',
        'apiService',
        'dialogsService',

        function ($rootScope, $scope, $location, $q, loaderService, userService, apiService, dialogsService) {

            var dropDownModel = {
                entityTypeDropDown: false,
                entityAccessoryDropDown: false
            };

            var userOptionModel = {
                entityType: null,
                entityAccessory: null
            };

            var clusteringOptions = {
                clustersNumber: 0,
                clusteringQuantity: 0
            };

            var allEntityTypes = null;
            var allEntityAccessories = null;

            function setEntityType(entityType) {

                if (userOptionModel.entityType != entityType) {
                    userOptionModel.entityType = entityType;

                    updateEntityAccessories();
                }

                dropDownModel.entityTypeDropDown = false;
            }

            function setEntityAccessory(entityAccessory) {

                if (userOptionModel.entityAccessory != entityAccessory) {

                    _.forEach(entityAccessory.ports, function (port) {
                        port.value = '0';
                    });

                    userOptionModel.entityAccessory = entityAccessory;
                }

                dropDownModel.entityAccessoryDropDown = false;
            }

            function updateEntityAccessories() {
                var entityType = userOptionModel.entityType;
                var entityAccessories = [];

                _.forEach(allEntityAccessories, function (entityAccessory) {
                    if (entityAccessory.entityTypes == '#any') {
                        entityAccessories.push(entityAccessory);
                    } else {
                        if (_.contains(entityAccessory.entityTypes, entityType.type)) {
                            entityAccessories.push(entityAccessory);
                        }
                    }
                });

                if (!_.contains(entityAccessories, userOptionModel.entityAccessory)) {
                    userOptionModel.entityAccessory = null;
                }

                $scope.entityAccessories = entityAccessories;
            }

            function getSolution() {

                $rootScope.data = {
                    entityTypeId: userOptionModel.entityType['id'],
                    entityAccessoryId: userOptionModel.entityAccessory['id'],
                    ports: (function () {

                        var ports = [];
                        _.forEach(userOptionModel.entityAccessory['ports'], function (port) {
                            ports.push({
                                id: port.id,
                                value: port.value
                            });
                        });

                        return ports;
                    })(),
                    dimension: userOptionModel.entityAccessory['ports'].length,
                    clustersNumber: clusteringOptions.clustersNumber,
                    clusteringQuantity: clusteringOptions.clusteringQuantity
                };

                $location.path('/master-result');
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

            function getEntityAccessoriesSelectTitle() {
                if ($scope.entityAccessories['length'] > 0) {
                    if (userOptionModel.entityAccessory) {
                        return userOptionModel.entityAccessory['name'];
                    }
                    return 'Не вибрано';
                }
                return 'Немає доступних варіантів';
            }

            function addProblematicSituation() {
                dialogsService.addProblematicSituation({
                    onAdd: function (entityType, entityModel, solution, closeCallback) {
                        closeCallback();
                    }
                });
            }

            $scope.dropDownModel = dropDownModel;
            $scope.userOptionModel = userOptionModel;

            $scope.entityTypes = [];
            $scope.entityAccessories = [];

            $scope.clusteringOptions = clusteringOptions;

            $scope.setEntityType = setEntityType;
            $scope.setEntityAccessory = setEntityAccessory;

            $scope.updateEntityAccessories = updateEntityAccessories;

            $scope.getSolution = getSolution;

            $scope.getEntityTypesSelectTitle = getEntityTypesSelectTitle;
            $scope.getEntityAccessoriesSelectTitle = getEntityAccessoriesSelectTitle;

            $scope.addProblematicSituation = addProblematicSituation;

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
                        entityAccessories: get$Q(function (resolve, reject) {
                            apiService.getEntityAccessories({
                                success: function (data) {

                                    function getPorts(data) {

                                        var ports = data.split('|');
                                        var result = [];

                                        _.forEach(ports, function (port) {
                                            port = port.split(',');
                                            result.push({
                                                id: port[0],
                                                name: port[1],
                                                value: '0'
                                            });
                                        });

                                        return result;
                                    }

                                    var entityAccessories = [];

                                    _.forEach(data, function (item) {
                                        if (item['EntityTypes'] == '#any') {
                                            entityAccessories.push({
                                                id: item['Id'],
                                                name: item['Name'],
                                                entityTypes: '#any',
                                                ports: getPorts(item['Ports'])
                                            });
                                        } else {
                                            entityAccessories.push({
                                                id: item['Id'],
                                                name: item['Name'],
                                                entityTypes: item['EntityTypes'].split(','),
                                                ports: getPorts(item['Ports'])
                                            });
                                        }
                                    });

                                    resolve(entityAccessories);
                                },
                                failure: function (error) {
                                    reject(error);
                                }
                            });
                        })
                    }).then(function (values) {

                        allEntityTypes = values.entityTypes;
                        allEntityAccessories = values.entityAccessories;

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
