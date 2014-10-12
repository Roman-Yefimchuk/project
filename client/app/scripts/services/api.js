"use strict";

angular.module('application')

    .service('apiService', [

        '$cookies',
        '$http',
        'httpClientService',
        'API_SERVER_URL',

        function ($cookies, $http, httpClientService, API_SERVER_URL) {
            return {
                login: function (data, handler) {
                    httpClientService.sendRequest({
                        method: 'POST',
                        data: data,
                        url: API_SERVER_URL + '/api/login/'
                    }, handler);
                },
                signUp: function (data, handler) {
                    httpClientService.sendRequest({
                        method: 'POST',
                        data: data,
                        url: API_SERVER_URL + '/api/sign-up/'
                    }, handler);
                },
                getEntityTypes: function (handler) {
                    httpClientService.sendRequest({
                        method: 'POST',
                        data: {
                            action: 'find_all_entity_types',
                            token: $cookies.token || 'empty_token'
                        },
                        url: API_SERVER_URL + '/api/database/'
                    }, handler);
                },
                getEntityTypeById: function (id, handler) {
                    httpClientService.sendRequest({
                        method: 'POST',
                        data: {
                            action: 'find_entity_type_by_id',
                            token: $cookies.token || 'empty_token',
                            data: {
                                id: id
                            }
                        },
                        url: API_SERVER_URL + '/api/database/'
                    }, handler);
                },
                getEntityModels: function (handler) {
                    httpClientService.sendRequest({
                        method: 'POST',
                        data: {
                            action: 'find_all_entity_models',
                            token: $cookies.token || 'empty_token'
                        },
                        url: API_SERVER_URL + '/api/database/'
                    }, handler);
                },
                getEntityModelById: function (id, handler) {
                    httpClientService.sendRequest({
                        method: 'POST',
                        data: {
                            action: 'find_entity_model_by_id',
                            token: $cookies.token || 'empty_token',
                            data: {
                                id: id
                            }
                        },
                        url: API_SERVER_URL + '/api/database/'
                    }, handler);
                },
                getEntityAccessories: function (handler) {
                    httpClientService.sendRequest({
                        method: 'POST',
                        data: {
                            action: 'find_all_accessories',
                            token: $cookies.token || 'empty_token'
                        },
                        url: API_SERVER_URL + '/api/database/'
                    }, handler);
                },
                getEntityAccessoryById: function (id, handler) {
                    httpClientService.sendRequest({
                        method: 'POST',
                        data: {
                            action: 'find_accessory_by_id',
                            token: $cookies.token || 'empty_token',
                            data: {
                                id: id
                            }
                        },
                        url: API_SERVER_URL + '/api/database/'
                    }, handler);
                },
                getProblems: function (handler) {
                    httpClientService.sendRequest({
                        method: 'POST',
                        data: {
                            action: 'find_all_problems',
                            token: $cookies.token || 'empty_token'
                        },
                        url: API_SERVER_URL + '/api/database/'
                    }, handler);
                },
                getProblemById: function (id, handler) {
                    httpClientService.sendRequest({
                        method: 'POST',
                        data: {
                            action: 'find_problem_by_id',
                            token: $cookies.token || 'empty_token',
                            data: {
                                id: id
                            }
                        },
                        url: API_SERVER_URL + '/api/database/'
                    }, handler);
                },
                getUserSolution: function (entityTypeId, entityModelId, problemId, handler) {
                    httpClientService.sendRequest({
                        method: 'POST',
                        data: {
                            action: 'find_user_solution',
                            token: $cookies.token || 'empty_token',
                            data: {
                                entityTypeId: entityTypeId,
                                entityModelId: entityModelId,
                                problemId: problemId
                            }
                        },
                        url: API_SERVER_URL + '/api/database/'
                    }, handler);
                },
                addUserSolution: function (entityTypeId, entityModelId, problemId, solution, handler) {
                    httpClientService.sendRequest({
                        method: 'POST',
                        data: {
                            action: 'add_user_solution',
                            token: $cookies.token || 'empty_token',
                            data: {
                                entityTypeId: entityTypeId,
                                entityModelId: entityModelId,
                                problemId: problemId,
                                solution: solution
                            }
                        },
                        url: API_SERVER_URL + '/api/database/'
                    }, handler);
                },
                getMasterSolution: function (data, handler) {
                    httpClientService.sendRequest({
                        method: 'POST',
                        data: {
                            action: 'find_master_solution',
                            token: $cookies.token || 'empty_token',
                            data: data
                        },
                        url: API_SERVER_URL + '/api/database/'
                    }, handler);
                },
                addMasterSolution: function (entityTypeId, entityAccessoryId, ports, problem, solution, handler) {
                    httpClientService.sendRequest({
                        method: 'POST',
                        data: {
                            action: 'add_master_solution',
                            token: $cookies.token || 'empty_token',
                            data: {
                                entityTypeId: entityTypeId,
                                entityAccessoryId: entityAccessoryId,
                                ports: (function () {
                                    var result = "";

                                    _.forEach(ports, function (port) {
                                        if (result.length == 0) {
                                            result += port.id + '=' + port.value;
                                        } else {
                                            result += ',' + port.id + '=' + port.value;
                                        }
                                    });

                                    return result;
                                })(),
                                problem: problem,
                                solution: solution
                            }
                        },
                        url: API_SERVER_URL + '/api/database/'
                    }, handler);
                }
            };
        }
    ]
);