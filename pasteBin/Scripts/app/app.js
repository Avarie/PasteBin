// Main configuration file. Sets up AngularJS module and routes and any other config objects

var appRoot = angular.module('main', ['ngRoute',
    'ngGrid',
    'ngResource',
    'textAngular'
]);     //Define the main module

appRoot
    .config(['$routeProvider', function ($routeProvider) {
        $routeProvider
            .when('/', { templateUrl: '/copy/main', controller: 'MainController' })
            .otherwise({ redirectTo: '/' });
    }])
    .controller('RootController',
    ['$scope', '$route', '$routeParams', '$location',
        function ($scope, $route, $routeParams, $location) {
        $scope.$on('$routeChangeSuccess', function (e, current, previous) {
            $scope.activeViewPath = $location.path();
        });
    }]);

