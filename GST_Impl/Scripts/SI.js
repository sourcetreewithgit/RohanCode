///<reference path="gstapp.js"/>
app.controller("SIController", ['$scope', '$http', 'NgTableParams', 'POService', 'ClientService', '$filter', 'SIService', function ($scope, $http, NgTableParams, POService, ClientService, $filter, SIService) {

    $scope.GetCustomers = function () {
       
        $scope.myPromise =  ClientService.GetClientList().then(function successCallBack(response) {
                $scope.Customers = response.data;
            }, function errorCallBack(response) { $scope.Error(); });
        
    }


    $scope.Check = function ()
    {
        $scope.myPromise = SIService.Check().then(function successCallBack(response) {
            $scope.Customers = response.data.customers;
            $scope.GUID = response.data.GUID;
            if ($scope.GUID == null || $scope.GUID == "") {
                $scope.GetCustomers(); 
            }
            else {
                $scope.Edit($scope.GUID);
            }
        }, function errorCallBack(response) { $scope.Error(); });
    }



    $scope.GetCustomerPOList = function (_param) {
    
        $scope.myPromise =   POService.GetCustomerPOList(_param).then(function successCallBack(response) {
                $scope.PoList = response.data;
            }, function errorCallBack(response) { $scope.Error(); });
      
    } 
    $scope.GetSIList = function () {
        $scope.myPromise = SIService.GetSIList().then(function successCallBack(response) {
            $scope.SI = response.data;
            $scope.SIListTable = new NgTableParams({}, { dataset: $scope.SI });
        }, function errorCallBack(response) { $scope.Error(); });
    }
    $scope.GetPO_InviceDetails = function (_param) { 
        $scope.myPromise = SIService.GetPO_InviceDetails(_param).then(function successCallBack(response) {
            if (response.data.alert!="") {
                alert(response.data.alert);
                window.location.href = "/SalesInvoice/Index";
            } else { 
            $scope.SI = response.data.SalesInvoice;
            $scope.SI.CustomerId = $scope.SI.CustomerId.toString();
            $scope.SI.PoId = $scope.SI.PoId.toString();
            $scope.SI.SelectedPoId = $scope.SI.SelectedPoId.toString();  
            $scope.SI.DeletedSIDetailsList = [];
            $scope.SI.ShippedTo =$scope.SI.BilledTo;
            }
        }, function errorCallBack(response) { $scope.Error(); });

    }  

    $scope.Edit = function (_param) {
        $scope.myPromise = SIService.Edit(_param).then(function successCallBack(response) { 
            $scope.SI = response.data.SalesInvoice;
            $scope.PoList = response.data.PoList;
                $scope.SI.CustomerId = $scope.SI.CustomerId.toString();
                $scope.SI.PoId = $scope.SI.PoId.toString();
                $scope.SI.SelectedPoId = $scope.SI.SelectedPoId.toString();
                $scope.SI.DeletedSIDetailsList = [];
                $scope.SI.ShippedTo = $scope.SI.BilledTo;
                if (response.data.remainingItems != null && response.data.remainingItems.length>0) {
                    for (var i = 0; i < response.data.remainingItems.length; i++) {
                        $scope.SI.SalesInvoiceItems.push(response.data.remainingItems[i]);
                    }
                }
                $scope.NumToWord($scope.SI.GrandTotal);
        }, function errorCallBack(response) { $scope.Error(); });

    }  



    $scope.Save = function () { 
        if (confirm("Are you sure to save the record?") == true) {
            if ($scope.SI.SalesInvoiceItems != undefined) {
                var count = _.filter($scope.SI.SalesInvoiceItems, function (c) { return c.Check == true });
                if (count == undefined || count.length==0) {
                    alert("Kindly select the sales invoice line items to include."); return false;
                }
                else {
                    $scope.SI.SalesInvoiceItems = count;
                } 
       
                $scope.myPromise =SIService.Save($scope.SI).then(function successCallBack(response) { 
                    alert("Record saved successfully!!!")
                    window.location.href = "/SalesInvoice/Index";
        }, function errorCallBack(response) { $scope.Error(); });   
            }
        }
    }
    $scope.calTotal  = function ()
    {
        if ($scope.SI.SalesInvoiceItems[this.$index].Check == false && $scope.SI.SalesInvoiceItems[this.$index].CrudStatus=="U") {
            $scope.SI.DeletedSIDetailsList.push($scope.SI.SalesInvoiceItems[this.$index].Id); 
            $scope.SI.SalesInvoiceItems.splice(this.$index, 1);
        } 
        $scope.SI.GrandTotal = 0;
        for (var i = 0; i < $scope.SI.SalesInvoiceItems.length; i++) {
            if ($scope.SI.SalesInvoiceItems[i].Check == false) {
                $scope.SI.SalesInvoiceItems[i].ItemTotal = 0;
            }
        } 
        var rows = _.filter($scope.SI.SalesInvoiceItems, function (c) { return c.Check == true });
        for (var i = 0; i < rows.length; i++) {
            $scope.calRow(rows[i]);
        }
      
        $scope.SI.GrandTotal = Math.round(_.sumBy($scope.SI.SalesInvoiceItems, 'ItemTotal'));
        $scope.SI.Inwords = $scope.NumToWord($scope.SI.GrandTotal);
        $scope.SI.BasicAmtTot = Math.round(_.sumBy($scope.SI.SalesInvoiceItems, 'BasicAmt')); 
        $scope.SI.IGSTorCGST_AmtTot = Math.round(_.sumBy($scope.SI.SalesInvoiceItems, 'IGSTorCGST_Amt'));
        $scope.SI.SGSTorUGST_AmtTot = Math.round(_.sumBy($scope.SI.SalesInvoiceItems, 'SGSTorUGST_Amt'));
        $scope.SI.OtherTax_AmtTot = Math.round(_.sumBy($scope.SI.SalesInvoiceItems, 'OtherTax_Amt'));

    }
     
    $scope.calRow = function (row) { 
        if (row != undefined) {
            if (isNaN(row.Quantity) || row.Quantity == undefined || row.Quantity <= 0 || row.Quantity == null) {
                row.Quantity = 1;
            }
            else {
                if (row.Quantity > row.PO_Qty) {
                    row.Quantity = row.PO_Qty;
                }
                row.Quantity = Math.round(row.Quantity * 100) / 100;
            }
            
            row.ItemAdvance = Math.round((row.UnitAdvance * row.Quantity) * 100) / 100;

            row.IGSTorCGST_Amt = Math.round((row.unitIGSTorCGST_Amt * row.Quantity) * 100) / 100;
            row.SGSTorUGST_Amt = Math.round((row.unitSGSTorUGST_Amt * row.Quantity) * 100) / 100;
            row.OtherTax_Amt = Math.round((row.unitOtherTax_Amt * row.Quantity) * 100) / 100; 
            row.BasicAmt = row.Quantity * row.BasicRate;
            row.BasicAmt = Math.round(row.BasicAmt * 100) / 100;
        }
        row.ItemTotal = Math.round(row.BasicAmt + row.IGSTorCGST_Amt + row.SGSTorUGST_Amt + row.OtherTax_Amt - row.ItemAdvance);
        if (isNaN(row.ItemTotal)) {
            row.ItemTotal = 0;
        }
        
    }

    $scope.NumToWord = function (num)
    {

        var a = ['', 'one ', 'two ', 'three ', 'four ', 'five ', 'six ', 'seven ', 'eight ', 'nine ', 'ten ', 'eleven ', 'twelve ', 'thirteen ', 'fourteen ', 'fifteen ', 'sixteen ', 'seventeen ', 'eighteen ', 'nineteen '];
        var b = ['', '', 'twenty', 'thirty', 'forty', 'fifty', 'sixty', 'seventy', 'eighty', 'ninety'];
          
            //if ((num = num.toString()).length > 9) return 'overflow';
           var n = ('000000000' + num).substr(-9).match(/^(\d{2})(\d{2})(\d{2})(\d{1})(\d{2})$/);
           if (!n) return;
           var str = '';
            str += (n[1] != 0) ? (a[Number(n[1])] || b[n[1][0]] + ' ' + a[n[1][1]]) + 'crore ' : '';
            str += (n[2] != 0) ? (a[Number(n[2])] || b[n[2][0]] + ' ' + a[n[2][1]]) + 'lakh ' : '';
            str += (n[3] != 0) ? (a[Number(n[3])] || b[n[3][0]] + ' ' + a[n[3][1]]) + 'thousand ' : '';
            str += (n[4] != 0) ? (a[Number(n[4])] || b[n[4][0]] + ' ' + a[n[4][1]]) + 'hundred ' : '';
            str += (n[5] != 0) ? ((str != '') ? 'and ' : '') + (a[Number(n[5])] || b[n[5][0]] + ' ' + a[n[5][1]]) + 'only ' : '';
            return str;
      
    }



}]);