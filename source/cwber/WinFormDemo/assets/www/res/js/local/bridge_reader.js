var use_bridge_tag = true;

window.onload = function () {
};
var bridge_reader = {
    ajax_callbacks: { index: 0 },
    ajax_errors:{index:0},
    open_by_com: function (param) {
        if (use_bridge_tag) {
            var res = window.CallCSharpMethod("open_by_com", JSON.stringify(param));
            if (res) {
                return JSON.parse(res);
            }
        }
    },
    close_by_com:function(){
        if (use_bridge_tag) {
            var res = window.CallCSharpMethod("close_by_com");
            if (res) {
                return JSON.parse(res);
            }
        }
    },
    open_by_tcp: function (param) {
        if (use_bridge_tag) {
            var res = window.CallCSharpMethod("open_by_tcp", JSON.stringify(param));
            if (res) {
                return JSON.parse(res);
            }
        }
    },
    close_by_tcp: function () {
        if (use_bridge_tag) {
            var res = window.CallCSharpMethod("close_by_tcp");
            if (res) {
                return JSON.parse(res);
            }
        }
    },
    beep_setting: function (param) {
        if (use_bridge_tag) {
            var res = window.CallCSharpMethod("beep_setting", JSON.stringify(param));
            if (res) {
                return JSON.parse(res);
            }
        }
    },
    start_inventory: function (param) {
        if (use_bridge_tag) {
            if (param.call_back && typeof param.call_back == 'function') {
                this.ajax_callbacks["_callback" + this.ajax_callbacks.index] = param.call_back;
               // console.info("_callback" + this.ajax_callbacks.index);
               // console.info(this.ajax_callbacks["_callback" + this.ajax_callbacks.index]);
                param.call_back = "bridge_reader.ajax_callbacks._callback" + this.ajax_callbacks.index;
                this.ajax_callbacks.index++;
            }
            if (param.error && typeof param.error == 'function')
            {
                this.ajax_errors["_error" + this.ajax_errors.index] = param.error;
                console.info("_error"+this.ajax_errors.index);
                param.error = "bridge_reader.ajax_errors._error" + this.ajax_errors.index;
                this.ajax_errors.index++;
            }
            var res = window.CallCSharpMethod("start_inventory", JSON.stringify(param));
            if (res) {
                return JSON.parse(res);
            }
        }
    },
    stop_inventory:function(param){
        if (use_bridge_tag) {
            param = {null:""};
            var res = window.CallCSharpMethod("stop_inventory", JSON.stringify(param));
            if (res) {
                return JSON.parse(res);
            }
        }
    },
    get_rwd_serial_no: function () {
        if (use_bridge_tag) {
            var res = window.CallCSharpMethod("get_rwd_serial_no");
            if (res) {
                return JSON.parse(res);
            }
        }
    },
    set_rwd_power:function(param){
    if (use_bridge_tag) {
        var res = window.CallCSharpMethod("set_rwd_power", JSON.stringify(param));
        if (res) {
            return JSON.parse(res);
            }
        }
    },
};