var controllers = angular.module('tipControllers', []);

controllers.controller('ListContactCtlr', ['$scope', '$q', 'Contact',
    function ($scope, $q, Contact) {
        $scope.list = [];

        // these are delegates methods for the dialog
        $scope.confirmContact = null;
        $scope.cancelContact = null;
        // this is the data for the dialog
        $scope.user = null;

        $scope.addContact = function () {

            $("#addContactModal").modal({ keyboard: false, show: true, backdrop: 'static' });
            
            getSingleContactAndShowModal().then(
                // resolve
                function (user) {
                    $("#addContactModal").modal('hide');
                    $scope.list.push(user);
                    //alert('accepted');
                },
                // reject
                function (user) {
                    $("#addContactModal").modal('hide');
                    //alert('rejected');
                });
        };

        // this wraps the data call and shows the modal as a promise
        var getSingleContactAndShowModal = function () {
            // this allows this method to defer itself
            var deferred = $q.defer();

            Contact.single().$promise.then(function (data) {
                // gets the result from the resource call
                var results = data.results;

                // save the user
                var user = results[0].user;

                // scope the modal data
                $scope.user = user;

                // here is the key to how this works
                // this creates a delegate function which
                // has the deferred object scoped data
                // stored inside this method
                $scope.confirmContact = function () {

                    // confirm button pushed

                    // clear out single user data
                    $scope.user = null;

                    // resolve the deferred object
                    deferred.resolve(user);

                    // remove delegates
                    $scope.confirmContact = null;
                    $scope.cancelContact = null;

                };

                // same idea here except we reject the deferred object
                $scope.cancelContact = function () {
                    // cancel button pushed

                    // clear out single user data
                    $scope.user = null;

                    // reject the deferred object
                    deferred.reject(user);

                    // remove delegates
                    $scope.confirmContact = null;
                    $scope.cancelContact = null;
                };
            });


            return deferred.promise;
        };

        $scope.getList = function()
        {
            Contact.query().$promise.then(function (data) {
                var results = data.results;

                var list = [];

                angular.forEach(results, function (value, key) {
                    list.push(value.user);
                });

                $scope.list = list;
            });
        }

        $scope.getList();
    }
]);