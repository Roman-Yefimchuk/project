"use strict";

angular.module('application', [

    'ui.bootstrap',
    'ngRoute',
    'ngSanitize',
    'ngCookies'

])
    .config([

        '$routeProvider',
        '$locationProvider',
        '$tooltipProvider',

        function ($routeProvider, $locationProvider, $tooltipProvider) {

            $tooltipProvider.options({
                placement: 'top',
                animation: true,
                popupDelay: 500,
                appendToBody: true
            });

            $routeProvider.when('/', {
                templateUrl: '/app/views/controllers/index-view.html',
                controller: 'IndexController',
                options: {
                    title: 'Головна'
                }
            }).when('/login', {
                templateUrl: '/app/views/controllers/login-view.html',
                controller: 'LoginController',
                options: {
                    title: 'Увійти'
                }
            }).when('/input', {
                templateUrl: '/app/views/controllers/input-view.html',
                controller: 'InputController',
                options: {
                    title: 'Введення даних'
                }
            }).when('/result', {
                templateUrl: '/app/views/controllers/result-view.html',
                controller: 'ResultController',
                options: {
                    title: 'Результат'
                }
            }).when('/sign-up', {
                templateUrl: '/app/views/controllers/sign-up-view.html',
                controller: 'SignUpController',
                options: {
                    title: 'Реєстрація'
                }
            }).when('/logout', {
                template: '',
                controller: 'LogoutController'
            }).otherwise({
                templateUrl: '/app/views/controllers/page-not-found-view.html',
                controller: 'NotFoundController',
                options: {
                    title: 'Сторінка не знайдена'
                }
            });
        }
    ])

    .run([

        '$rootScope',
        '$document',

        function ($rootScope, $document) {

            $rootScope.$on('$routeChangeSuccess', function (event, currentRoute, prevRoute) {
                var options = currentRoute['options'];
                if (options) {
                    $document.attr('title', options.title);
                }
            });
        }
    ]);