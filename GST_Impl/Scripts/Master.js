///<reference path="gstapp.js"/>

app.controller("MasterController", ['$scope', '$http', 'NgTableParams', 'MasterService', '$filter', function ($scope, $http, NgTableParams, MasterService, $filter) {

    $scope.UMOInit = function () {
        $scope.myPromise = MasterService.UMOInit().then(function successCallBack(response) {
            $scope.umo = response.data.umo;
            $scope.UMOList = response.data.UMOList;
            $scope.UMOListTable = new NgTableParams({}, { dataset: $scope.UMOList });
        }, function errorCallBack(response) { $scope.Error();});
    };

    $scope.GetUMOById = function (Id) {
        $scope.myPromise = MasterService.GetUMOById(Id).then(function successCallBack(response) {
            $scope.umo = response.data;            
        }, function errorCallBack(response) { $scope.Error(); });
    };

    $scope.UMODelete = function (Id) {
        $scope.myPromise = MasterService.UMODelete(Id).then(function successCallBack(response) {
            if (response.data == "S") {
                alert("Delete Successfully");
                $scope.UMOInit();
            } else {
                $scope.Error();
            };
        }, function errorCallBack(response) { $scope.Error(); });
    };

    $scope.UMOSave = function () {
        $scope.myPromise = MasterService.UMOSave($scope.umo).then(function successCallBack(response) {
            if (response.data == "S") {
                alert("Save Successfully");
                $scope.UMOInit();
            } else {
                $scope.Error();
            };
        }, function errorCallBack(response) { $scope.Error(); });
    };

    //GST
    $scope.GSTInit = function () {
        $scope.myPromise = MasterService.GSTInit().then(function successCallBack(response) {
            $scope.gst = response.data.gst;
            $scope.GSTList = response.data.GSTList;
            $scope.GSTTypes = response.data.GSTTypes;
            $scope.GSTListTable = new NgTableParams({}, { dataset: $scope.GSTList });
        }, function errorCallBack(response) { $scope.Error(); });
    };

    $scope.GSTSave = function () {
        $scope.myPromise = MasterService.GSTSave($scope.gst).then(function successCallBack(response) {
            if (response.data == "S") {
                alert("Save Successfully");
                $scope.GSTInit();
            } else {
                $scope.Error();
            };
        }, function errorCallBack(response) { $scope.Error(); });
    };

    $scope.GSTDisable = function (Id) {
        $scope.myPromise = MasterService.GSTDisable(Id).then(function successCallBack(response) {
            if (response.data == "S") {
                alert("Save Successfully");
                $scope.GSTInit();
            } else {
                $scope.Error();
            };
        }, function errorCallBack(response) { $scope.Error(); });
    };

    //HSN

    $scope.HSNInit = function () {
        $scope.myPromise = MasterService.HSNInit().then(function successCallBack(response) {
            $scope.hsn = response.data.hsn;
            $scope.HSNList = response.data.HSNList;
            $scope.HSNListTable = new NgTableParams({}, { dataset: $scope.HSNList });
        }, function errorCallBack(response) { $scope.Error(); });
    };

    $scope.GetHSNById = function (Id) {
        $scope.myPromise = MasterService.GetHSNById(Id).then(function successCallBack(response) {
            $scope.hsn = response.data;
        }, function errorCallBack(response) { $scope.Error(); });
    };

    $scope.HSNDelete = function (Id) {
        $scope.myPromise = MasterService.HSNDelete(Id).then(function successCallBack(response) {
            if (response.data == "S") {
                alert("Delete Successfully");
                $scope.HSNInit();
            } else {
                $scope.Error();
            };
        }, function errorCallBack(response) { $scope.Error(); });
    };

    $scope.HSNSave = function () {
        $scope.myPromise = MasterService.HSNSave($scope.hsn).then(function successCallBack(response) {
            if (response.data == "S") {
                alert("Save Successfully");
                $scope.HSNInit();
            } else {
                $scope.Error();
            };
        }, function errorCallBack(response) { $scope.Error(); });
    };


    $scope.Error = function () {
        alert("Internal Server Error");
    };

}]);