"use strict";

angular.module('application')

    .controller('IndexController', [

        '$scope',
        'loaderService',

        function ($scope, loaderService) {
            loaderService.hideLoader();
        }
    ]
);
