'use strict';

angular.module('application')

    .service('userService', [

        'sessionManagerService',

        function (sessionManagerService) {

            var user = null;

            return {
                getData: function (handler) {

                    var successCallback = function (user) {
                        (handler.success || angular.noop)(user);
                    };

                    var failureCallback = function (error) {
                        (handler.failure || angular.noop)(error);
                    };

                    sessionManagerService.isAuthenticated({
                        success: function (response) {
                            if (response.isAuthenticated) {
                                if (user) {
                                    successCallback(user);
                                } else {
                                    sessionManagerService.getUserData({
                                        success: function (user) {
                                            successCallback(user);
                                        },
                                        failure: function (error) {
                                            failureCallback(error);
                                        }
                                    });
                                }
                            } else {
                                failureCallback("Ви не авторизовані");
                            }
                        },
                        failure: function (error) {
                            failureCallback(error);
                        }
                    });
                },
                logout: function (callback) {
                    sessionManagerService.logout({
                        success: function () {
                            user = null;
                            callback();
                        },
                        failure: function () {
                            user = null;
                            callback();
                        }
                    });
                }
            };
        }
    ]
);
