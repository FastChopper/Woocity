(function ($) {
    $.fn.dragsort = function (options) {
        var opts = $.extend({}, $.fn.dragsort.defaults, options);
        var $instance = $(this);
        if (opts.debug) {
            $('body').append('<div data-dragsort-debug style="position:fixed; bottom:0; right:0;  ">srcIdx:<span data-dragsort-debug-srcIdx></span> \
srcId:<span data-dragsort-debug-srcId></span> \
tarIdx:<span data-dragsort-debug-tarIdx></span> \
tarId:<span data-dragsort-debug-tarId></span> \
curIdx:<span data-dragsort-debug-curIdx></span> \
curId:<span data-dragsort-debug-curId></span> \
direction:<span data-dragsort-debug-direction></span></div>')
        }
        var onMoving = false; 
        var srcEle, srcId, srcIdx;
        var tarEle, tarId, tarIdx;
        $instance.find(opts.handler)
            .css('cursor', 'move')
            .mousedown(function (e) {
                if (e.which == 1) {
                    onMoving = true;
                    srcEle = $(this);
                    srcIdx = srcEle.index();
                    if (opts.idAttr == null) {
                        srcId = srcIdx;
                    }
                    else {
                        srcId = srcEle.attr(opts.idAttr);
                    }
                    tarEle = srcEle;
                    tarId = srcId;
                    tarIdx = srcIdx;
                    if (opts.debug) {
                        $('[data-dragsort-debug-srcIdx]').text(srcIdx);
                        $('[data-dragsort-debug-srcId]').text(srcId);
                        $('[data-dragsort-debug-tarIdx]').text(tarIdx);
                        $('[data-dragsort-debug-tarId]').text(tarId);
                    }
                    e.preventDefault();
                }
            })
            .mouseover(function (e) {
                if (onMoving && e.which == 1) {
                    var curEle = $(this);
                    var curIdx = curEle.index();
                    var curId;

                    if (curIdx != tarIdx) {
                        if (opts.idAttr == null) {
                            curId = curIdx;
                        }
                        else {
                            curId = curEle.attr(opts.idAttr);
                        }
                        tarEle = curEle;

                        if (tarIdx > curIdx) {
                            tarEle.before(srcEle);
                        }
                        else {
                            tarEle.after(srcEle);
                        }
                        tarId = curId;
                        tarIdx = curIdx;
                        if (srcIdx == tarIdx) {
                            tarId = srcId;
                            tarEle = srcEle;
                        }
                    }

                    if (opts.debug) {
                        $('[data-dragsort-debug-curIdx]').text(curIdx);
                        $('[data-dragsort-debug-curId]').text(curId);
                        $('[data-dragsort-debug-tarIdx]').text(tarIdx);
                        $('[data-dragsort-debug-tarId]').text(tarId);
                        $('[data-dragsort-debug-direction]').text(tarIdx - srcIdx);
                    }
                    e.preventDefault();
                }
            })
            .mouseup(function (e) {
                if (onMoving == true) {
                    onMoving = false;
                    opts.callback({
                        sourceId: srcId,
                        targetId: tarId,
                        sourceIndex: srcIdx,
                        targetIndex: tarIdx,
                        direction: tarIdx - srcIdx,
                        cancel: function () {
                            if (srcId != tarId) {
                                if (srcIdx < tarIdx){
                                    tarEle.before(srcEle);
                                }
                                else {
                                    tarEle.after(srcEle);
                                }
                            }
                        }
                    });
                    e.preventDefault();
                }
            });
    };
    $.fn.dragsort.defaults = {
        handler: '>*',
        callback: function (data) { },
        idAttr: null,
        debug: false
    };
})(jQuery);

