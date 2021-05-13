<%@ page language="java" contentType="text/html; charset=EUC-KR"
	pageEncoding="EUC-KR"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=EUC-KR">
<title>Insert title here</title>

<link href="http://u-campus.ajou.ac.kr/ltms/css/style.css"
	rel="stylesheet" type="text/css">
<script LANGUAGE="JavaScript"
	src="http://u-campus.ajou.ac.kr/ltms/js/siv.js"></script>
<script LANGUAGE="JavaScript"
	src="http://u-campus.ajou.ac.kr/ltms/js/prototype-1.5.1.2.js"></script>
<script src="http://code.jquery.com/jquery-1.9.1.min.js"></script>

<script>
	function init() {

	}

	function sendScroe() {
		alert('send');

		$.ajax({
			datatype : "json",
			method : "GET",
			url : "servlet",
			data : "option=score&id=" + $("#id").val() + "&score="
					+ $("#score").val(),
			success : function(jsonResp) {
				alert('sendScore');
				//var json_obj = $.parseJSON(jsonResp);
				//var str = json_obj.user_info;
				//$("#user_info").html("<xmp>" + str + "</xmp>");
			}
		});
	}

	function getTop() {
		alert('send');
		$.ajax({
			datatype : "json",
			method : "GET",
			url : "servlet",
			data : "option=getTop",
			success : function(jsonResp) {
				alert('getTop');
				alert(jsonResp);
				test_ajax = jQuery.parseJSON(jsonResp);
				
				for ( var i = 0 ; i < test_ajax.user_list.length ;  i++) 
				{
					alert(test_ajax.user_list[i].id + "  " + test_ajax.user_list[i].score);
				}		
			}
		});
	}
</script>

</head>

<body onload="init()">
	<form name="update_form" action="" method="post" onsubmit="">
		<input type="hidden" name="ajou_usr_idno" />

		<div class="divID">
			<p>SCORE TEST
			<p>
				ID <input id="id" value="" /> SCORE <input id="score" value="" />
				<input type="button" value="SendScore" onclick="sendScroe();" />
		</div>

		<div class="divID">
			<p>GET TOP
			<p>
				<input type="button" value="GetTop" onclick="getTop();" />
		</div>
	</form>

</body>
</html>