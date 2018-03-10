var app = angular.module('GSTApp', ['ngTable', 'ui.select', '720kb.datepicker', 'cgBusy']);
var _methods = ["POST","GET","DELETE","PUT"];

app.service('POService', ['$http', function ($http) {
    var self = this;

    self.Init = function () {
        return $http({ method: _methods[0], url: '/PO/Init' });
    };

    self.Save = function (_param, status) {
        return $http({ method: _methods[0], url: '/PO/Save', data: { _paramPO: _param, status: status } });
    }

    self.GetPOList = function () {
        return $http({ method: _methods[0], url: '/PO/GetPOList' });
    }

    self.DisplayPO = function () {
        return $http({ method: _methods[0], url: '/DisplayTemplates/DisplayPO'});
    }
    self.GetCustomerPOList = function (_param) {
        return $http({ method: _methods[0], url: '/PO/GetCustomerPOList',data:{_param: _param} });
    }
    
}]);

app.service('SIService', ['$http', function ($http) {
    var self = this;
    //self.Init = function () {
    //    return $http({ method: _methods[0], url: '/PO/Init' });
    //}; 
    self.GetPO_InviceDetails = function (_param) {
        return $http({ method: _methods[0], url: '/SalesInvoice/GetPO_InviceDetails', data: { _param: _param} });
    } 
    self.Save = function (_param) {
        return $http({ method: _methods[0], url: '/SalesInvoice/Save', data: { _param: _param } });
    }
    self.Edit = function (_param) {
        return $http({ method: _methods[0], url: '/SalesInvoice/Edit', data: { _param: _param } });
    }
    self.GetSIList = function () {
        return $http({ method: _methods[0], url: '/SalesInvoice/GetSIList' });
    }
    self.Check = function () {
        return $http({ method: _methods[0], url: '/SalesInvoice/Check' });
    }
    
}]);

app.service('ClientService', ['$http', function ($http) {
    var self = this;
    
    self.Init = function () {
        return $http({ method: _methods[0], url: '/Client/Init' });
    };

    self.Save = function (_param) {
        return $http({ method: _methods[0], url: '/Client/Save', data: { _paramCustomer: _param } });
    }

    self.Edit = function (_param) {
        return $http({ method: _methods[0], url: '/Client/Edit', data: { _pGUID: _param } });
    }

    self.GetClientList = function () {
        return $http({ method: _methods[0], url: '/Client/GetClientList' });
    }

}]);

app.service('MessageService', ['$http', function ($http) {
    var self = this; 
    self.ShowMsgfor = function (_code) {
        var msg = "";
        switch (_code) {
            //Success codes
            case "SS": msg = "Record saved successfully!"; break;
            case "SS_POD": msg = "PO saved as draft!"; break;
            case "SS_POS": msg = "PO submitted successfully!"; break;
                //Error codes
            case "ERR": msg = "Error occured while processing your request!"; break; 
            case "ERR_DR_INV": msg = "Draft for PO ammendment cannot be submitted as original request is partially processed!"; break; 
            
                //Warning codes
            case "ERR_URL_TMP": msg = "Invalid request found!"; break;
            default:""
        }
        return "";
    };

    
}]);

app.service('MasterService', ['$http', function ($http) {
    var self = this;

    self.UMOInit = function () {
        return $http({ method: _methods[0], url: '/Master/UMOInit' });
    };

    self.UMOSave = function (_param) {
        return $http({ method: _methods[0], url: '/Master/UMOSave', data: { _pUmo: _param } });
    };

    self.GetUMOById = function (_param) {
        return $http({ method: _methods[0], url: '/Master/GetUMOById', data: { _pUomId: _param } });
    };

    self.UMODelete = function (_param) {
        return $http({ method: _methods[0], url: '/Master/UMODelete', data: { _pUomId: _param } });
    };

    self.GSTInit = function () {
        return $http({ method: _methods[0], url: '/Master/GSTInit' });
    };

    self.GSTSave = function (_param) {
        return $http({ method: _methods[0], url: '/Master/GSTSave', data: { _pgst: _param } });
    };

    self.GSTDisable = function (_param) {
        return $http({ method: _methods[0], url: '/Master/GSTDisable', data: { _pTaxId: _param } });
    };

    self.HSNInit = function () {
        return $http({ method: _methods[0], url: '/Master/HSNInit' });
    };

    self.HSNSave = function (_param) {
        return $http({ method: _methods[0], url: '/Master/HSNSave', data: { _phsn: _param } });
    };

    self.GetHSNById = function (_param) {
        return $http({ method: _methods[0], url: '/Master/GetHSNById', data: { _pHsnHacCodeId: _param } });
    };

    self.HSNDelete = function (_param) {
        return $http({ method: _methods[0], url: '/Master/HSNDelete', data: { _pHsnHacCodeId: _param } });
    };

}]);