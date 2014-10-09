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
                getSolution: function (entityTypeId, entityModelId, problemId, handler) {
                    httpClientService.sendRequest({
                        method: 'POST',
                        data: {
                            action: 'find_solution',
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
                addSolution: function (entityTypeId, entityModelId, problemId, solution, handler) {
                    httpClientService.sendRequest({
                        method: 'POST',
                        data: {
                            action: 'add_solution',
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
                }
            };
        }
    ]
);