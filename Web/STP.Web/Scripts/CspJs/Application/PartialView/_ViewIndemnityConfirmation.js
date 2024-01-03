                                                {
                                                    $(document).ready(function () {
                                                        startAnimation();
                                                        Resize_PopUp(540);
                                                        $("#IDcloseMp").on('click', closeMp);
                                                        $('#span-close').click(function () {
                                                            $('#overlay').hide();
                                                            addscroll();
                                                            resetdialogue();

                                                        });
                                                        $('#PrintPage').click(function () {
                                                            var link = "../Movements/IndemnityConfirmation?notificationId=" + $('#NotificationId').val() + "";
                                                            window.open(link, '_blank');
                                                        });
                                                        stopAnimation();
                                                        removescroll();
                                                        $("#dialogue").show();
                                                        $("#overlay").show();
                                                    });
                                                    function closeSpan() {
                                                        $('#overlay').hide();
                                                        addscroll();
                                                        resetdialogue();
                                                    }
                                                }
                                                function closeMp() {
                                                    $('#contactDetails').modal('hide');
                                                    $("#overlay").hide();

                                                }
