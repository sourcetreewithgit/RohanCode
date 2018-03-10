///<reference path="gstapp.js"/>

app.controller("ClientController",['$scope', '$http', 'NgTableParams', 'ClientService', '$filter', function ($scope, $http, NgTableParams, ClientService, $filter) {

    $scope.Init = function(){
    $scope.myPromise = ClientService.Init().then(function successCallBack(response) {
        $scope.customer = response.data.customer;  
        $scope.CustomerList = response.data.CustomerList;

        $scope.CustomerListTable = new NgTableParams({}, { dataset: $scope.CustomerList });

        $scope.customer.CustomerContacts = [];
        $scope.customer.DeletedCustomerContactDetails = [];
        $scope.NewRow();
    },function errorCallBack(response){});
    };

    $scope.Edit = function (_param) {
        $scope.myPromise = ClientService.Edit(_param).then(function successCallBack(response) {
            $scope.customer = response.data;           
            $scope.customer.DeletedCustomerContactDetails = [];
        }, function errorCallBack(response) { });
    };

    $scope.Save = function () {
    $scope.myPromise =  ClientService.Save($scope.customer).then(function successCallBack(response) {
            if (response.data == "S") {
                alert('Customer deatils submitted successfully!');
                $scope.Init();
            } else {
                alert('Oops!! An error occured while submitting customer details!');
            }
            }, function errorCallBack(response) { $scope.Error(); });
    }

    $scope.Error = function () {
        alert("Internal Server Error");
    };

    $scope.NewRow = function () {
        $scope.customer.CustomerContacts.push({ CustomerId: $scope.customer.CustomerId, CrudStatus: "I"});
    };
    $scope.DeleteRow = function () {       
        if ($scope.customer.CustomerContacts.length > 1) {
            if ($scope.customer.CustomerContacts[this.$index].CrudStatus == "U") {
                $scope.customer.DeletedCustomerContactDetails.push($scope.customer.CustomerContacts[this.$index]);
            }
            $scope.customer.CustomerContacts.splice(this.$index, 1);
           
        }
    };

}]);
