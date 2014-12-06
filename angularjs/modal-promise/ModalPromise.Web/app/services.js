var services = angular.module('tipServices', ['ngResource']);

services.factory('Contact', ['$resource',
    function ($resource) {
        return $resource('http://api.randomuser.me/', {}, {
            query: { method: 'GET', params: { results: 10 }, isArray: false },
            single: { method: 'GET', params: { results: 1 }, isArray: false }
        });
    }
]);