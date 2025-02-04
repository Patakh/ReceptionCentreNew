﻿/*!
 * ASP.NET SignalR JavaScript Library v2.2.2
 * http://signalr.net/
 *
 * Copyright (c) .NET Foundation. All rights reserved.
 * Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
 *
 */

/// <reference path="..\..\SignalR.Client.JS\Scripts\jquery-1.6.4.js" />
/// <reference path="jquery.signalR.js" />
(function ($, window, undefined) {
    /// <param name="$" type="jQuery" />
    "use strict";

    if (typeof ($.signalR) !== "function") {
        throw new Error("SignalR: SignalR is not loaded. Please ensure jquery.signalR-x.js is referenced before ~/signalr/js.");
    }

    var signalR = $.signalR;

    function makeProxyCallback(hub, callback) {
        return function () {
            // Call the client hub method
            callback.apply(hub, $.makeArray(arguments));
        };
    }

    function registerScanHubProxies(instance, shouldSubscribe) {
        var key, hub, memberKey, memberValue, subscriptionMethod;

        for (key in instance) {
            if (instance.hasOwnProperty(key)) {
                hub = instance[key];

                if (!(hub.hubName)) {
                    // Not a client hub
                    continue;
                }

                if (shouldSubscribe) {
                    // We want to subscribe to the hub events
                    subscriptionMethod = hub.on;
                } else {
                    // We want to unsubscribe from the hub events
                    subscriptionMethod = hub.off;
                }

                // Loop through all members on the hub and find client hub functions to subscribe/unsubscribe
                for (memberKey in hub.client) {
                    if (hub.client.hasOwnProperty(memberKey)) {
                        memberValue = hub.client[memberKey];

                        if (!$.isFunction(memberValue)) {
                            // Not a client hub function
                            continue;
                        }

                        subscriptionMethod.call(hub, memberKey, makeProxyCallback(hub, memberValue));
                    }
                }
            }
        }
    }

    $.hubConnection.prototype.createScanHubProxies = function () {
        var proxies = {};
        this.starting(function () {
            // Register the hub proxies as subscribed
            // (instance, shouldSubscribe)
            registerScanHubProxies(proxies, true);

            this._registerSubscribedHubs();
        }).disconnected(function () {
            // Unsubscribe all hub proxies when we "disconnect".  This is to ensure that we do not re-add functional call backs.
            // (instance, shouldSubscribe)
            registerScanHubProxies(proxies, false);
        });

        proxies['scanHub'] = this.createHubProxy('scanHub');
        proxies['scanHub'].client = {};
        proxies['scanHub'].server = {
            openFileFtp: function (ftpServer, ftpLogin, ftpPassword, ftpFolder, dataServicesInfoId, fileId) {
                return proxies['scanHub'].invoke.apply(proxies['scanHub'], $.merge(["OpenFileFtp"], $.makeArray(arguments)));
            },
            scan: function (command, message) {
                return proxies['scanHub'].invoke.apply(proxies['scanHub'], $.merge(["Scan"], $.makeArray(arguments)));
            },

            selectDevice: function () {
                return proxies['scanHub'].invoke.apply(proxies['scanHub'], $.merge(["SelectDevice"], $.makeArray(arguments)));
            },

            closeApp: function () {
                return proxies['scanHub'].invoke.apply(proxies['scanHub'], $.merge(["CloseApp"], $.makeArray(arguments)));
            },

            uploadFTP: function () {
                return proxies['scanHub'].invoke.apply(proxies['scanHub'], $.merge(["UploadFTP"], $.makeArray(arguments)));
            }
        };

        return proxies;
    };

    signalR.scanAppHub = $.hubConnection("/signalr", { useDefaultPath: false });
    $.extend(signalR, signalR.scanAppHub.createScanHubProxies());

}(window.jQuery, window));