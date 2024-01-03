var reqParameterName = "AImq7w2TM6YIugpo6idvg=";

function ValidateTextInput(text, checkForEmpty) {
    if (checkForEmpty && (text == "" || text == null)) return false;

    if (text.toLowerCase().includes("javascript:") || text.toLowerCase().includes("<script>")) return false;

    return true;
}

function AntiForgeryTokenInclusionRequest(antiForgeryToken) {
    // Append AntiForgeryTiken Input to Form
    $('form').submit(function (event) {
        /*if ($(this).attr("method").toUpperCase() == "POST"
            && !$(this).find("[name=" + $(antiForgeryToken).attr("name") + "]").length) {*/
        if (!$(this).find("[name=" + $(antiForgeryToken).attr("name") + "]").length) {
            $(this).append($(antiForgeryToken));
        }
    });

    // Append AntiForgeryToken to Jquery Ajax Requests$.param()
    $(document).ajaxSend(function (event, request, opt) {
        var securityToken = $(antiForgeryToken).val();
        if (opt.hasContent && securityToken) {   // handle all verbs with content
            var dataParse = TryParseJSONObject(opt.data);
            if (dataParse) {
                var data = $.isArray(dataParse) ?
                    dataParse[0] : dataParse;
                if (!data.hasOwnProperty('__RequestVerificationToken')) {
                    var tokenParam = { __RequestVerificationToken: $(antiForgeryToken).val() };
                    $.extend(data, tokenParam);
                }
                opt.data = $.param(data);
                //opt.data = encodeURIComponent(String(EncryptedData(opt.data)));
            }
            else {
                if ((opt.data && opt.data.indexOf != undefined && opt.data.indexOf('__RequestVerificationToken') < 0)
                    || opt.data == "" || opt.data == undefined || opt.data == null) {
                    var tokenParam = "__RequestVerificationToken=" + encodeURIComponent(securityToken);
                    opt.data = opt.data ? [opt.data, tokenParam].join("&") : tokenParam;
                    //opt.data = encodeURIComponent(String(EncryptedData(opt.data)));
                } else if (opt.data.append != undefined) {
                    //form data
                    opt.data.append("__RequestVerificationToken", encodeURIComponent(securityToken))
                }
            }
            // ensure Content-Type header is present!
            if (opt.contentType !== false || event.contentType) {
                request.setRequestHeader("Content-Type", opt.contentType);
            }
        }
        else {
            var url = opt.url.split("?");
            if (url.length > 1) {
                opt.url = url[0] + EncodedQueryString(url[1]);// reqParameterName + encodeURIComponent(String(EncryptedData(url[1])));
            }
        }
    });
}

function TryParseJSONObject(jsonString) {
    try {
        var o = JSON.parse(jsonString);

        if (o && typeof o === "object") {
            return o;
        }
    }
    catch (e) { }

    return false;
}

function EncryptedData(data) {
    var key = CryptoJS.enc.Utf8.parse('8080808080808080');
    var iv = CryptoJS.enc.Utf8.parse('8080808080808080');
    return CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(data), key,
        { keySize: 128 / 8, iv: iv, mode: CryptoJS.mode.CBC, padding: CryptoJS.pad.Pkcs7 });
}

function EncodedQueryString(data) {
    return "?" + reqParameterName + EncryptedData(data);
    //return "?" + reqParameterName + encodeURIComponent(String(EncryptedData(data)));
}