///<reference path="gstapp.js"/>

app.controller("POController", ['$scope', '$http', 'NgTableParams', 'POService', '$filter',function ($scope, $http, NgTableParams, POService, $filter) {

    $scope.Init = function () {
        $scope.myPromise = POService.Init().then(function successCallBack(response) {

            $scope.PO = response.data.PO;           
            $scope.IsAmd = response.data.IsAmd;
            $scope.TaxList = response.data.TaxList;
            $scope.Uoms = response.data.Uoms;
            $scope.Customers = response.data.Customers;
            $scope.HsnCodes = response.data.HsnCodes;
            $scope.PO.DeletedPODetails = [];
            if ($scope.PO.CrudStatus == "I" && $scope.IsAmd=="N") {
               
                $scope.PO.PurchaseOrderDetails = [];

                $scope.PO.PODate = $filter('date')(new Date(), 'dd/MMM/yyyy');
                $scope.PO.ExpDeliveryDate = $filter('date')(new Date(), 'dd/MMM/yyyy');
               
                $scope.PO.GrandTotal = 0.0;
                $scope.NewRow();
            } else {
                $scope.PO.PODate = $filter('date')(new Date($scope.PO.PODate), 'dd/MMM/yyyy');
                $scope.PO.ExpDeliveryDate = $filter('date')(new Date($scope.PO.ExpDeliveryDate), 'dd/MMM/yyyy');
                $scope.PO.CustomerId = $scope.PO.CustomerId.toString();
            }
          
            
            if ($scope.PO.PurchaseOrderDetails != null&& $scope.PO.PurchaseOrderDetails.length>0) {
                for (var i = 0; i < $scope.PO.PurchaseOrderDetails.length; i++) {
                    if ($scope.PO.PurchaseOrderDetails[i].IGSTorCGST != null) {
                        $scope.PO.PurchaseOrderDetails[i].IGSTorCGST = $scope.PO.PurchaseOrderDetails[i].IGSTorCGST.toString();
                    }
                    if ($scope.PO.PurchaseOrderDetails[i].SGSTorUGST != null) {
                        $scope.PO.PurchaseOrderDetails[i].SGSTorUGST = $scope.PO.PurchaseOrderDetails[i].SGSTorUGST.toString();
                    }
                    if ($scope.PO.PurchaseOrderDetails[i].OtherTax != null) {
                        $scope.PO.PurchaseOrderDetails[i].OtherTax = $scope.PO.PurchaseOrderDetails[i].OtherTax.toString();
                    } 
                    if ($scope.PO.PurchaseOrderDetails[i].UomId != null) {
                        $scope.PO.PurchaseOrderDetails[i].UomId = $scope.PO.PurchaseOrderDetails[i].UomId.toString();
                    } 
                    if ($scope.PO.PurchaseOrderDetails[i].HsnSacCode != null) {
                        $scope.PO.PurchaseOrderDetails[i].HsnSacCode = $scope.PO.PurchaseOrderDetails[i].HsnSacCode.toString();
                    } 
                    
                }
            }
        }, function errorCallBack(response) { $scope.Error(); });
    }

    $scope.NewRow = function () {
        $scope.PO.PurchaseOrderDetails.push
            ({
                PO_ID: $scope.PO.PO_ID, CrudStatus: "I", BasicRate: 0.0, Quantity: 1, BasicAmt: 0.0,
                Total: 0.0, IGSTorCGST: '1', SGSTorUGST: '2', OtherTax: '3', IGSTorCGST_Amt: 0.0, SGSTorUGST_Amt: 0.0, OtherTax_Amt:0.0
            });
    } 
    $scope.DeleteRow = function () {       
        if ($scope.PO.PurchaseOrderDetails.length > 1) {
            if ($scope.PO.PurchaseOrderDetails[this.$index].CrudStatus == "U") {
                $scope.PO.DeletedPODetails.push($scope.PO.PurchaseOrderDetails[this.$index]);
            }
            $scope.PO.PurchaseOrderDetails.splice(this.$index, 1);
            $scope.calTotal(0);
        }
    }

    $scope.Save = function (status) {
        if (confirm("Are you sure to save PO details.")==true) {
            if ($scope.PO.PurchaseOrderDetails != null && $scope.PO.PurchaseOrderDetails.length>0) {
                for (var i = 0; i < $scope.PO.PurchaseOrderDetails.length; i++) {
                    $scope.calTotal(i);
                }
            }
       
            POService.Save($scope.PO, status).then(function successCallBack(response) {
            if (response.data == "S") {
                alert('Purchase order submitted successfully!');
                window.location.href = "/PO/Index";
            } else {
                alert('Oops!! An error occured while submitting purchase order!');
            }
            }, function errorCallBack(response) { $scope.Error(); });
        }
    }

    $scope.GetPOList = function () {
        $scope.myPromise = POService.GetPOList().then(function successCallBack(response) {
            $scope.PO = response.data;
            $scope.POListTable = new NgTableParams({}, { dataset: $scope.PO });
        }, function errorCallBack(response) { $scope.Error(); });
    }

    $scope.Error = function () {
        alert("Internal Server Error");
    }

    $scope.calTotal = function (ind)
    {
        if (ind == undefined) {
            ind = this.$index; 
        } 
        var row = $scope.PO.PurchaseOrderDetails[ind];
        if (row != undefined) {
            if (isNaN(row.Quantity)|| row.Quantity == undefined || row.Quantity <= 0 || row.Quantity == null) {
                row.Quantity = 1;
            }
            else {
                row.Quantity = Math.round(row.Quantity*100)/100;
            }
            if (isNaN(row.BasicRate)||row.BasicRate == undefined || row.BasicRate < 0 || row.BasicRate == null) {
                row.BasicRate = 0.0;
            }
            else {
                row.BasicRate = Math.round(row.BasicRate * 100) / 100;
                //Math.round(num * 100) / 100
            }
            row.BasicAmt = row.Quantity * row.BasicRate;
            row.BasicAmt = Math.round(row.BasicAmt * 100) / 100

            if (isNaN(row.Advance) || row.Advance == undefined || row.Advance < 0 || row.Advance == null) {
                row.Advance = 0.0;
            }
            else {
                row.Advance = Math.round(row.Advance * 100) / 100;
            }
        }
        var T1 = _.find($scope.TaxList, function (c) {return c.TaxId == row.IGSTorCGST });
        row.IGSTorCGST_Amt = Math.round((row.BasicAmt * (T1.Percentage / 100))*100)/100;
        var T2 = _.find($scope.TaxList, function (c) { return c.TaxId == row.SGSTorUGST });
        row.SGSTorUGST_Amt = Math.round(row.BasicAmt * (T2.Percentage / 100) * 100) / 100;
        var T3 = _.find($scope.TaxList, function (c) { return c.TaxId == row.OtherTax });
        row.OtherTax_Amt = Math.round(row.BasicAmt * (T3.Percentage / 100) * 100) / 100;
        row.Total = Math.round(row.BasicAmt + row.IGSTorCGST_Amt + row.SGSTorUGST_Amt + row.OtherTax_Amt);
      
         
        $scope.PO.GrandTotal = Math.round(_.sumBy($scope.PO.PurchaseOrderDetails, 'Total'));
        $scope.PO.TotalAdvance = Math.round(_.sumBy($scope.PO.PurchaseOrderDetails, 'ItemAdvance'));

        
    }

   
    

}]);

app.controller("DisplayTemplatesCtrl", ['$scope', '$http', 'NgTableParams', 'POService', '$filter', function ($scope, $http, NgTableParams, POService, $filter) {
    {

        $scope.DisplayPO = function () {
            $scope.myPromise = POService.DisplayPO().then(function successCallBack(response) {
                $scope.PO = response.data;
            }, function errorCallBack(response) { $scope.Error(); });
        }
    }
}]);