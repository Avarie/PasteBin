angular.module('main')
    .controller('MainController', ['$scope', '$http', function ($scope, $http) {

        var canvas = "img_canvas";
        $scope.imageSet = false;
        $scope.text = '';
        $scope.isInProgress = false;

        $scope.publish = function () {
            $scope.isInProgress = true;
            var image64 = document.getElementById(canvas).toDataURL();
            $http({ method: 'POST', url: '/Copy/Publish', 
                data: {
                    html: $scope.text != '' ? $scope.text : null,
                    image: $scope.imageSet ? image64 : null
                }
            })
                .then(function(r) { 
                    window.location.assign("copy/paste/" + r.data.Id);
            });
        }

        // taken from http://stackoverflow.com/questions/18377891/how-can-i-let-user-paste-image-data-from-the-clipboard-into-a-canvas-element-in
        clipboardClass(canvas, true);

        /**
         * image pasting into canvas
         * 
         * @param {string} canvas_id - canvas id
         * @param {boolean} autoresize - if canvas will be resized
         */
        function clipboardClass(canvas_id, autoresize) {

            var _self = this;
            var canvas = document.getElementById(canvas_id);
            var ctx = document.getElementById(canvas_id).getContext("2d");
            var ctrl_pressed = false;
            var command_pressed = false;
            var paste_event_support;
            var pasteCatcher;

            //handlers
            document.addEventListener('keydown', function (e) {
                _self.on_keyboard_action(e);
            }, false); //firefox fix
            document.addEventListener('keyup', function (e) {
                _self.on_keyboardup_action(e);
            }, false); //firefox fix
            document.addEventListener('paste', function (e) {
                _self.paste_auto(e);
            }, false); //official paste handler

            //constructor - we ignore security checks here
            this.init = function () {
                pasteCatcher = document.createElement("div");
                pasteCatcher.setAttribute("id", "paste_ff");
                pasteCatcher.setAttribute("contenteditable", "");
                pasteCatcher.style.cssText = 'opacity:0;position:fixed;top:0px;left:0px;width:10px;margin-left:-20px;';
                document.body.appendChild(pasteCatcher);

                // create an observer instance
                var observer = new MutationObserver(function (mutations) {
                    mutations.forEach(function (mutation) {
                        if (paste_event_support === true || ctrl_pressed == false || mutation.type != 'childList') {
                            //we already got data in paste_auto()
                            return true;
                        }

                        //if paste handle failed - capture pasted object manually
                        if (mutation.addedNodes.length == 1) {
                            if (mutation.addedNodes[0].src != undefined) {
                                //image
                                _self.paste_createImage(mutation.addedNodes[0].src);
                            }
                            //register cleanup after some time.
                            setTimeout(function () {
                                pasteCatcher.innerHTML = '';
                            }, 20);
                        }
                    });
                });
                var target = document.getElementById('paste_ff');
                var config = { attributes: true, childList: true, characterData: true };
                observer.observe(target, config);
            }();
            //default paste action
            this.paste_auto = function (e) {
                paste_event_support = false;
                if (pasteCatcher != undefined) {
                    pasteCatcher.innerHTML = '';
                }
                if (e.clipboardData) {
                    var items = e.clipboardData.items;
                    if (items) {
                        paste_event_support = true;
                        //access data directly
                        for (var i = 0; i < items.length; i++) {
                            if (items[i].type.indexOf("image") !== -1) {
                                //image
                                var blob = items[i].getAsFile();
                                var urlObj = window.URL || window.webkitURL;
                                var source = urlObj.createObjectURL(blob);
                                this.paste_createImage(source);
                            }
                        }
                        e.preventDefault();
                    }
                    else {
                        //wait for DOMSubtreeModified event
                        //https://bugzilla.mozilla.org/show_bug.cgi?id=891247
                    }
                }
            };
            //on keyboard press
            this.on_keyboard_action = function (event) {
                var k = event.keyCode;
                //ctrl
                if (k == 17 || event.metaKey || event.ctrlKey) {
                    if (ctrl_pressed == false)
                        ctrl_pressed = true;
                }
                //v
                if (k == 86) {
                    if (document.activeElement != undefined && document.activeElement.type == 'text') {
                        //let user paste into some input
                        return false;
                    }

                    if (ctrl_pressed == true && pasteCatcher != undefined) {
                        pasteCatcher.focus();
                    }
                }
            };
            //on kaybord release
            this.on_keyboardup_action = function (event) {
                //ctrl
                if (event.ctrlKey == false && ctrl_pressed == true) {
                    ctrl_pressed = false;
                }
                    //command
                else if (event.metaKey == false && command_pressed == true) {
                    command_pressed = false;
                    ctrl_pressed = false;
                }
            };
            //draw pasted image to canvas
            this.paste_createImage = function (source) {
                var pastedImage = new Image();
                pastedImage.onload = function () {
                    if (autoresize == true) {
                        //resize
                        canvas.width = pastedImage.width;
                        canvas.height = pastedImage.height;
                    }
                    else {
                        //clear canvas
                        ctx.clearRect(0, 0, canvas.width, canvas.height);
                    }

                    $scope.imageSet = true;
                    $scope.$apply();

                    ctx.drawImage(pastedImage, 0, 0);
                };
                pastedImage.src = source;
            };
        }

    }])
;