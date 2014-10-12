'use strict';

angular.module('application')

    .service('dialogsService', [

        '$modal',

        function ($modal) {

            function open(modalOptions) {
                return $modal.open(modalOptions);
            }

            return {
                showConfirmation: function (options) {
                    return open({
                        templateUrl: '/app/views/controllers/dialogs/confirmation-dialog-view.html',
                        controller: 'ConfirmationDialogController',
                        resolve: {
                            options: function () {
                                return options;
                            }
                        }
                    });
                },
                showAlert: function (options) {
                    return open({
                        templateUrl: '/app/views/controllers/dialogs/alert-dialog-view.html',
                        controller: 'AlertDialogController',
                        resolve: {
                            options: function () {
                                return options;
                            }
                        }
                    });
                },
                addProblematicSituation: function (options) {
                    return open({
                        templateUrl: '/app/views/controllers/dialogs/problematic-situation-dialog-view.html',
                        controller: 'ProblematicSituationDialogController',
                        resolve: {
                            options: function () {
                                return options;
                            }
                        }
                    });
                }
            };
        }
    ]
);