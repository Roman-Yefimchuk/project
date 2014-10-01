"use strict";

(function (require) {

    var express = require('express');
    var app = express();
    var server = require('http').Server(app);

    var http = require("http");
    var fs = require("fs");

    var loadIndex = (function () {

        var indexFile = null;

        function getIndexFile(callback) {
            fs.readFile('app/index.html', "binary", function (error, file) {
                if (error) {
                    callback.failure(error);
                } else {
                    callback.success(file);
                }
            });
        }

        return function (request, response) {

            function sendIndexFile(file) {
                response.writeHead(200);
                response.write(file, "binary");
                response.end();
            }

            if (indexFile) {
                sendIndexFile(indexFile);
            } else {
                getIndexFile({
                    success: function (file) {
                        indexFile = file;
                        sendIndexFile(file);
                    },
                    failure: function (error) {
                        response.writeHead(500, {
                            "Content-Type": "text/plain"
                        });
                        response.write(error);
                        response.end();
                    }
                });
            }
        }
    })();

    app.configure(function () {
        var staticPath = express.static(__dirname);
        app.use(staticPath);
    });

    app.get('/', function (request, response) {
        loadIndex(request, response);
    });

    server.listen(8888);

    console.log('Static server started successful');

})(require);